using Carpentry.Logic.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Carpentry.CarpentryData.Models;
using Carpentry.CarpentryData;
using Microsoft.EntityFrameworkCore;
using Carpentry.Logic.Models.Scryfall;

namespace Carpentry.Logic
{
    public interface IDataUpdateService
    {
        Task ValidateDatabase();

        Task<List<SetDetailDto>> GetTrackedSets(bool showUntracked, bool update);
        Task AddTrackedSet(int setId);
        Task RemoveTrackedSet(int setId);
        Task UpdateTrackedSet(int setId);

        Task TryUpdateAvailableSets();
    }

    //TODO - consider renaming this to "DataMaintenanceService" or "DataIntegrityService"
    //Idea being this checks if sets / cards exist, and can check if DB defaults
    // Maybe just CardDataService?
    //      Would that be a good spot to also get filter options?
    //
    public class DataUpdateService : IDataUpdateService
    {
        private readonly ILogger<DataUpdateService> _logger;
        private readonly IScryfallService _scryService;
        private readonly IDataIntegrityService _dataIntegrityService;
        private readonly CarpentryDataContext _cardContext;
        private readonly int _dbRefreshIntervalDays;


        public DataUpdateService(
            ILogger<DataUpdateService> logger,
            IScryfallService scryService,
            IDataIntegrityService dataIntegrityService,
            CarpentryDataContext cardContext
            )
        {
            _logger = logger;
            _scryService = scryService;
            _dataIntegrityService = dataIntegrityService;
            _dbRefreshIntervalDays = 0; //TODO - move to a config
            _cardContext = cardContext;
        }

        #region Private Methods

        private static char GetRarityId(string rarityName)
        {
            char rarity;
            switch (rarityName)
            {
                case "mythic":
                    rarity = 'M';
                    break;

                case "rare":
                    rarity = 'R';
                    break;
                case "uncommon":
                    rarity = 'U';
                    break;
                case "common":
                    rarity = 'C';
                    break;
                //I guess 'special' is required for Time Spiral timeshifted cards 
                case "special":
                    rarity = 'S';
                    break;
                default:
                    throw new Exception("Error reading scryfall rarity");

            }
            return rarity;
        }
        
        private static CardData MapScryCardToLocal(ScryfallMagicCard scryCard, int setId, List<MagicFormatData> allFormats)
        {
            var result = new CardData
            {
                MultiverseId = scryCard.MultiverseId,
                Cmc = scryCard.Cmc,
                ManaCost = scryCard.ManaCost,
                Name = scryCard.Name,
                Text = scryCard.Text,
                Type = scryCard.Type,
                SetId = setId,
                RarityId = GetRarityId(scryCard.Rarity),
                CollectorNumber = scryCard.CollectionNumber,
                ImageUrl = scryCard.ImageUrl,
                Price = (double?)scryCard.Price,
                PriceFoil = (double?)scryCard.PriceFoil,
                TixPrice = (double?)scryCard.PriceTix,
                Color = scryCard.Colors == null ? null : string.Join("", scryCard.Colors),
                ColorIdentity = string.Join("", scryCard.ColorIdentity),
                CollectorNumberStr = scryCard.CollectionNumberStr,
                CollectorNumberSuffix = scryCard.CollectionNumberSuffix,

                //This may seem weird, but I'm not tracking all the formats listed in scryfall
                Legalities = allFormats
                    .Where(format => scryCard.Legalities.Contains(format.Name))
                    .Select(format => new CardLegalityData
                    {
                        FormatId = format.FormatId,
                    }).ToList(),
            };
            return result;
        }

        #endregion

        #region Public Methods

        public async Task ValidateDatabase()
        {
            await _dataIntegrityService.EnsureDatabasesCreated();
        }

        /// <summary>
        /// Gets a list of set totals, filtering if requested
        /// </summary>
        /// <param name="showUntracked"></param>
        /// <param name="update"></param>
        /// <returns></returns>
        public async Task<List<SetDetailDto>> GetTrackedSets(bool showUntracked, bool update)
        {
            if (update) await TryUpdateAvailableSets();

            var result = await _cardContext.GetSetTotals()
                .Where(s => showUntracked || s.IsTracked == true)
                .Select(s => new SetDetailDto()
                {
                    SetId = s.SetId,
                    Code = s.Code,
                    DataLastUpdated = s.LastUpdated,
                    Name = s.Name,
                    CollectedCount = s.CollectedCount ?? 0,
                    InventoryCount = s.InventoryCount ?? 0,
                    IsTracked = s.IsTracked,
                    TotalCount = s.TotalCount,
                    ReleaseDate = s.ReleaseDate,
                })
                .OrderByDescending(s => s.ReleaseDate)
                .ToListAsync();

            return result;
        }

        public async Task AddTrackedSet(int setId)
        {
            var dbSet = await _cardContext.Sets
                .Include(s => s.Cards)
                .FirstOrDefaultAsync(s => s.SetId == setId);

            if (dbSet == null) throw new ArgumentException($"No set matching provided set ID of {setId}");
            
            if (dbSet.IsTracked) return;

            if (dbSet.Cards.Any()) throw new ArgumentException($"Set {dbSet.Name} is supposed to be untracked but already contains cards");

            var currentScryData = await _scryService.GetSetDetail(dbSet.Code);

            List<MagicFormatData> allFormats = _cardContext.MagicFormats.ToList();

            var newCards = currentScryData.Cards
                .Select(dto => MapScryCardToLocal(dto, setId, allFormats));

            _cardContext.Cards.AddRange(newCards);

            dbSet.IsTracked = true;
            dbSet.LastUpdated = DateTime.Now;

            _cardContext.Sets.Update(dbSet);

            await _cardContext.SaveChangesAsync();
        }

