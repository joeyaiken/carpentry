using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Carpentry.Data.Interfaces;
using Carpentry.Data.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Carpentry;

namespace CarpentryMigrationTool
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

    

    class ImportService
    {
        private readonly ILogger<ImportService> _logger;
        private readonly ICardRepo _cardRepo;
        private readonly ICardStringRepo _scryRepo;

        public ImportService(ILogger<ImportService> logger, ICardRepo cardRepo, ICardStringRepo scryRepo)
        {
            _logger = logger;
            _cardRepo = cardRepo;
            _scryRepo = scryRepo;

        }

        //I need: A method of importing prebuilt decks into the carpentry DB
        public async Task ImportDeckLists()
        {
            throw new Exception("This  has already been imported");
            //I don't know where this should be configured, but for now it can be static
            _logger.LogInformation("Beginning deck import...");

            //Populate EDH deck:

            //string populateEDHFilepath = "C:\\DotNet\\carpentry-refactor\\CarpentryMigrationTool\\Imports\\Populate.txt";
            //await ImportNewDeck("Primal Genesis","commander", populateEDHFilepath);

            //Knights brawl deck
            //string knightsBrawlFilepath = "C:\\DotNet\\carpentry-refactor\\CarpentryMigrationTool\\Imports\\Knight.txt";
            //await ImportNewDeck("Knight's Charge", "brawl", knightsBrawlFilepath);

            ////Chulane brawl deck
            //string chulaneBrawlFilepath = "C:\\DotNet\\carpentry-refactor\\CarpentryMigrationTool\\Imports\\Chulane.txt";
            //await ImportNewDeck("Wild Bounty", "brawl", chulaneBrawlFilepath);

            //Draggo brawl deck
            string draggoBrawlFilepath = "C:\\DotNet\\carpentry-refactor\\CarpentryMigrationTool\\Imports\\Draggo.txt";
            await ImportNewDeck("Savage Hunter 2", "brawl", draggoBrawlFilepath);

        }

        //gets the count of a card name, then removes that card from the list
        private int PullLandCount(string landType, List<ImportListRecord> cards)
        {
            var basicLands = cards.FirstOrDefault(x => x.Name == landType);
            if(basicLands != null)
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

        private async Task ImportNewDeck(string deckName, string deckFormat, string cardListFilepath)
        {
            _logger.LogInformation($"ImportNewDeck - Attempting to import deck {deckName}");
            List<ImportListRecord> mappedRecords = LoadCardList(cardListFilepath);

            DeckProperties newDeck = new DeckProperties()
            {
                Name = deckName,
                Format = deckFormat,
            };

            newDeck.BasicW = PullLandCount("Plains", mappedRecords);
            newDeck.BasicU = PullLandCount("Island", mappedRecords);
            newDeck.BasicB = PullLandCount("Swamp", mappedRecords);
            newDeck.BasicR = PullLandCount("Mountain", mappedRecords);
            newDeck.BasicG = PullLandCount("Forest", mappedRecords);

            //At this point I should save the new deck, and get the ID!!!!
            var newDeckId = await _cardRepo.AddDeck(newDeck);

            //I'll potentially need to create multiple deck cards for single item
            List<DeckCardDto> newDeckCards = new List<DeckCardDto>();
            

            //I have concerns over this...
            //How can I be sure it's finished the ForEach 
            //Maybe this should just be a traditional for loop (it probably has to be, so we don't hammer scryfall)

            for(int i = 0; i < mappedRecords.Count(); i++)
            {
                var record = mappedRecords[i];

                int recordMultiverseId = await _scryRepo.GetCardMultiverseId(record.Name, record.Code);
                
                await Task.Delay(100);
                
                for (int c = 0; c < record.Count; c++)
                {
                    newDeckCards.Add(new DeckCardDto
                    {
                        DeckId = newDeckId,
                        InventoryCard = new InventoryCardDto
                        {
                            InventoryCardStatusId = 1, //1 == in inventory
                            IsFoil = record.IsFoil,
                            MultiverseId = recordMultiverseId,
                            VariantType = "normal"
                        }
                    });
                }
            }

            //await _cardRepo.AddDeckCardBatch(newDeckCards);

            _logger.LogInformation($"ImportNewDeck - Successfully imported deck {deckName}");
        }

        private List<ImportListRecord> LoadCardList(string filepath)
        {
            //not doing any verification that the file actually exists
            string[] fileContents = System.IO.File.ReadAllLines(filepath);
            var mappedRecords = fileContents.Select(line => ParseImportListRecord(line)).ToList();
            return mappedRecords;
        }

        /// <summary>
        /// Parses an ImportListRecrd object from a string
        /// </summary>
        /// <param name="recordString">String to parse, assuming Arena style record</param>
        /// <returns>The mapped ImportLiistRecord object</returns>
        private static ImportListRecord ParseImportListRecord(string recordString)
        {
            var importLineTokens = recordString.Split(' ');

            if (importLineTokens.Count() < 4)
            {
                throw new Exception("Expected at least 4 tokens in a line, bad data");
            }

            var mappedRecord = new ImportListRecord();

            if (int.TryParse(importLineTokens[0], out int parsedCount))
                mappedRecord.Count = parsedCount;

            //number will be also used to determine if a card is foil
            var numberToken = importLineTokens[importLineTokens.Count() - 1];

            //if (int.TryParse(importLineTokens[importLineTokens.Count() - 1], out int parsedNumber))
            if (int.TryParse(numberToken.Split('_')[0], out int parsedNumber))
                mappedRecord.Number = parsedNumber;

            if (numberToken.Split('_').Length > 1)
                mappedRecord.IsFoil = true;

            //Only need the code, but it's wrapped in parens (123)
            //assuming always a 3-char code
            mappedRecord.Code = importLineTokens[importLineTokens.Count() - 2].Substring(1, 3);

            mappedRecord.Name = importLineTokens.Skip(1).Take(importLineTokens.Length - 3).Aggregate((i, j) => $"{i} {j}");

            return mappedRecord;
        }

    }
}
