using Carpentry.Data.DataContext;
using Carpentry.Data.Interfaces;
using Carpentry.Data.DataModels;
using Carpentry.Logic.Interfaces;
using Carpentry.Logic.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Carpentry.Data.Models;

namespace Carpentry.Logic.Implementations
{
    //TODO - consider renaming this to "DataMaintenanceService" or "DataIntegrityService"
    //Idea being this checks if sets / cards exist
    public class DataUpdateService : IDataUpdateService
    {
        private readonly ILogger<DataBackupService> _logger;

        private readonly IScryfallService _scryService;

        private readonly ICardDataRepo _cardRepo;

        private readonly IScryfallDataRepo _scryfallRepo;

        private readonly int _dbRefreshIntervalDays;

        public DataUpdateService(
            ILogger<DataBackupService> logger,
            IScryfallService scryService,
            ICardDataRepo cardRepo,
            IScryfallDataRepo scryfallRepo
            )
        {
            _logger = logger;
            _scryService = scryService;
            _scryfallRepo = scryfallRepo;
            _cardRepo = cardRepo;
            _dbRefreshIntervalDays = 0;
        }

        public async Task EnsureCardDefinitionExists(int multiverseId)
        {
            //var dbCard = await _cardRepo.QueryCardDefinitions().FirstOrDefaultAsync(x => x.Id == multiverseId);
            var dbCard = await _cardRepo.GetCardById(multiverseId);

            if (dbCard != null)
            {
                return;
            }

            var scryfallCard = await _scryService.GetCardByMid(multiverseId);

            await UpdateSetData(scryfallCard.Set);

            //await _cardRepo.AddCardDefinition(scryfallCard);
        }

        /// <summary>
        /// Updates all pricing and legality data for a card set
        /// </summary>
        /// <returns></returns>
        public async Task UpdateAllSets()
        {
            _logger.LogInformation("UpdateAllSets - Calculating sets to update");

            var setCodes = await _cardRepo.GetAllCardSetCodes();

            _logger.LogInformation($"Found {setCodes.Count()} sets in the database, checking last updated");

            for (int i = 0; i < setCodes.Count(); i++)
            {

                DateTime? dbLastUpdated = await _cardRepo.GetCardSetLastUpdated(setCodes[i]);

                if (dbLastUpdated != null && dbLastUpdated.Value.AddDays(_dbRefreshIntervalDays) > DateTime.Today.Date)
                {
                    _logger.LogInformation($"Set code {setCodes[i]} was last updated {dbLastUpdated.ToString()}, nothing will be updated.");
                    return;
                } 
                else
                {
                    string replaceReason = dbLastUpdated == null ? "has never been updated" : $"was last updated {dbLastUpdated.ToString()}";
                    _logger.LogInformation($"Set code {replaceReason}.  Set will now be updated");
                    await UpdateSetData(setCodes[i]);
                }
            }

            _logger.LogWarning("UpdateAllSets - Done refreshing set data");

        }

