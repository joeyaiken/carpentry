using Carpentry.CarpentryData;
using Carpentry.CarpentryData.Models;
using Carpentry.Logic.Models;
using Carpentry.Logic.Models.Backups;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.Extensions.Logging;
using SQLitePCL;

namespace Carpentry.Logic
{
    public interface IDataImportService
    {
        //public async ValidatedCardImportDto ValidateImport([Raw]CardImportDto)
        //public async void AddValidatedImport(ValidatedCardImportDto)

        //deck import
        Task<ValidatedDeckImportDto> ValidateDeckImport(CardImportDto payload);
        Task<int> AddValidatedDeckImport(ValidatedDeckImportDto validatedPayload);

        //inventory import
        Task<ValidatedCarpentryImportDto> ValidateCarpentryImport(CardImportDto payload);
        Task AddValidatedCarpentryImport(ValidatedCarpentryImportDto payload);
    }

    public class LandType
    {
        private LandType(string value) { Value = value; }
        public string Value { get; set; }
        public static LandType Plains { get { return new LandType("Plains"); } }
        public static LandType Island { get { return new LandType("Island"); } }
        public static LandType Swamp { get { return new LandType("Swamp"); } }
        public static LandType Mountain { get { return new LandType("Mountain"); } }
        public static LandType Forest { get { return new LandType("Forest"); } }
    }

    

    //Maybe this SHOULD have access to the repo
    public class DataImportService : IDataImportService
    {
        private readonly IDataUpdateService _dataUpdateService;
        private readonly IDeckService _deckService;
        private readonly IInventoryService _trimmingToolService;
        private readonly int _cardStatus_InInventory = 1;
        private readonly IDataBackupConfig _config;

        private readonly CarpentryDataContext _cardContext;

        private static List<string> BasicLands = new List<string>() { "Plains", "Island", "Swamp", "Mountain", "Forest" };
        public DataImportService(
            IDataUpdateService dataUpdateService,
            IDeckService deckService,
            IInventoryService trimmingToolService,
            IDataBackupConfig config,
            CarpentryDataContext cardContext
            )
        {
            _dataUpdateService = dataUpdateService;
            _deckService = deckService;
            _trimmingToolService = trimmingToolService;
            _config = config;
            _cardContext = cardContext;
        }

