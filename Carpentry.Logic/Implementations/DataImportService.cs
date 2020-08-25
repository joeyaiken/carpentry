using Carpentry.Data.DataModels;
using Carpentry.Data.Interfaces;
using Carpentry.Data.QueryResults;
using Carpentry.Logic.Interfaces;
using Carpentry.Logic.Models;
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

    public class DataImportService : IDataImportService
    {
        private readonly IDataUpdateService _dataUpdateService;
        private readonly ICardDataRepo _cardDataRepo;
        private readonly IDeckService _deckService;
        private readonly IInventoryService _inventoryService;
        private readonly int _cardStatus_InInventory = 1;
        public DataImportService(
            IDataUpdateService dataUpdateService,
            ICardDataRepo cardDataRepo,
            IDeckService deckService,
            IInventoryService inventoryService
            )
        {
            _dataUpdateService = dataUpdateService;
            _cardDataRepo = cardDataRepo;
            _deckService = deckService;
            _inventoryService = inventoryService;
        }

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
                    result.UntrackedSets.Add(new ValidatedDtoUntrackedSet() { SetId = set.Id, SetCode = set.Code });
                }
                //await _dataUpdateService.AddTrackedSet(set.Id);
            }

            //List<ValidatedCardDto> validatedCards = new List<ValidatedCardDto>();

            //for each card, get the matching DB card by Name+Code (from the carpentry DB)
            foreach (var card in mappedRecords)
            {
                var matchingCard = await _cardDataRepo.GetCardData(card.Name, card.Code);

                ValidatedCardDto newCard = new ValidatedCardDto()
                {
                    CardId = matchingCard.Id,
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
                IsFoil = c.IsFoil,
                CardId = c.CardId,
                InventoryCardStatusId = _cardStatus_InInventory,
                CategoryId = null,
                InventoryCardId = 0, //new inventory card
                Id = 0, //new card
            }).ToList();

            await _deckService.AddDeckCardBatch(cardsToAdd);
        }

        public async Task<ValidatedCarpentryImportDto> ValidateCarpentryImport(CardImportDto payload)
        {
            //TODO - Is there a better way to submit a file than reading from a directory?

            var result = new ValidatedCarpentryImportDto()
            {
                BackupDirectory = payload.ImportPayload,
            };

            //ignoring the payload type for now

            //verify the directory exists

            //Extract the Backup Props file

            //see what sets the Backup Props are still untracked

            //






            throw new NotImplementedException();
        }

        public async Task AddValidatedCarpentryImport(ValidatedCarpentryImportDto payload)
        {
            throw new NotImplementedException();
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