        /// <summary>
        /// Updates the card definitions for a tracked set
        /// Will throw an error if the SetId isn't valid
        /// Will not update anything if the set isn't tracked
        /// </summary>
        /// <param name="setId"></param>
        /// <returns></returns>
        public async Task UpdateTrackedSet(int setId)
        {
            var dbSet = await _cardContext.Sets
                .FirstOrDefaultAsync(s => s.SetId == setId);

            if (dbSet == null) throw new ArgumentException($"No set matching provided set ID of {setId}");
            
            if (!dbSet.IsTracked) return; //Don't want to update untracked sets
            
            //Only update a set if it's "stale enough"
            if (dbSet.LastUpdated != null && dbSet.LastUpdated.Value.AddDays(_dbRefreshIntervalDays) > DateTime.Today.Date)
            {
                _logger.LogInformation($"UpdateSetData - Card set {dbSet.Code} is already up to date, nothing will be updated.");
                return;
            }

            //Get the updated card definitions
            var scryData = await _scryService.GetSetDetail(dbSet.Code);

            var setCards = await _cardContext.Cards
                .Include(c => c.Legalities).ThenInclude(l => l.Format)
                .Where(c => c.SetId == setId)
                .ToDictionaryAsync(c => c.CollectorNumberStr, c => c);

            var allFormats = await _cardContext.MagicFormats.ToListAsync();
            var formatsDict = allFormats.ToDictionary(f => f.Name, f => f);

            var unmatchedCards = new List<ScryfallMagicCard>();

            foreach(var scryCard in scryData.Cards)
            {

                if (!setCards.TryGetValue(scryCard.CollectionNumberStr, out CardData existingCard))
                {
                    unmatchedCards.Add(scryCard);
                    continue;
                }

                existingCard.Price = (double?)scryCard.Price;
                existingCard.PriceFoil = (double?)scryCard.PriceFoil;
                existingCard.TixPrice = (double?)scryCard.PriceTix;

                var cardLegalitiesToDelete = existingCard.Legalities.Where(cl => !scryCard.Legalities.Contains(cl.Format.Name));

                var legalityStringsToKeep = existingCard.Legalities
                    .Where(cl => scryCard.Legalities.Contains(cl.Format.Name))
                    .Select(cl => cl.Format.Name);

                var legalitiesToAdd = scryCard.Legalities
                    .Where(sc => !legalityStringsToKeep.Contains(sc))
                    .Select(l => new CardLegalityData()
                    {
                        CardId = existingCard.CardId,
                        Format = formatsDict[l]
                    })
                    .Where(l => l.Format != null);

                if (cardLegalitiesToDelete.Any())
                    _cardContext.CardLegalities.RemoveRange(cardLegalitiesToDelete);
                if (legalitiesToAdd.Any())
                    _cardContext.CardLegalities.AddRange(legalitiesToAdd);

                _cardContext.Update(existingCard);
            }

            if (unmatchedCards.Any())
            {
                var newCards = unmatchedCards.Select(card => MapScryCardToLocal(card, setId, allFormats));
                _cardContext.Cards.AddRange(newCards);
            }

            //Finally, update the Set Definition so we know it's up to date
            dbSet.LastUpdated = DateTime.Now;
            _cardContext.Sets.Update(dbSet);

            await _cardContext.SaveChangesAsync();
        }

        public async Task RemoveTrackedSet(int setId)
        {
            var dbSet = await _cardContext.Sets.FirstOrDefaultAsync(s => s.SetId == setId);
            var setTotals = await _cardContext.GetSetTotals().FirstOrDefaultAsync(s => s.SetId == setId);

            if (setTotals == null) throw new ArgumentException($"No set matching provided set ID of {setId}");
            
            if (!setTotals.IsTracked) return; //If not tracked, just return
            
            //verify 0 owned/collected (won't delete anything with owned cards)
            if (setTotals.InventoryCount > 0 || setTotals.CollectedCount > 0) return;
            
            var cardsToDelete = _cardContext.Cards.Where(c => c.SetId == setId).ToList();
            _cardContext.Cards.RemoveRange(cardsToDelete);

            dbSet.LastUpdated = null;
            dbSet.IsTracked = false;

            _cardContext.Sets.Update(dbSet);
            await _cardContext.SaveChangesAsync();
        }

        /// <summary>
        /// Update the list of sets available to track (does not update the data of cards in the set)
        /// Gets called by DataRestore service to populate possible sets
        /// </summary>
        /// <returns></returns>
        public async Task TryUpdateAvailableSets()
        {
            //get the set definition overviews from the scry DB
            var scrySets = await _scryService.GetSetOverviews();

            var existingCardSets = await _cardContext.Sets.ToListAsync();

            foreach (var scrySet in scrySets)
            {
                //for each result, see if it exists in the DB
                var existingSet = existingCardSets.Where(x => x.Code.ToLower() == scrySet.Code.ToLower()).FirstOrDefault();

                //if it doesn't, create a new one that can be added to the db
                if (existingSet == null)
                {
                    existingSet = new CardSetData()
                    {
                        Code = scrySet.Code,
                        IsTracked = false,
                        LastUpdated = null,
                    };
                }

                //This mainly makes sure these fields are updated in the DB
                existingSet.Name = scrySet.Name;
                existingSet.ReleaseDate = scrySet.ReleasedAt;

                _cardContext.Sets.Update(existingSet); //Update will still add new if is untracked
                await _cardContext.SaveChangesAsync();
            }
        }

        #endregion

    }
}
