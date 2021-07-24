using Carpentry.Logic.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Carpentry.CarpentryData.Models;
using Carpentry.CarpentryData;
using Microsoft.EntityFrameworkCore;

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
    //Note on "premium" cards
    //  For WAR, scryfall includes 36 Japanese alt-art plainswalkers
    //  For now, I'm intentionally not adding them to the system
    //  IDK what other sets include premium cards, but they may exist
    //  A premium card is an card with a collector_number that includes a star/★
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
            //IDataQueryService dataQueryService
            CarpentryDataContext cardContext
            )
        {
            _logger = logger;
            _scryService = scryService;
            _dataIntegrityService = dataIntegrityService;
            _dbRefreshIntervalDays = 0; //TODO - move to a config
            //_dataQueryService = dataQueryService;
            _cardContext = cardContext;
        }

        #region Private Methods

        /// <summary>
        /// This gets a list of card definitions from scryfall, mapped to a different class type
        /// </summary>
        /// <param name="setId"></param>
        /// <param name="setCode"></param>
        /// <returns></returns>
        private async Task<List<CardDataDto>> GetUpdatedScrySet(/*int setId,*/ string setCode)
        {
            var scryPayload = await _scryService.GetSetDetail(setCode);

            var mappedCards = scryPayload.Cards
                //.Where(card => !card.IsPremium) //DODO - stop filtering out premium cards (premium cards include a character in collector#)
                .Select(card => new CardDataDto()
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
                CollectorNumber = card.CollectionNumber,
                ImageUrl = card.ImageUrl,
                Price = card.Price,
                PriceFoil = card.PriceFoil,
                TixPrice = card.PriceTix,

            }).ToList();

            return mappedCards;
        }

        private async Task RemoveAllCardDefinitionsForSetId(int setId)
        {
            //this check probably doesn't belong here, but just to be safe...
            //check for any inventory cards belonging to this set
            var inventoryCardCount = await _cardContext.InventoryCards.Where(ic => ic.Card.SetId == setId).CountAsync();
            if (inventoryCardCount > 0)
            {
                throw new Exception("Cannot delete a set with owned cards");
            }

            var cardsToDelete = _cardContext.Cards.Where(c => c.SetId == setId).ToList();

            _cardContext.Cards.RemoveRange(cardsToDelete);

            await _cardContext.SaveChangesAsync();

        }

        /// <summary>
        /// Updates variable data on a batch of cards already in the DB
        /// Will update Legality data
        /// </summary>
        /// <param name="cards"></param>
        /// <returns></returns>
        private async Task UpdateCardDataBatch(List<CardDataDto> cards)
        {
            //Assumes all card definitions already exist in the DB (does it have to?)

            List<CardDataDto> unmatchedCards = new List<CardDataDto>();

            foreach (var card in cards)
            {
                try
                {
                    //I'm no longer storing variants the same way, so now I need to update all card definitions in the batch

                    //get the card by natural key
                    //NOTE - NATURAL KEY CAN'T BE TRUSTED YET, COLLECTION NUMBERS DON'T WORK
                    //var existingCard = _cardContext.Cards.Where(x => x.Set.Code == card.Set && x.CollectorNumber == card.CollectorNumber).FirstOrDefault();

                    //var matchingByName = _cardContext.Cards.Where(x => x.Name == card.Name).ToList();




                    //first, try to match on Set/Name/Collection number
                    var existingCard = _cardContext.Cards.Where(
                        x => x.Set.Code == card.Set
                        && x.CollectorNumber == card.CollectorNumber).FirstOrDefault();

                    //Then, if that doesn't work, try to just match on Name/Set/MID
                    if (existingCard == null)
                    {
                        existingCard = _cardContext.Cards.Where(
                            x => x.Set.Code == card.Set
                            && x.Name == card.Name
                            && card.MultiverseId == x.MultiverseId).FirstOrDefault();
                    }

                    //it it's still not found, assume it doesn't exist yet
                    if (existingCard == null)
                    {
                        unmatchedCards.Add(card);
                        continue;
                    }

                    //variable fields
                    existingCard.Price = card.Price;
                    existingCard.PriceFoil = card.PriceFoil;
                    existingCard.TixPrice = card.TixPrice;

                    //Static fields for cleaning up the data
                    existingCard.ImageUrl = card.ImageUrl;
                    existingCard.Color = card.Colors == null ? null : string.Join("", card.Colors);
                    existingCard.ColorIdentity = card.ColorIdentity == null ? null : string.Join("", card.ColorIdentity);
                    existingCard.CollectorNumber = card.CollectorNumber;

                    _cardContext.Cards.Update(existingCard);

                    //Update variants
                    //var existingVariants = _cardContext.CardVariants
                    //    .Where(x => x.CardId == card.MultiverseId).Include(x => x.Type).ToList();

                    //foreach (var variant in existingVariants)
                    //{
                    //    string variantName = variant.Type.Name;

                    //    CardVariantDto matchingDtoVariant = card.Variants.Where(dtoV => dtoV.Name.ToLower() == variantName.ToLower()).FirstOrDefault();

                    //    if(matchingDtoVariant == null)
                    //    {
                    //        //there's some mismatch. Is there another, non-normal, variant, not in the DTO?
                    //        //need to check the DTO to see if there's anything not contained in ExistingVariants

                    //        var excludedDtoVariants = card.Variants.Where(dtoV => !existingVariants.Select(v => v.Type.Name).Contains(dtoV.Name)).ToList();

                    //        //var excludedVariants = existingVariants.Where(ev => !card.Variants.Select(dtoV => dtoV.Name).Contains(ev.Type.Name)).ToList();

                    //        if(excludedDtoVariants.Count == 1)
                    //        {
                    //            matchingDtoVariant = excludedDtoVariants[0];
                    //        }
                    //        else
                    //        {
                    //            throw new Exception("Error matching card variants");
                    //        }



                    //    }


                    //    variant.Price = matchingDtoVariant.Price;
                    //    variant.PriceFoil = matchingDtoVariant.PriceFoil;
                    //    variant.ImageUrl = matchingDtoVariant.Image; //why not update this too

                    //}

                    //await existingVariants.ForEachAsync(v =>
                    //{
                    //    string variantName = v.Type.Name;

                    //    CardVariantDto matchingDtoVariant = card.Variants.Where(dtoV => dtoV.Name.ToLower() == variantName.ToLower()).FirstOrDefault();

                    //    v.Price = matchingDtoVariant.Price;
                    //    v.PriceFoil = matchingDtoVariant.PriceFoil;
                    //    v.ImageUrl = matchingDtoVariant.Image; //why not update this too
                    //});

                    //_cardContext.CardVariants.UpdateRange(existingVariants);

                    //Update legalities
                    var allExistingLegalities = _cardContext.CardLegalities.Where(x => x.CardId == existingCard.CardId).Include(x => x.Format);

                    //IDK if this will get messed up by case sensitivity
                    var existingLegalitiesToDelete = allExistingLegalities.Where(x => !card.Legalities.Contains(x.Format.Name));

                    var legalityStringsToKeep = allExistingLegalities.Where(x => card.Legalities.Contains(x.Format.Name)).Select(x => x.Format.Name);

                    var legalitiesToAdd = card.Legalities
                        .Where(x => !legalityStringsToKeep.Contains(x))
                        .Select(x => new CardLegalityData()
                        {
                            CardId = existingCard.CardId,//MultiverseId
                            Format = _cardContext.MagicFormats.Where(f => f.Name == x).FirstOrDefault(),
                        })
                        .Where(x => x.Format != null)
                        .ToList();

                    if (existingLegalitiesToDelete.Any())
                        _cardContext.CardLegalities.RemoveRange(existingLegalitiesToDelete);
                    if (legalitiesToAdd.Any())
                        _cardContext.CardLegalities.AddRange(legalitiesToAdd);
                }
                catch (Exception ex)
                {
                    throw ex;
                }


            }

            await _cardContext.SaveChangesAsync();

            await AddCardDataBatch(unmatchedCards);




            //or
            //  Try to be clever and pull everything at once
            //      Would I still have to itterate somewhere along the way?
            //var dbLegalities = _cardContext.CardLegalities
            //    .Join(
            //        cards,
            //        dbL => dbL,
            //        card => card.,
            //        (db, card) => new
            //        {

            //        }
            //    ).ToList();



            //Update all variants



        }

        //This probably doesn't actually have to return an ID
        private async Task<int> AddOrUpdateCardSet(CardSetData setData)
        {
            //TODO Actually map between DTOs instead of blindly taking the obj
            //  Followup: Why?
            var existingSet = _cardContext.Sets.Where(x => x.Code.ToLower() == setData.Code.ToLower()).FirstOrDefault();
            if (existingSet != null)
            {
                existingSet.Code = setData.Code;
                existingSet.LastUpdated = setData.LastUpdated;
                existingSet.Name = setData.Name;
                existingSet.ReleaseDate = setData.ReleaseDate;

                //setData.Id = existingSet.Id;
                _cardContext.Sets.Update(existingSet);
            }
            else
            {
                _cardContext.Sets.Add(setData);
            }
            await _cardContext.SaveChangesAsync();
            return setData.SetId;
        }

        /// <summary>
        /// Adds a batch of card definitions
        /// Assumes the card's set definition already exists in the DB
        /// Assumes all of the cards should be added to the DB, and none already exist
        ///     TODO - Should ensure a natural key covers SetId/CollectionNumber
        /// </summary>
        /// <param name="cards"></param>
        /// <returns></returns>
        private async Task AddCardDataBatch(List<CardDataDto> cards)
        {
            List<MagicFormatData> allFormats = _cardContext.MagicFormats.ToList();
            List<CardSetData> allSets = _cardContext.Sets.ToList();

            var newCards = cards.Select(dto => new CardData
            {
                //Id = dto.CardId,
                MultiverseId = dto.MultiverseId,
                Cmc = dto.Cmc,
                ManaCost = dto.ManaCost,
                Name = dto.Name,
                Text = dto.Text,
                Type = dto.Type,
                SetId = allSets.Where(s => s.Code == dto.Set).FirstOrDefault().SetId,
                RarityId = GetRarityId(dto.Rarity),
                CollectorNumber = dto.CollectorNumber,
                ImageUrl = dto.ImageUrl,
                Price = dto.Price,
                PriceFoil = dto.PriceFoil,
                TixPrice = dto.TixPrice,
                Color = dto.Colors == null ? null : string.Join("", dto.Colors),
                ColorIdentity = string.Join("", dto.ColorIdentity),
                Legalities = allFormats
                    .Where(format => dto.Legalities.Contains(format.Name))
                    .Select(format => new CardLegalityData
                    {
                        FormatId = format.FormatId,
                    }).ToList(),
            });

            await _cardContext.Cards.AddRangeAsync(newCards);

            await _cardContext.SaveChangesAsync();
        }

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
            //get the set for this ID
            var dbSet = await _cardContext.Sets
                .Include(s => s.Cards)
                .FirstOrDefaultAsync(s => s.SetId == setId);

            if (dbSet == null) throw new ArgumentException($"No set matching provided set ID of {setId}");
            
            if (dbSet.IsTracked) return;

            if (dbSet.Cards.Any()) throw new ArgumentException($"Set {dbSet.Name} is supposed to be untracked but already contains cards");





            //In order to add this set we...
            //Get a scry set
            //map the scry set to [some dto record]
            //Get the DB set
            //Add all cards individually after re-querying the set (and ALL sets, wow this was dumb)
            //  Including mapping to a 3rd record (the actual DB record)





            //get current scry data
            //var scryData = await GetUpdatedScrySet(dbSet.Code);
            var scryData = await _scryService.GetSetDetail(dbSet.Code);


            //var formatIdsByName = await _cardContext.MagicFormats
            //    .ToDictionaryAsync(f => f.Name, f => f.FormatId);
            List<MagicFormatData> allFormats = _cardContext.MagicFormats.ToList();

            var newCards = scryData.Cards.Select(dto => new CardData
            {
                //Id = dto.CardId,
                MultiverseId = dto.MultiverseId,
                Cmc = dto.Cmc,
                ManaCost = dto.ManaCost,
                Name = dto.Name,
                Text = dto.Text,
                Type = dto.Type,
                //SetId = allSets.Where(s => s.Code == dto.Set).FirstOrDefault().SetId,
                SetId = dbSet.SetId,
                RarityId = GetRarityId(dto.Rarity),
                CollectorNumber = dto.CollectionNumber,
                ImageUrl = dto.ImageUrl,
                Price = dto.Price,
                PriceFoil = dto.PriceFoil,
                TixPrice = dto.PriceTix,
                Color = dto.Colors == null ? null : string.Join("", dto.Colors),
                ColorIdentity = string.Join("", dto.ColorIdentity),
                CollectorNumberStr = "",
                CollectorNumberSuffix = null,

                //This may seem weird, but I'm not tracking all the formats listed in scryfall
                Legalities = allFormats
                    .Where(format => dto.Legalities.Contains(format.Name))
                    .Select(format => new CardLegalityData
                    {
                        FormatId = format.FormatId,
                    }).ToList(),
                //Legalities = dto.Legalities.Select(formatName => new CardLegalityData
                //{
                //    FormatId = formatIdsByName[formatName],
                //}).ToList(),
            });

            //dbSet.Cards.AddRange()

            _cardContext.Cards.AddRange(newCards);



            //var premiumCards = scryData.Where(c => c.)

