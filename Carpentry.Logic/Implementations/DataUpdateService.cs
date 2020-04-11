using Carpentry.Logic.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Carpentry.Logic.Implementations
{
    public class DataUpdateService //: IDataUpdateService
    {
        private readonly ILogger<DataBackupService> _logger;

        //private readonly SqliteDataContext _cardContext;
        ////private readonly ScryfallDataContext _scryContext;

        //private readonly MigrationToolConfig _config;

        //private readonly ScryfallRepo _scryRepo;

        //private readonly SqliteCardRepo _cardRepo;

        //private readonly CarpentryService _carpentryService;

        private readonly IScryfallService _scryService;

        public DataUpdateService(
            ILogger<DataBackupService> logger//,
            //SqliteDataContext cardContext,
            //MigrationToolConfig config,
            //ScryfallRepo scryRepo,
            //SqliteCardRepo cardRepo
            )
        {
            _logger = logger;
            //_cardContext = cardContext;
            //_config = config;
            //_scryRepo = scryRepo;
            //_cardRepo = cardRepo;
        }

        public async Task UpdateAllSets()
        {

            //Updates all pricing and legality data
            //When a scry set is updated, all data cards in that set should also be updated

            _logger.LogWarning("RefreshDB - Calculating sets to update");

            var setCodesToRefresh = await _scryRepo.QueryScrySets()
                .Where(set => set.LastUpdated == null || set.LastUpdated.Value.AddDays(_config.DataRefreshIntervalDays) < DateTime.Today.Date)
                .Select(x => x.Code).ToListAsync();

            _logger.LogWarning($"Found {setCodesToRefresh.Count()} sets to refresh");

            for (int i = 0; i < setCodesToRefresh.Count(); i++)
            {
                await RefreshSetData(setCodesToRefresh[i]);
            }

            _logger.LogWarning("RefreshDB - Done refreshing set data");


        }

        //Update the card and scry data for a particular set
        public async Task UpdateSetData(string setCode)
        {
            //steps to update a set:

            //check if the set is actually up to date, return if it is

            //I guess add new if the set doesn't actually exist yet

            //check if the scry set is up to date

            //check if the scry set is parsed

            //Re-parse scry data, addOrUpdate all card definitions













            _logger.LogError($"Refresh Set Data - Beginning process of refreshing {setCode}");


         
            //var existingScryfallSet = _scryContext.Sets.Where(x => x.Code == setCode).FirstOrDefault();
            var existingScryfallSet = await _scryRepo.GetScrySetByCode(setCode);

            //get a full set from scryfall
            var setFromScryfall = await _scryService.GetFullSet(setCode);

            //update all scryfall card data for that set
            //var scryDbSet = _scryContext.Sets.FirstOrDefault(x => x.Code == setCode);

            var tasks = setFromScryfall.Cards.Select(async card =>
            {
                var storedCard = await _scryRepo.GetScryCardById(card.MultiverseId);

                if (storedCard != null)
                {
                    storedCard.StringData = card.Serialize();
                    return _scryRepo.UpdateScryCard(storedCard);
                }
                else
                {
                    var cardToAdd = new ScryfallCard()
                    {
                        MultiverseId = card.MultiverseId,
                        StringData = card.Serialize(),
                        Set = existingScryfallSet
                    };
                    return _scryRepo.AddScryCard(cardToAdd);
                }


                //return _scryRepo.UpdateScryCard()
            });

            await Task.WhenAll(tasks);


            //setFromScryfall.Cards.ForEach(card =>
            //{


            //    var storedCard = _scryContext.Cards.FirstOrDefault(x => x.MultiverseId == card.MultiverseId);
            //    if (storedCard != null)
            //    {
            //        storedCard.StringData = card.Serialize();
            //        //_scryContext.Update(storedCard);
            //        _scryRepo.UpdateScryCard(storedCard);
            //    }
            //    else
            //    {
            //        var cardToAdd = new ScryfallCard()
            //        {
            //            MultiverseId = card.MultiverseId,
            //            StringData = card.Serialize(),
            //            Set = existingScryfallSet
            //        };
            //        _scryRepo.AddScryCard(cardToAdd);
            //        _scryContext.Cards.Add(cardToAdd);
            //    }
            //});


            //update the set definition (name/release date)
            var setDefinition = _cardContext.Sets.FirstOrDefault(x => x.Code == setCode);

            setDefinition.Name = setFromScryfall.Name;
            setDefinition.ReleaseDate = DateTime.Parse(setFromScryfall.ReleaseDate);
            _cardContext.Sets.Update(setDefinition);
            await _cardContext.SaveChangesAsync();

            //update any card definitions for the set
            var cardsToUpdate = await _cardContext.Cards.Where(x => x.SetId == setDefinition.Id).ToListAsync();

            if (cardsToUpdate.Any())
            {
                //update all cards
                var updateTasks = cardsToUpdate.Select(card =>
                {
                    var thisCardFromScryfall = setFromScryfall.Cards.Where(c => c.MultiverseId == card.Id).FirstOrDefault();
                    return _cardRepo.UpdateCardDefinition(thisCardFromScryfall);
                });

                await Task.WhenAll(updateTasks);
            }


            //update the scry set record
            existingScryfallSet.LastUpdated = DateTime.Now;

            await _scryRepo.UpdateScrySet(existingScryfallSet);

            _logger.LogError($"Refresh Set Data - Completed process of refreshing {setCode}");
        }

    }
}