        /// <summary>
        /// (old)Used by the Quick Import Tool to import a deck(old)
        /// 
        /// Parses & validates a string deck list
        /// Returns a list of validation objects containing parsed data & validation results
        /// (1 object for each card line)
        /// </summary>
        /// <param name="payload"></param>
        /// <returns></returns>
        public async Task<ValidatedDeckImportDto> ValidateDeckImport(CardImportDto payload)
        {
            var result = new ValidatedDeckImportDto() { IsValid = true };

            //Get paload split into lines
            string[] importRows = payload.ImportPayload.Split(Environment.NewLine);

            if(importRows.Count() == 1)
            {
                var otherNewline = payload.ImportPayload.Split('\n');
                if(otherNewline.Count() > 1) importRows = otherNewline;
            }

            //var mappedRecords = new List<ImportListRecord>();
            var mappedRecords = new List<ValidatedCardDto>();

            char? category = null;

            foreach (var row in importRows)
            {
                //is this an empty line?
                if (string.IsNullOrWhiteSpace(row)) continue;

                if (row == "Deck")
                {
                    category = null;
                    continue;
                };
                if (row == "Commander")
                {
                    category = 'c';
                    continue;
                };
                if (row == "Sideboard")
                {
                    category = 's';
                    continue;
                };

                mappedRecords.Add(ParseImportListRecord(row, category));

                //try
                //{
                //    mappedRecords.Add(ParseImportListRecord(row, category));
                //}
                //catch
                //{
                //    result.InvalidRows.Add(row);
                //}   
            }

            var distinctSetCodes = mappedRecords.Where(c => !c.IsBasicLand && !c.IsEmpty).Select(x => x.SetCode).Distinct().ToList();

            foreach (var code in distinctSetCodes)
            {
                //will just return silently if the set is already tracked
                var set = await _cardContext.Sets.FirstOrDefaultAsync(s => s.Code == code.ToLower());
                if (!set.IsTracked)
                {
                    result.UntrackedSets.Add(new ValidatedDtoUntrackedSet() { SetId = set.SetId, SetCode = set.Code });
                }
                //await _dataUpdateService.AddTrackedSet(set.Id);
            }

            if (result.UntrackedSets.Any()) result.IsValid = false;

            //List<ValidatedCardDto> validatedCards = new List<ValidatedCardDto>();

            //for each card, get the matching DB card by Name+Code (from the carpentry DB)
            foreach (var card in mappedRecords)
            {

                //For each basic land, just continue
                if (card.IsBasicLand) continue;

                if (card.IsEmpty)
                {
                    //For each valid, EMPTY card, I need to ensure there's at least 1 card with the same name in the db
                    var namedCardCount = await _cardContext.Cards.Where(x => x.Name == card.Name).CountAsync();
                    if (namedCardCount == 0) card.IsValid = false;
                }
                else
                {
                    //for each valid, non-empty card, I need to get the CardId for the instance specified

                    //I can't add a card for an untracked set
                    var recordIsUntracked = result.UntrackedSets.Any(s => s.SetCode == card.SetCode);
                    if (recordIsUntracked)
                    {
                        //result.InvalidCards.Add(card);
                        card.IsValid = false;
                        continue;
                    }

                    //I feel like this could be cleaner...
                    try
                    {
                        var newCard = await _cardContext.Cards
                           .Where(x => x.Set.Code.ToLower() == card.SetCode.ToLower() && x.Name.ToLower() == card.Name.ToLower())
                           .Select(matchingCard => new ValidatedCardDto()
                           {
                               CardId = matchingCard.CardId,
                               Name = matchingCard.Name,
                               SetCode = matchingCard.Set.Code,
                               CollectorNumber = matchingCard.CollectorNumber, //
                               IsFoil = card.IsFoil,
                           })
                           .FirstOrDefaultAsync();

                        if (newCard == null)
                        {
                            card.IsValid = false;
                            continue;
                        }
                        
                        for (var i = 0; i < card.Count; i++)
                        {
                            result.ValidatedCards.Add(newCard);
                        }
                    }
                    catch
                    {
                        card.IsValid = false;
                        //result.InvalidCards.Add(card);
                        //result.IsValid = false;
                    }
                }
            }

            if (mappedRecords.Any(c => !c.IsValid))
            {
                result.InvalidCards = mappedRecords.Where(c => !c.IsValid).ToList();
                result.IsValid = false;
            }

            //only pull land counts if all cards are valid
            if (result.IsValid)
            {
                result.DeckProps.BasicW = PullLandCount(LandType.Plains, mappedRecords);
                result.DeckProps.BasicU = PullLandCount(LandType.Island, mappedRecords);
                result.DeckProps.BasicB = PullLandCount(LandType.Swamp, mappedRecords);
                result.DeckProps.BasicR = PullLandCount(LandType.Mountain, mappedRecords);
                result.DeckProps.BasicG = PullLandCount(LandType.Forest, mappedRecords);
            }

            return result;
        }
        public async Task<int> AddValidatedDeckImport(ValidatedDeckImportDto validatedPayload)
        {
            //If deck == null, just adding inventory cards
            if (validatedPayload.DeckProps == null)
            {
                //just create a list of inventory cards to add
                var cardBatch = validatedPayload.ValidatedCards
                    .Select(x => new NewInventoryCard()
                    {
                        //MultiverseId = x.MultiverseId,
                        StatusId = _cardStatus_InInventory,
                        IsFoil = x.IsFoil,
                        //VariantName = x.VariantName,
                    })
                    //.Select(c => new InventoryCardData()
                    //{
                    //    IsFoil = c.IsFoil,
                    //    InventoryCardStatusId = _cardStatus_InInventory,
                    //    //CardId = c.CardId
                    //})
                    .ToList();

                await _trimmingToolService.AddInventoryCardBatch(cardBatch); // TODO - consider removing this bit of logic, it would mean the inventory service could be removed from this completely

                return 0;
            }

            int deckId = validatedPayload.DeckProps.Id;

            //if deck doesn't exist, add it first
            if (deckId == 0)
            {
                deckId = await _deckService.AddDeck(validatedPayload.DeckProps);
            }
            else //else update the existing one (in case we changed the land count)
            {
                await _deckService.UpdateDeck(validatedPayload.DeckProps);
            }

            List<DeckCardDto> cardsToAdd = validatedPayload.ValidatedCards.Select(c => new DeckCardDto()
            {
                DeckId = deckId,
                CardName = c.Name,
                IsFoil = c.IsFoil,
                CardId = c.CardId,
                InventoryCardStatusId = _cardStatus_InInventory,
                CategoryId = null,
                InventoryCardId = 0, //new inventory card
                Id = 0, //new card
            }).ToList();

            await _deckService.AddDeckCardBatch(cardsToAdd);

            return deckId;
        }



