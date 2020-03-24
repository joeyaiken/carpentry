
using Carpentry.Data.Implementations;
using Carpentry.Data.Interfaces;
using Carpentry.Data.Models;
//using Carpentry.Data.S9Data;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using CarpentryMigrationTool.Models;
using Newtonsoft.Json.Linq;
using Carpentry.Data.DataContext;
using System.Net.Http;
using Carpentry.Data.DataContextLegacy;
using Card = Carpentry.Data.DataContext.Card;
using InventoryCard = Carpentry.Data.DataContext.InventoryCard;
using InventoryCardStatus = Carpentry.Data.DataContext.InventoryCardStatus;
using Deck = Carpentry.Data.DataContext.Deck;
using DeckCard = Carpentry.Data.DataContext.DeckCard;
using Carpentry.Data.BackupTool.Models;

namespace Carpentry.Data.BackupTool
{
    public class DataBackupService
    {

        private readonly ILogger<DataBackupService> _logger;

        readonly SqliteDataContext _cardContext;
        //readonly DataBackupConfig _config;

        readonly string _configDeckBackupLocation;
        readonly string _configCardBackupLocation;
        readonly string _configPropsBackupLocation;

        public DataBackupService(
            ILoggerFactory loggerFactory,
            SqliteDataContext cardContext
            //DataBackupConfig config
            )
        {
            _logger = loggerFactory.CreateLogger<DataBackupService>();

            //_legacyContext = legacyContext;
            _cardContext = cardContext;
            //_scryContext = scryContext;
            //_client = client;


            //_config = config;
            string appRoot = "C:\\DotNet\\carpentry-refactor\\Carpentry.Data.BackupTool\\Backups\\";
            string deckBackupLocation = "DeckBackups.txt";
            string cardsBackupLocation = "CardBackups.txt";
            string propsBackupLocation = "PropsBackup.txt";

            _configDeckBackupLocation = $"{appRoot}{deckBackupLocation}";
            _configCardBackupLocation = $"{appRoot}{cardsBackupLocation}";
            _configPropsBackupLocation = $"{appRoot}{propsBackupLocation}";
        }

        private List<ScryfallMagicCard> MapScryfallDataToCards(List<JToken> cardSearchData)
        {
            try
            {
                
            
                _logger.LogWarning("Begin MapScryfallDataToCards");

                List<ScryfallMagicCard> updatedCards = new List<ScryfallMagicCard>();
                List<JToken> specialCards = new List<JToken>();

                //for each card
                cardSearchData.ForEach(card =>
                {
                    try
                    {
                    //does it have at least 1 MID?
                    int? parsedMID = (int?)card.SelectToken("multiverse_ids[0]");

                        if (parsedMID != null)
                        {
                            ScryfallMagicCard cardToAdd = new ScryfallMagicCard();
                            cardToAdd.RefreshFromToken(card);

                            updatedCards.Add(cardToAdd);



                        }
                        else
                        {
                            specialCards.Add(card);
                        }
                    }
                    catch (Exception e)
                    {
                        throw;
                    }
                });

                //a 'special card' is a unique variation of a named card already in the set
                specialCards.ForEach(specialCard =>
                {

                    try
                    {
                        string cardName = specialCard.Value<string>("name");
                        var cardToUpdate = updatedCards.Where(x => x.Name == cardName).FirstOrDefault(); //should this just be First()?
                        if(cardToUpdate != null)
                        {
                            //_logger.LogWarning($"Applying variant to {cardName}");

                            cardToUpdate.ApplyVariant(specialCard);
                        }
                        else
                        {
                            _logger.LogError($"Could not find matching card for special card: {cardName}");
                        }
                        
                    }
                    catch (Exception e)
                    {
                        throw;
                    }
                });

                _logger.LogWarning("Completed MapScryfallDataToCards");



                return updatedCards;

            }
            catch (Exception e)
            {
                throw;
            }
        }

        #region legacy methods

        public void SaveDb()
        {
            //query inventory cards
            var cardExports = _cardContext.InventoryCards.Select(x => new BackupInventoryCard
            {
                MultiverseId = x.MultiverseId,
                InventoryCardStatusId = x.InventoryCardStatusId,
                IsFoil = x.IsFoil,
                VariantName = x.VariantType.Name,
                DeckCards = x.DeckCards.Select(x => new BackupDeckCard
                {
                    DeckId = x.DeckId,
                    Category = 'm',
                }).ToList(),
            }).OrderBy(x => x.MultiverseId).ToList();

            //query decks
            var deckExports = _cardContext.Decks.Select(x => new BackupDeck
            {
                    ExportId = x.Id,
                    Name = x.Name,
                    Format = x.Format.Name,
                    Notes = x.Notes,
                    BasicW = x.BasicW,
                    BasicU = x.BasicU,
                    BasicB = x.BasicB,
                    BasicR = x.BasicR,
                    BasicG = x.BasicG
            }).OrderBy(x => x.ExportId).ToList();

            //query set codes
            var setCodes = _cardContext.Sets.Select(x => x.Code).OrderBy(x => x).ToList();

            BackupDataProps backupProps = new BackupDataProps()
            {
                SetCodes = setCodes,
                TimeStamp = DateTime.Now,
            };

            var deckBackupObj = JArray.FromObject(deckExports);
            var cardBackupObj = JArray.FromObject(cardExports);
            var propsBackupObj = JObject.FromObject(backupProps);

            System.IO.File.WriteAllText(_configDeckBackupLocation, deckBackupObj.ToString());
            System.IO.File.WriteAllText(_configCardBackupLocation, cardBackupObj.ToString());
            System.IO.File.WriteAllText(_configPropsBackupLocation, propsBackupObj.ToString());


            //System.IO.File.WriteAllText(@"C:\DotNet\Carpentry\CarpentryMigrationTool\Backups\InventoryBackup.txt", invObj.ToString());

            //System.IO.File.WriteAllText(@"C:\DotNet\Carpentry\CarpentryMigrationTool\Backups\DeckBackups.txt", deckObj.ToString());

        }