        /// <summary>
        /// Update the card and scry data for a particular set
        /// </summary>
        /// <param name="setCode"></param>
        /// <returns></returns>
        public async Task UpdateSetData(string setCode)
        {
            //steps to update a set:

            //check if the set is actually up to date, return if it is
            DateTime? dbLastUpdated = await _cardRepo.GetCardSetLastUpdated(setCode);
            if (dbLastUpdated != null && dbLastUpdated.Value.AddDays(_dbRefreshIntervalDays) > DateTime.Today.Date)
            {
                _logger.LogInformation($"UpdateSetData - Card set {setCode} is already up to date, nothing will be updated.");
                return;
            }

            //check if the scry set is up to date
            DateTime? scryDataLastUpdated = await _scryfallRepo.GetSetDataLastUpdated(setCode);

            ScryfallSetData scryData = null;

            //If not, get from SF
            if (scryDataLastUpdated == null || scryDataLastUpdated.Value.AddDays(_dbRefreshIntervalDays) < DateTime.Today.Date)
            {
                var scryfallPayload = await _scryService.GetFullSet(setCode);

                scryData = new ScryfallSetData
                {
                    Code = scryfallPayload.Code,
                    Name = scryfallPayload.Name,
                    DataIsParsed = false,
                    LastUpdated = DateTime.Now,
                    ReleasedAt = DateTime.Parse(scryfallPayload.ReleaseDate),
                    CardData = JsonConvert.SerializeObject(scryfallPayload.CardTokens)
                };

                await _scryfallRepo.AddOrUpdateSet(scryData);
            }
            else
            {
                scryData = await _scryfallRepo.GetSetByCode(setCode);
            }

            List<MagicCard> magicCards = null;

            //check if the scry set is parsed
            if (scryData.DataIsParsed)
            {
                //deserialize as MagicCard
                magicCards = JsonConvert.DeserializeObject<List<MagicCard>>(scryData.CardData);
            }
            else
            {
                //deserialize as JToken list
                List<JToken> unparsedCards = JsonConvert.DeserializeObject<List<JToken>>(scryData.CardData);

                //map
                magicCards = _scryService.MapScryfallDataToCards(unparsedCards).Select(x => x.ToMagicCard()).ToList();

                //Serialize & apply to set
                scryData.CardData = JsonConvert.SerializeObject(magicCards);
                await _scryfallRepo.AddOrUpdateSet(scryData);
            }

            //Ensure the set definition exists / get the set ID
            CardSetData cardSet = new CardSetData()
            {
                Code = scryData.Code,
                Name = scryData.Name,
                LastUpdated = null,
                ReleaseDate = scryData.ReleasedAt,
            };
            cardSet.Id = await _cardRepo.AddOrUpdateCardSet(cardSet);

            //AddOrUpdate all card definitions in card DB
            var cardTasks = magicCards.Select(card =>
            {
                //_cardRepo.add
                CardDataDto cardDefinition = new CardDataDto()
                {
                    Cmc = card.Cmc,
                    ColorIdentity = card.ColorIdentity,
                    Colors = card.Colors,
                    Legalities = card.Legalities,
                    ManaCost = card.ManaCost,
                    MultiverseId = card.MultiverseId,
                    Name = card.Name,
                    Rarity = card.Rarity,
                    Set = card.Set,
                    Text = card.Text,
                    Type = card.Type,
                    Variants = card.Variants.Keys.Select(x => new CardVariantDto
                    {
                        Name = x,
                        Image = card.Variants[x],
                        Price = card.Prices[x],
                        PriceFoil = card.Prices[$"{x}_foil"],
                    }).ToList(),
                };

                return _cardRepo.AddOrUpdateCardDefinition(cardDefinition);
            }).ToList();

            await Task.WhenAll(cardTasks);

            //Update the LastUpdated timestamp after cards have been successfully updated
            cardSet.LastUpdated = DateTime.Now;
            await _cardRepo.AddOrUpdateCardSet(cardSet);
            
            //////////////////////

            //_logger.LogError($"Refresh Set Data - Beginning process of refreshing {setCode}");

            ////update any card definitions for the set
            //var cardsToUpdate = await _cardContext.Cards.Where(x => x.SetId == setDefinition.Id).ToListAsync();

            //if (cardsToUpdate.Any())
            //{
            //    //update all cards
            //    var updateTasks = cardsToUpdate.Select(card =>
            //    {
            //        var thisCardFromScryfall = setFromScryfall.Cards.Where(c => c.MultiverseId == card.Id).FirstOrDefault();
            //        return _cardRepo.UpdateCardDefinition(thisCardFromScryfall);
            //    });

            //    await Task.WhenAll(updateTasks);
            //}


            ////update the scry set record
            //existingScryfallSet.LastUpdated = DateTime.Now;

            //await _scryRepo.UpdateScrySet(existingScryfallSet);

            //_logger.LogError($"Refresh Set Data - Completed process of refreshing {setCode}");
        }

    }
}