//            await AddCardDataBatch(scryData);

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
            var dbSet = await _cardContext.Sets.FirstOrDefaultAsync(s => s.SetId == setId);

            if (dbSet == null)
            {
                throw new ArgumentException($"No set matching provided set ID of {setId}");
            }

            if (!dbSet.IsTracked)
            {
                return; //Don't want to update untracked sets
            }

            //Only update a set if it's "stale enough"
            if (dbSet.LastUpdated != null && dbSet.LastUpdated.Value.AddDays(_dbRefreshIntervalDays) > DateTime.Today.Date)
            {
                _logger.LogInformation($"UpdateSetData - Card set {dbSet.Code} is already up to date, nothing will be updated.");
                return;
            }

            //Get the updated card definitions, mapped to the expected CardData object
            var scryData = await GetUpdatedScrySet(dbSet.Code);

            await UpdateCardDataBatch(scryData); // Should this really use a DTO?? w/e for now...

            //Finally, update the Set Definition so we know it's up to date
            dbSet.LastUpdated = DateTime.Now;
            await AddOrUpdateCardSet(dbSet);
        }

        public async Task RemoveTrackedSet(int setId)
        {
            var dbSet = await _cardContext.Sets.FirstOrDefaultAsync(s => s.SetId == setId);
            var setTotals = await _cardContext.GetSetTotals().FirstOrDefaultAsync(s => s.SetId == setId);

            if (setTotals == null)
            {
                throw new ArgumentException($"No set matching provided set ID of {setId}");
            }

            if (!setTotals.IsTracked)
            {
                return; //If not tracked, just return
            }
            //get DB set

            //verify 0 owned/collected

            if (setTotals.InventoryCount > 0 || setTotals.CollectedCount > 0)
            {
                return; //won't delete anything with owned cards
                //TODO - should be able to remove a set with only buylist cards?
            }

            //remove card definitions
            await RemoveAllCardDefinitionsForSetId(setId);

            //update set
            dbSet.LastUpdated = null;
            dbSet.IsTracked = false;
            await AddOrUpdateCardSet(dbSet);
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

                await AddOrUpdateCardSet(existingSet);
            }
        }

        #endregion

    }
}