        /// <summary>
        /// A carpentry import (dto) is just a directory where backups can be found
        /// maybe some day this can be refactored to take a file
        /// </summary>
        /// <param name="payload"></param>
        /// <returns></returns>
        public async Task<ValidatedCarpentryImportDto> ValidateCarpentryImport(CardImportDto payload)
        {
            //TODO - Is there a better way to submit a file than reading from a directory?
            var result = new ValidatedCarpentryImportDto()
            {
                BackupDirectory = payload.ImportPayload,
                //BackupDate = null,
                UntrackedSets = new List<ValidatedDtoUntrackedSet>(),
            };

            //For now I'm just going to use those raw text files
            var propsBackupLocation = $"{payload.ImportPayload}{_config.PropsBackupFilename}";
            var propsBackupDataString = await System.IO.File.ReadAllTextAsync(propsBackupLocation);
            var backupProps = JObject.Parse(propsBackupDataString).ToObject<BackupDataProps>();

            result.BackupDate = backupProps.TimeStamp;

            //Getting all sets
            var allSets = await _dataUpdateService.GetTrackedSets(true, false);
            //TODO - I feel like this can be refactored to use less itterations
            //remember this is already looking at the result of a query
            foreach(var setCode in backupProps.SetCodes)
            {
                var setTracking = allSets.Where(s => s.Code.ToLower() == setCode.ToLower()).FirstOrDefault();
                if (!setTracking.IsTracked)
                {
                    result.UntrackedSets.Add(new ValidatedDtoUntrackedSet() 
                    {  
                        SetCode = setTracking.Code, 
                        SetId = setTracking.SetId 
                    });
                }
            }

            //ignoring the payload type for now

            //verify the directory exists

            //Extract the Backup Props file

            //see what sets the Backup Props are still untracked

            //
            return result;
        }
        public async Task AddValidatedCarpentryImport(ValidatedCarpentryImportDto payload)
        {
            //inventory cards must be loaded before deck cards
            #region Load Card Backups

            string cardBackupLocation = $"{payload.BackupDirectory}{_config.CardBackupFilename}";
            string cardBackupsDataString = await System.IO.File.ReadAllTextAsync(cardBackupLocation);
            List<BackupInventoryCard> parseCardsBackups = JArray.Parse(cardBackupsDataString).ToObject<List<BackupInventoryCard>>();

            //_logger.LogWarning("RestoreDb - LoadCardBackups...definitions exist, mapping & saving");

            //I think this JOIN is the source of my current bug, it broke from CollectionNumber refactoring
            var mappedInventoryCards = parseCardsBackups
                .Join(_cardContext.Cards.AsQueryable(),
                    backup => new { CollectorNumberStr = backup.CollectorNumberStr, SetCode = backup.SetCode },
                    card => new { CollectorNumberStr = card.CollectorNumberStr, SetCode = card.Set.Code },
                    (backup, card) => new
                    {
                        Backup = backup,
                        Card = card,
                    })
                .Select(x => new InventoryCardData
                {
                    InventoryCardStatusId = x.Backup.InventoryCardStatusId,
                    IsFoil = x.Backup.IsFoil,
                    CardId = x.Card.CardId,
                }).ToList();

            _cardContext.InventoryCards.AddRange(mappedInventoryCards);

            await _cardContext.SaveChangesAsync();

            //_logger.LogWarning("RestoreDb - LoadCardBackups...COMPLETE!");

            #endregion

            #region Load Deck Backups

            string deckBackupLocation = $"{payload.BackupDirectory}{_config.DeckBackupFilename}";

            //_logger.LogWarning("LoadDeckBackups - Loading parsed decks");

            var formats = await _cardContext.MagicFormats.ToDictionaryAsync(f => f.Name, f => f);

            string deckBackupsDataString = await System.IO.File.ReadAllTextAsync(deckBackupLocation);
            List<BackupDeck> parsedBackupDecks = JArray.Parse(deckBackupsDataString).ToObject<List<BackupDeck>>();

            /*
                Ways to get unused inventory cards:
                    Query once for each deck card (kinda eww)
                    Get possible matches by...name or something?
                        I COULD get all distinct names, then get all (unused) inventory cards by that name
                        ATM all inventory cards 'should' be unused inventory cards
             */

            var allCardNames = parsedBackupDecks.SelectMany(d => d.Cards.Select(c => c.Name)).Distinct();


            var unusedCards = await _cardContext.GetInventoryCardsByName(allCardNames);

            //running off the assumption that, since this is only used by import atm, I can assume no cards are in a deck

            //I want to submit a list of card names, and get a collection of inventory cards for each name

            var newDeckList = new List<DeckData>();

            foreach (var parsedDeck in parsedBackupDecks)
            {
                var newDeck = new DeckData()
                {
                    Name = parsedDeck.Name,
                    Notes = parsedDeck.Notes,
                    Format = formats[parsedDeck.Format],
                    BasicB = parsedDeck.BasicB,
                    BasicG = parsedDeck.BasicG,
                    BasicR = parsedDeck.BasicR,
                    BasicU = parsedDeck.BasicU,
                    BasicW = parsedDeck.BasicW,
                    Cards = new List<DeckCardData>(),
                };

                foreach (var parsedDeckCard in parsedDeck.Cards)
                {
                    var newDeckCard = new DeckCardData
                    {
                        CardName = parsedDeckCard.Name,
                        CategoryId = parsedDeckCard.Category,
                    };

                    if (parsedDeckCard.InventoryCard != null)
                    {
                        var localUnusedCards = unusedCards[parsedDeckCard.Name];

                        var parsedInvCard = parsedDeckCard.InventoryCard;

                        var matchingUnusedCard = localUnusedCards.First(ic =>
                            ic.Card.Set.Code == parsedDeckCard.InventoryCard.SetCode
                            && ic.IsFoil == parsedDeckCard.InventoryCard.IsFoil
                            && ic.Card.CollectorNumberStr == parsedDeckCard.InventoryCard.CollectorNumberStr
                            && ic.InventoryCardStatusId == parsedDeckCard.InventoryCard.InventoryCardStatusId
                        );

                        unusedCards[parsedDeckCard.Name].Remove(matchingUnusedCard);

                        //get matching unused inventory card id
                        //var inventoryCardId = 0;

                        //So, I only want each inventory card to belong to at most 1 deck card
                        //Is EF smart enough to track this all before SaveChangesAsync?...
                        //I should try to avoid a query per deck-card


                        newDeckCard.InventoryCardId = matchingUnusedCard.InventoryCardId;
                    }

                    newDeck.Cards.Add(newDeckCard);
                }
                newDeckList.Add(newDeck);
            }

            _cardContext.Decks.AddRange(newDeckList);

            await _cardContext.SaveChangesAsync();

            #endregion
        }

