using Carpentry.Data.DataModels;
using Carpentry.Data.Interfaces;
using Carpentry.Data.QueryResults;
using Carpentry.Logic.Interfaces;
using Carpentry.Logic.Models;
using Carpentry.Logic.Models.Backups;
using Microsoft.EntityFrameworkCore.Internal;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carpentry.Logic.Implementations
{
    
    public class ImportListRecord
    {
        public int Count { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public int Number { get; set; }
        //Maybe I change the Number on select records to "{number}_FOIL"
        //There will be 1-3 foil cards per deck I'm adding (3 for commander, 1 for brawl)
        public bool IsFoil { get; set; }
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
        private readonly ICardDataRepo _cardDataRepo;
        private readonly IDeckService _deckService;
        private readonly IDeckDataRepo _deckDataRepo;
        private readonly IInventoryService _inventoryService;
        private readonly IInventoryDataRepo _inventoryRepo;
        private readonly int _cardStatus_InInventory = 1;
        private readonly IDataBackupConfig _config;
        public DataImportService(
            IDataUpdateService dataUpdateService,
            ICardDataRepo cardDataRepo,
            IDeckService deckService,
            IDeckDataRepo deckDataRepo,
            IInventoryService inventoryService,
            IInventoryDataRepo inventoryRepo,
            IDataBackupConfig config
            )
        {
            _dataUpdateService = dataUpdateService;
            _cardDataRepo = cardDataRepo;
            _deckService = deckService;
            _deckDataRepo = deckDataRepo;
            _inventoryService = inventoryService;
            _inventoryRepo = inventoryRepo;
            _config = config;
        }

        /// <summary>
        /// Used by the Quick Import Tool to import a deck
        /// </summary>
        /// <param name="payload"></param>
        /// <returns></returns>
        public async Task<ValidatedDeckImportDto> ValidateDeckImport(CardImportDto payload)
        {
            var result = new ValidatedDeckImportDto();

            //Get paload split into lines
            string[] importRows = payload.ImportPayload.Split(Environment.NewLine);

            var mappedRecords = new List<ImportListRecord>();

            foreach(var row in importRows)
            {
                mappedRecords.Add(ParseImportListRecord(row));
            }
            //importRows.Select(line => ParseImportListRecord(line)).ToList();

            //DeckPropertiesDto deckProps = new DeckPropertiesDto();

            result.DeckProps.BasicW = PullLandCount(LandType.Plains, mappedRecords);
            result.DeckProps.BasicU = PullLandCount(LandType.Island, mappedRecords);
            result.DeckProps.BasicB = PullLandCount(LandType.Swamp, mappedRecords);
            result.DeckProps.BasicR = PullLandCount(LandType.Mountain, mappedRecords);
            result.DeckProps.BasicG = PullLandCount(LandType.Forest, mappedRecords);

            var distinctSetCodes = mappedRecords.Select(x => x.Code).Distinct().ToList();

            foreach (var code in distinctSetCodes)
            {
                //will just return silently if the set is already tracked
                var set = await _cardDataRepo.GetCardSetByCode(code);
                if (!set.IsTracked)
                {
                    result.UntrackedSets.Add(new ValidatedDtoUntrackedSet() { SetId = set.SetId, SetCode = set.Code });
                }
                //await _dataUpdateService.AddTrackedSet(set.Id);
            }

            //List<ValidatedCardDto> validatedCards = new List<ValidatedCardDto>();

            //for each card, get the matching DB card by Name+Code (from the carpentry DB)
            foreach (var card in mappedRecords)
            {
                var matchingCard = await _cardDataRepo.GetCardData(card.Name, card.Code);

                //if (matchingCard == null) continue;

                ValidatedCardDto newCard = new ValidatedCardDto()
                {
                    CardId = matchingCard.CardId,
                    Name = matchingCard.Name,
                    SetCode = matchingCard.Set.Code,
                    CollectorNumber = matchingCard.CollectorNumber,
                    IsFoil = card.IsFoil,
                };

                for (int i = 0; i < card.Count; i++)
                {
                    result.ValidatedCards.Add(newCard);
                }
            }

            return result;
        }
        public async Task AddValidatedDeckImport(ValidatedDeckImportDto validatedPayload)
        {
            //If deck == null, just adding inventory cards
            if (validatedPayload.DeckProps == null)
            {
                //just create a list of inventory cards to add
                List<InventoryCardDto> cardBatch = validatedPayload.ValidatedCards
                    .Select(x => new InventoryCardDto()
                    {
                        //MultiverseId = x.MultiverseId,
                        StatusId = _cardStatus_InInventory,
                        IsFoil = x.IsFoil,
                        //VariantName = x.VariantName,
                    })
                    .ToList();

                await _inventoryService.AddInventoryCardBatch(cardBatch); // TODO - consider removing this bit of logic, it would mean the inventory service could be removed from this completely

                return;
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
            string propsBackupLocation = $"{payload.ImportPayload}{_config.PropsBackupFilename}";
            var backupProps = await LoadBackupProps(propsBackupLocation);

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
            //new approach requires inventory cards to be loaded before deck cards

            //inv cards
            string cardBackupLocation = $"{payload.BackupDirectory}{_config.CardBackupFilename}";
            await LoadCardBackups(cardBackupLocation);

            //deck cards
            string deckBackupLocation = $"{payload.BackupDirectory}{_config.DeckBackupFilename}";
            await LoadDeckBackups(deckBackupLocation);

            //Note: Required card definitions should exist at this point

            //decks to import

            //cards to import
        }


        private async Task<BackupDataProps> LoadBackupProps(string directory)
        {
            //Ensuring all sets in Props are properly tracked
            //_logger.LogInformation($"LoadTrackedSets - begin");
            //string propsBackupLocation = ""; // $"{_config.BackupDirectory}{_config.PropsBackupFilename}";

            string propsBackupDataString = await System.IO.File.ReadAllTextAsync(directory);

            BackupDataProps parsedPropsBackups = JObject.Parse(propsBackupDataString).ToObject<BackupDataProps>();

            //foreach (var setCode in parsedPropsBackups.SetCodes)
            //{
            //    _logger.LogInformation($"LoadTrackedSets - loading {setCode}");
            //    var set = await _cardDataRepo.GetCardSetByCode(setCode);
            //    await _dataUpdateService.AddTrackedSet(set.Id);
            //}

            //_logger.LogInformation($"LoadTrackedSets - complete");

            return parsedPropsBackups;
        }

        private async Task LoadDeckBackups(string directory)
        {
            //throw new NotImplementedException("Updates/review required");
            //int existingDeckCount = _cardContext.Decks.Select(x => x.Id).Count();
            //int existingDeckCount = (await _deckDataRepo.GetAllDecks()).Count();
            //if (existingDeckCount > 0)
            //{
            //    _logger.LogWarning("LoadDeckBackups - Decks already exist, not loading anything from parsed data");
            //    return;
            //}

            //_logger.LogWarning("LoadDeckBackups - Loading parsed decks");

            var formats = await _cardDataRepo.GetAllFormatsByName();

            string deckBackupsDataString = await System.IO.File.ReadAllTextAsync(directory);
            List<BackupDeck> parsedBackupDecks = JArray.Parse(deckBackupsDataString).ToObject<List<BackupDeck>>();

            /*
                Ways to get unused inventory cards:
                    Query once for each deck card (kinda eww)
                    Get possible matches by...name or something?
                        I COULD get all distinct names, then get all (unused) inventory cards by that name
                        ATM all inventory cards 'should' be unused inventory cards

             
             
             */

            var allCardNames = parsedBackupDecks.SelectMany(d => d.Cards.Select(c => c.Name)).Distinct();


            var unusedCards = await _inventoryRepo.GetUnusedInventoryCards(allCardNames);

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
                            && ic.Card.CollectorNumber == parsedDeckCard.InventoryCard.CollectorNumber
                            && ic.InventoryCardStatusId == parsedDeckCard.InventoryCard.InventoryCardStatusId
                        );

                        unusedCards[parsedDeckCard.Name].Remove(matchingUnusedCard);



                        //get matching unused inventory card id
                        //var inventoryCardId = 0;

                        //So, I only want each inventory card to belong to at most 1 deck card
                        //Is EF smart enough to track this all before SaveChangesAsync?...
                        //I should try to avoid a query per deck-card





                        //newDeckCard.InventoryCardId = inventoryCardId;
                        newDeckCard.InventoryCardId = matchingUnusedCard.InventoryCardId;
                    }
                    
                    newDeck.Cards.Add(newDeckCard);
                }
                newDeckList.Add(newDeck);
            }

            await _deckDataRepo.AddImportedDeckBatch(newDeckList);

            //var newerDecks = parsedBackupDecks.Select(x => new DeckData
            //{
            //    Name = x.Name,
            //    Notes = x.Notes,
            //    //Format = x.Format,
            //    BasicB = x.BasicB,
            //    BasicG = x.BasicG,
            //    BasicR = x.BasicR,
            //    BasicU = x.BasicU,
            //    BasicW = x.BasicW,
            //    Cards = x.Cards.Select(dc => new DeckCardData
            //    {
            //        CardName = dc.Name,
            //        CategoryId = dc.Category,
                    
            //    }).ToList(),
            //}).ToList();





            //List<DeckPropertiesDto> newDecks = parsedBackupDecks.Select(x => new DeckPropertiesDto()
            //{
            //    BasicB = x.BasicB,
            //    BasicG = x.BasicG,
            //    BasicR = x.BasicR,
            //    BasicU = x.BasicU,
            //    BasicW = x.BasicW,
            //    Name = x.Name,
            //    Notes = x.Notes,
            //    Format = x.Format,
                
            //    //Id = x.ExportId
            //}).ToList();
            //throw new NotImplementedException("that shouldn't be used anymore");
            //await _deckService.AddImportedDeckBatch(newDecks);

            //List<Task<int>> newDeckTasks = newDecks.Select(deck => _deckDataRepo.AddDeck(deck)).ToList();

            //await Task.WhenAll(newDeckTasks);

            //_logger.LogWarning("LoadDeckBackups - Complete");
        }

        private async Task LoadCardBackups(string directory) //TODO - Should this take a string payload instead?
        {
            //throw new NotImplementedException("Updates/review required");

            //I don't need to check if card definitions exist anymore
            //If I own a card from a set, all definitions for that set should exist in the DB at this point
            //I can safely grab any backup card by MID

            //bool cardsExist = await _inventoryDataRepo.DoInventoryCardsExist();
            //if (cardsExist)
            //{
            //    _logger.LogWarning("LoadCardBackups - card data already exists, returning");
            //    return;
            //}
            //_logger.LogWarning("LoadCardBackups - preparing to load backups...");

            //string cardBackupLocation = ""; // $"{_config.BackupDirectory}{_config.CardBackupFilename}";

            string cardBackupsDataString = await System.IO.File.ReadAllTextAsync(directory);
            List<BackupInventoryCard> parseCardsBackups = JArray.Parse(cardBackupsDataString).ToObject<List<BackupInventoryCard>>();

            //_logger.LogWarning("RestoreDb - LoadCardBackups...definitions exist, mapping & saving");

            //List<DataReferenceValue<int>> allVariants = await _coreDataRepo.GetAllCardVariantTypes();

            var mappedInventoryCards = parseCardsBackups
                .Join(_cardDataRepo.QueryCardDefinitions(),
                    backup => new { CollectorNumber = backup.CollectorNumber, SetCode = backup.SetCode},
                    card => new { CollectorNumber = card.CollectorNumber, SetCode = card.Set.Code},
                    (backup, card) => new 
                    {
                        Backup = backup,
                        Card = card,
                    })
                .Select(x => new InventoryCardData
            {
                InventoryCardStatusId = x.Backup.InventoryCardStatusId,
                IsFoil = x.Backup.IsFoil,
                //CardId = x.CardId,
                CardId = x.Card.CardId,
                //MultiverseId = x.MultiverseId,
                //VariantTypeId = allVariants.FirstOrDefault(v => v.Name == x.VariantName).Id,
                //DeckCards =
                //    x.Backup.DeckCards.Select(d => new DeckCardData()
                //    {
                //        DeckId = d.DeckId,
                //        CategoryId = d.Category,
                //    }).ToList(),
            }).ToList();

            //await _inventoryDataRepo.AddInventoryCardBatch(mappedInventoryCards);
            await _inventoryRepo.AddInventoryCardBatch(mappedInventoryCards);

            //_logger.LogWarning("RestoreDb - LoadCardBackups...COMPLETE!");
        }


        /// <summary>
        /// Parses an ImportListRecrd object from a string
        /// </summary>
        /// <param name="recordString">String to parse, assuming Arena style record</param>
        /// <returns>The mapped ImportLiistRecord object</returns>
        private static ImportListRecord ParseImportListRecord(string recordString)
        {
            //TODO - re-think this, allowing for a different way of detecting foils (and variants) other than #_FOIL




            var importLineTokens = recordString.Split(' ').ToList();

            if (importLineTokens.Count() < 2)
            {
                throw new Exception("Expected at least 2 tokens in a line, bad data");
            }


            
            var mappedRecord = new ImportListRecord();

            //first token will be the count
            if (int.TryParse(importLineTokens[0], out int parsedCount))
            {
                mappedRecord.Count = parsedCount;
                importLineTokens.RemoveAt(0);
            }
            
            //Check if the last token is "FOIL"
            if(importLineTokens.Last().ToLower() == "foil")
            {
                mappedRecord.IsFoil = true;
                importLineTokens.RemoveAt(importLineTokens.Count() - 1);
            }


            //Is the next line a #, or a set code wrapped in parens?
            if(int.TryParse(importLineTokens.Last(), out int parsedCollectorNumber))
            {
                mappedRecord.Number = parsedCollectorNumber;
                importLineTokens.RemoveAt(importLineTokens.Count() - 1);
            }

            //next might be the set code
            var lastTokenChars = importLineTokens.Last().ToCharArray();
            if(lastTokenChars[0] == '(' && lastTokenChars[lastTokenChars.Length-1] == ')')
            {
                mappedRecord.Code = importLineTokens.Last().Substring(1, lastTokenChars.Length - 2);// In reality Substring should always take 3
                importLineTokens.RemoveAt(importLineTokens.Count() - 1);
            }

            //Everything else should be the name
            mappedRecord.Name = string.Join(' ', importLineTokens);




            //number will be also used to determine if a card is foil
            //var numberToken = importLineTokens[importLineTokens.Count() - 1];

            //if (int.TryParse(importLineTokens[importLineTokens.Count() - 1], out int parsedNumber))
            //if (int.TryParse(numberToken.Split('_')[0], out int parsedNumber))
            //    mappedRecord.Number = parsedNumber;

            //if (numberToken.Split('_').Length > 1)
            //    mappedRecord.IsFoil = true;

            //Only need the code, but it's wrapped in parens (123)
            //assuming always a 3-char code
            //mappedRecord.Code = importLineTokens[importLineTokens.Count() - 2].Substring(1, 3);

            //mappedRecord.Name = importLineTokens.Skip(1).Take(importLineTokens.Length - 3).Aggregate((i, j) => $"{i} {j}");

            return mappedRecord;
        }

        private int PullLandCount(LandType landType, List<ImportListRecord> cards)
        {
            var basicLands = cards.FirstOrDefault(x => x.Name == landType.Value);
            if (basicLands != null)
            {
                //var count = basicLands.Count;
                cards.Remove(basicLands);
                //return count;
                return basicLands.Count; //can I do this after it's removed from the parent list?
            }
            else
            {
                return 0;
            }
        }

    }
}