        private async Task ReloadDbFromCards()
        {
            throw new NotImplementedException();
            //    try
            //    {
            //        await ClearDb();

            //        string savedCards = System.IO.File.ReadAllText(@"C:\DotNet\Carpentry\Carpentry\CardBackups.txt");

            //        var parsedCardsObj = JObject.Parse(savedCards);

            //        List<Data.Card> parsedCards = parsedCardsObj["cards"].ToObject<List<Data.Card>>();

            //        List<string> storedSetCodes = new List<string>();

            //        //fuck it
            //        //loading just known sets
            //        List<string> setsToLoad = new List<string>()
            //        {
            //            "mh1",
            //            "m20",//,
            //            "war"
            //        };

            //        //need to make sure we don't throw too much at the API
            //        for (int i = 0; i < setsToLoad.Count(); i++)
            //        {
            //            await EnsureSetExistsLocally(setsToLoad[i]);
            //            await Task.Delay(1000);
            //        }


            //        //for each card
            //        parsedCards.ForEach(card =>
            //        {
            //            //I need to check if I should load the set

            //            //if(!storedSetCodes.Contains(card.))
            //            var thisDBCard = _context.CardDetails.FirstOrDefault(x => x.MultiverseId == card.MultiverseId);
            //            card.Data = thisDBCard;
            //            _context.Cards.Add(card);
            //        });


            //        await _context.SaveChangesAsync();
            //    }
            //    catch (Exception ex)
            //    {
            //        throw ex;
            //    }

        }

        private async Task LoadDb()
        {
            throw new NotImplementedException();
            //    try
            //    {
            //        await ReloadDbFromCards();
            //        return;


            //        string savedText = System.IO.File.ReadAllText(@"C:\DotNet\Carpentry\Carpentry\InventoryDbBackup.txt");

            //        var parsedStore = JObject.Parse(savedText);

            //        var parsedCards = parsedStore["cards"].ToObject<List<Data.Card>>();
            //        var parsedCardData = parsedStore["data"].ToObject<List<Data.CardDetail>>();
            //        //var parsedSets = parsedStore["sets"].ToObject<List<S4SetDetail>>();

            //        //Data.SetDetail mh1Set = new Data.SetDetail()
            //        //{
            //        //    Code = "MH1",
            //        //    StringData = "{}"
            //        //};



            //        parsedCards.ForEach((card) =>
            //        {
            //            card.Data = parsedCardData.FirstOrDefault(x => x.MultiverseId == card.MultiverseId);
            //        });

            //        List<SetDetail> newSetList = new List<SetDetail>();

            //        parsedCardData.ForEach(detail =>
            //        {
            //            //detail.SetDetail = mh1Set;
            //            //this will be a pain...

            //            //parse this card data (card data doesn't hold sets yet)
            //            //MagicCardDto thisCard = new MagicCardDto(detail.StringData);
            //            MagicCardDto thisCard = JObject.Parse(detail.StringData).ToObject<MagicCardDto>();

            //            detail.SetCode = thisCard.Set;

            //            var setInList = newSetList.FirstOrDefault(x => x.Code == thisCard.Set);
            //            if (setInList == null)
            //            {
            //                setInList = new SetDetail()
            //                {
            //                    Code = thisCard.Set,
            //                    StringData = null,
            //                    LastUpdated = null
            //                };

            //                newSetList.Add(setInList);
            //            }
            //            //else
            //            //{

            //            //}
            //            detail.SetDetail = setInList;
            //            //setInList.Cards.Add(detail);

            //            //  actually not the biggest pain
            //        });

            //        await ClearDb();


            //        //await _context.Sets.AddAsync(mh1Set);
            //        await _context.Sets.AddRangeAsync(newSetList);

            //        await _context.CardDetails.AddRangeAsync(parsedCardData);
            //        await _context.Cards.AddRangeAsync(parsedCards);

            //        await _context.SaveChangesAsync();
            //    }
            //    catch (Exception ex)
            //    {
            //        throw;
            //    }
        }

        #endregion
    }
}