        /// <summary>
        /// Parses an ImportListRecrd object from a string
        /// </summary>
        /// <param name="recordString">String to parse, assuming Arena style record</param>
        /// <returns>The mapped ImportLiistRecord object</returns>
        private static ValidatedCardDto ParseImportListRecord(string recordString, char? category = null)
        {
            var mappedRecord = new ValidatedCardDto()
            {
                Category = category,
                SourceString = recordString,
                IsValid = true,
            };

            var importLineTokens = recordString.Split(' ').ToList();

            if (importLineTokens.Count() < 2)
            {
                mappedRecord.IsValid = false;
                return mappedRecord;
                //throw new Exception("Bad Data: Expected at least 2 tokens in a line.");
            }
            
            //first token will be the count
            if (int.TryParse(importLineTokens[0], out int parsedCount))
            {
                mappedRecord.Count = parsedCount;
                importLineTokens.RemoveAt(0);
            }

            //check if the last token is a tag list
            var tagTokenChars = importLineTokens.Last().ToCharArray();
            if (tagTokenChars[0] == '{' && tagTokenChars[tagTokenChars.Length - 1] == '}')
            {
                var encodedTagList = importLineTokens.Last().Substring(1, tagTokenChars.Length - 2);// In reality Substring should always take 3
                var tagList = WebUtility.UrlDecode(encodedTagList);
                mappedRecord.Tags = tagList.Split(',').ToList();
                importLineTokens.RemoveAt(importLineTokens.Count() - 1);
                //mappedRecord.IsEmpty = false;



                //var tagString = string.Join(',', dc.Tags);
                //var encodedString = WebUtility.UrlEncode(tagString);
                //cardString = $"{cardString} {{{ encodedString }}}";
            }

            //Check if the last token is "FOIL"
            if (importLineTokens.Last().ToLower() == "foil")
            {
                mappedRecord.IsFoil = true;
                importLineTokens.RemoveAt(importLineTokens.Count() - 1);
            }


            //Is the next line a #?
            if(int.TryParse(importLineTokens.Last(), out int parsedCollectorNumber))
            {
                mappedRecord.CollectorNumber = parsedCollectorNumber;
                importLineTokens.RemoveAt(importLineTokens.Count() - 1);
            }

            //next might be the set code
            var lastTokenChars = importLineTokens.Last().ToCharArray();
            if(lastTokenChars[0] == '(' && lastTokenChars[lastTokenChars.Length-1] == ')')
            {
                mappedRecord.SetCode = importLineTokens.Last().Substring(1, lastTokenChars.Length - 2);// In reality Substring should always take 3
                importLineTokens.RemoveAt(importLineTokens.Count() - 1);
                mappedRecord.IsEmpty = false;
            }

            //Everything else should be the name
            mappedRecord.Name = string.Join(' ', importLineTokens);

            //if the name matches a basic land name, clear set/num/foil fields
            //var basicLands = new List<string>() { "Plains","Island","Swamp","Mountain","Forest" };
            if (BasicLands.Contains(mappedRecord.Name)) mappedRecord.IsBasicLand = true;

            return mappedRecord;
        }

        private int PullLandCount(LandType landType, List<ValidatedCardDto> cards)
        {
            var basicLands = cards.FirstOrDefault(x => x.Name == landType.Value);
            if (basicLands != null)
            {
                cards.Remove(basicLands);
                return basicLands.Count;
            }
            else return 0;
        }

    }
}
