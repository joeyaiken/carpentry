using Carpentry.Data.DataModels;
using Carpentry.Data.DataContext;
using Carpentry.Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Carpentry.Data.Models;
using Carpentry.Data.DataModels.QueryResults;
using Carpentry.Data.Exceptions;

namespace Carpentry.Data.Implementations
{
    public class CardDataRepo : ICardDataRepo
    {
        private readonly CarpentryDataContext _cardContext;
        
        private readonly ILogger<CardDataRepo> _logger;

        public CardDataRepo(CarpentryDataContext cardContext, ILogger<CardDataRepo> logger)
        {
            _cardContext = cardContext;
            _logger = logger;
        }

        public async Task<List<string>> GetAllCardSetCodes()
        {
            List<string> result = await _cardContext.Sets.Select(x => x.Code).ToListAsync();
            return result;
        }

        public async Task<DateTime?> GetCardSetLastUpdated(string setCode)
        {
            DateTime? setLastUpdated = await _cardContext.Sets.Where(x => x.Code == setCode).Select(x => x.LastUpdated).FirstOrDefaultAsync();
            return setLastUpdated;
        }

        public async Task<CardSetData> GetCardSetById(int setId)
        {
            CardSetData result = await _cardContext.Sets.FirstOrDefaultAsync(s => s.Id == setId);
            return result;
        }

        public async Task<CardSetData> GetCardSetByCode(string setCode)
        {
            CardSetData result = await _cardContext.Sets.FirstOrDefaultAsync(s => s.Code == setCode.ToLower());
            return result;
        }

        public IQueryable<SetTotalsResult> QuerySetTotals()
        {
            return _cardContext.SetTotals.AsQueryable();
        }

        //This probably doesn't actually have to return an ID
        public async Task<int> AddOrUpdateCardSet(CardSetData setData)
        {
            //TODO Actually map between DTOs instead of blindly taking the obj
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
            return setData.Id;
        }

        /// <summary>
        /// Adds a batch of card definitions
        /// Assumes the card's set definition already exists in the DB
        /// Assumes all of the cards should be added to the DB, and none already exist
        ///     TODO - Should ensure a natural key covers SetId/CollectionNumber
        /// </summary>
        /// <param name="cards"></param>
        /// <returns></returns>
        public async Task AddCardDataBatch(List<CardDataDto> cards)
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
                SetId = allSets.Where(s => s.Code == dto.Set).FirstOrDefault().Id,
                RarityId = GetRarityId(dto.Rarity),
                CollectorNumber = dto.CollectorNumber,
                ImageUrl = dto.ImageUrl,
                Price = dto.Price,
                PriceFoil = dto.PriceFoil,
                TixPrice = dto.TixPrice,
                Color = string.Join("",dto.Colors),
                ColorIdentity = string.Join("",dto.ColorIdentity),
                Legalities = allFormats
                    .Where(format => dto.Legalities.Contains(format.Name))
                    .Select(format => new CardLegalityData
                    {
                        FormatId = format.Id,
                    }).ToList(),
            });

            await _cardContext.Cards.AddRangeAsync(newCards);

            await _cardContext.SaveChangesAsync();
        }

        /// <summary>
        /// Updates variable data on a batch of cards already in the DB
        /// Will update Legality data & variant data
        /// </summary>
        /// <param name="cards"></param>
        /// <returns></returns>
        public async Task UpdateCardDataBatch(List<CardDataDto> cards)
        {
            //Assumes all card definitions already exist in the DB (does it have to?)

            List<CardDataDto> unmatchedCards = new List<CardDataDto>();

            foreach(var card in cards)
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
                    if(existingCard == null)
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
                    var allExistingLegalities = _cardContext.CardLegalities.Where(x => x.CardId == existingCard.Id).Include(x => x.Format);

                    //IDK if this will get messed up by case sensitivity
                    var existingLegalitiesToDelete = allExistingLegalities.Where(x => !card.Legalities.Contains(x.Format.Name));

                    var legalityStringsToKeep = allExistingLegalities.Where(x => card.Legalities.Contains(x.Format.Name)).Select(x => x.Format.Name);

                    var legalitiesToAdd = card.Legalities
                        .Where(x => !legalityStringsToKeep.Contains(x))
                        .Select(x => new CardLegalityData()
                        {
                            CardId = existingCard.Id,//MultiverseId
                            Format = _cardContext.MagicFormats.Where(f => f.Name == x).FirstOrDefault(),
                        })
                        .Where(x => x.Format != null)
                        .ToList();

                    if (existingLegalitiesToDelete.Any())
                        _cardContext.CardLegalities.RemoveRange(existingLegalitiesToDelete);
                    if (legalitiesToAdd.Any())
                        _cardContext.CardLegalities.AddRange(legalitiesToAdd);
                }
                catch(Exception ex)
                {
                    throw;
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

        public async Task<CardData> GetCardData(int cardId)
        {
            CardData result = await _cardContext.Cards.FirstOrDefaultAsync(x => x.Id == cardId);
            return result;
        }

        public async Task<CardData> GetCardData(string name, string setCode)
        {
            //var matchingSet = await _cardContext.Sets.FirstOrDefaultAsync(x => x.Code.ToLower() == setCode.ToLower());

            var matchingCard = await _cardContext.Cards
                .Where(x => x.Set.Code.ToLower() == setCode.ToLower() && x.Name.ToLower() == name.ToLower())
                //.Include(x => x.Variants).ThenInclude(v => v.Type)
                //.Include(x => x.CardColorIdentities)
                //.Include(x => x.CardColors)
                .Include(x => x.Legalities)
                .Include(c => c.Set)
                .Include(c => c.Rarity)
                .FirstOrDefaultAsync();

            if(matchingCard == null)
            {
                throw new CardNotFoundException(setCode, name);
            }

            //CardData result = await _cardContext.Cards.FirstOrDefaultAsync(x => x.Id == multiverseId);
            return matchingCard;
        }
        
        public async Task<CardData> GetCardData(string setCode, int collectorNumber)
        {
            var matchingCard = await _cardContext.Cards
                .Where(x => x.Set.Code.ToLower() == setCode.ToLower() && x.CollectorNumber == collectorNumber)
                //.Include(x => x.Variants).ThenInclude(v => v.Type)
                //.Include(x => x.CardColorIdentities)
                //.Include(x => x.CardColors)
                .Include(x => x.Legalities)
                .Include(c => c.Set)
                .Include(c => c.Rarity)
                .FirstOrDefaultAsync();

            if (matchingCard == null)
            {
                throw new CardNotFoundException(setCode, collectorNumber);
            }

            //CardData result = await _cardContext.Cards.FirstOrDefaultAsync(x => x.Id == multiverseId);
            return matchingCard;
        }

        public async Task<List<CardData>> GetCardsByName(string cardName)
        {
            var result = await _cardContext.Cards.Where(x => x.Name == cardName)
                //.Include(c => c.Variants).ThenInclude(v => v.Type)
                //.Include(c => c.CardColorIdentities).ThenInclude(ci => ci.ManaType)
                //.Include(c => c.CardColors).ThenInclude(c => c.ManaType)
                .Include(c => c.Legalities).ThenInclude(l => l.Format)
                .Include(c => c.Rarity)
                .Include(c => c.Set)
                .ToListAsync();
            
            return result;
        }

        
        public async Task<List<CardSetData>> GetAllCardSets()
        {
            var result = await _cardContext.Sets.ToListAsync();
            return result;
        }

        public async Task RemoveAllCardDefinitionsForSetId(int setId)
        {
            //this check probably doesn't belong here, but just to be safe...
            //check for any inventory cards belonging to this set
            var inventoryCardCount = await _cardContext.InventoryCards.Where(ic => ic.Card.SetId == setId).CountAsync();
            if(inventoryCardCount > 0)
            {
                throw new Exception("Cannot delete a set with owned cards");
            }

            var cardsToDelete = _cardContext.Cards.Where(c => c.SetId == setId).ToList();

            _cardContext.Cards.RemoveRange(cardsToDelete);

            await _cardContext.SaveChangesAsync();

        }

        #region private

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
                default:
                    throw new Exception("Error reading scryfall rarity");

            }
            return rarity;
        }

        //private async Task AddCardDefinition(CardDataDto dto)
        //{
        //    //ScryfallMagicCard scryfallCard = JsonConvert.DeserializeObject<ScryfallMagicCard>(scryfallDBCard.StringData);

        //    //besides just adding the card, also need to add

        //    //ColorIdentities
        //    List<string> dtoColorIdentities = dto.ColorIdentity ?? new List<string>();
        //    List<ManaTypeData> relevantColorIdentityManaTypes = _cardContext.ManaTypes
        //        .AsEnumerable()
        //        .Where(x => dtoColorIdentities.Contains(x.Id.ToString()))
        //        .ToList();

        //    //CardColors,
        //    List<string> dtoColors = dto.Colors ?? new List<string>();
        //    List<ManaTypeData> relevantColorManaTypes = _cardContext.ManaTypes
        //        .AsEnumerable()
        //        .Where(x => dtoColors.Contains(x.Id.ToString()))
        //        .ToList();

        //    //CardLegalities, 
        //    List<string> scryCardLegalities = dto.Legalities.ToList();

        //    //NOTE - This ends up ignoring any formats that don't actually exist in the DB
        //    //as of 11-16-2019, this is the desired effect
        //    List<MagicFormatData> relevantDbFormats = _cardContext.MagicFormats
        //        .AsEnumerable()
        //        .Where(x => scryCardLegalities.Contains(x.Name))
        //        .ToList();

        //    //CardVariants
        //    List<string> dtoVariantNames = dto.Variants.Select(x => x.Name).ToList();
        //    List<CardVariantTypeData> relevantDBVariantTypes = _cardContext.VariantTypes
        //        .AsEnumerable()
        //        .Where(x => dtoVariantNames.Contains(x.Name))
        //        .ToList();


        //    var dbSet = _cardContext.Sets.FirstOrDefault(x => x.Code == dto.Set);
        //    if (dbSet == null)
        //    {
        //        CardSetData newSet = new CardSetData()
        //        {
        //            Code = dto.Set,
        //            Name = dto.Set,
        //        };
        //        _cardContext.Sets.Add(newSet);
        //        dbSet = newSet;
        //    }

        //    //char rarity;
        //    //switch (dto.Rarity)
        //    //{
        //    //    case "mythic":
        //    //        rarity = 'M';
        //    //        break;

        //    //    case "rare":
        //    //        rarity = 'R';
        //    //        break;
        //    //    case "uncommon":
        //    //        rarity = 'U';
        //    //        break;
        //    //    case "common":
        //    //        rarity = 'C';
        //    //        break;
        //    //    default:
        //    //        throw new Exception("Error reading scryfall rarity");

        //    //}

        //    var newCard = new CardData
        //    {
        //        Id = dto.MultiverseId,
        //        Cmc = dto.Cmc,
        //        ManaCost = dto.ManaCost,
        //        Name = dto.Name,
        //        Text = dto.Text,
        //        Type = dto.Type,

        //        //Set
        //        Set = dbSet, //Should this be the ID instead?

        //        //Rarity
        //        RarityId = GetRarityId(dto.Rarity),

        //        //Color
        //        //jank
        //        CardColors = relevantColorManaTypes.Select(x => new CardColorData
        //        {
        //            ManaType = x,
        //        }).ToList(),


        //        //Color Identity
        //        CardColorIdentities = relevantColorIdentityManaTypes.Select(x => new CardColorIdentityData
        //        {
        //            ManaType = x,
        //        }).ToList(),

        //        //Variants
        //        Variants = dto.Variants.Select(x => new CardVariantData()
        //        {
        //            ImageUrl = x.Image,
        //            Price = x.Price,
        //            PriceFoil = x.PriceFoil,
        //            Type = relevantDBVariantTypes.FirstOrDefault(v => v.Name == x.Name)
        //        }).ToList(),

        //        //Legalities
        //        //This is weird, but I only want to add legalities that exist in the DB
        //        Legalities = relevantDbFormats.Select(format => new CardLegalityData
        //        {
        //            Format = format
        //        }).ToList(),



        //    };

        //    int preSaveId = newCard.Id;
        //    _cardContext.Cards.Add(newCard);
        //    await _cardContext.SaveChangesAsync();
        //}

        //private async Task UpdateCardDefinition(CardDataDto dto)
        //{
        //    var cardToUpdate = _cardContext.Cards.FirstOrDefault(x => x.Id == dto.MultiverseId);

        //    if (cardToUpdate == null)
        //    {
        //        throw new Exception("Could not find the card to update");
        //    }

        //    //So all of the things I have to update don't actually exist on the Cards table

        //    //I need to possibly add/remove legalities, and I need to update the price on variants

        //    var existingVariants = _cardContext.CardVariants
        //        .Where(x => x.CardId == cardToUpdate.Id).Include(x => x.Type);

        //    await existingVariants.ForEachAsync(v =>
        //    {
        //        string variantName = v.Type.Name;

        //        CardVariantDto matchingDtoVariant = dto.Variants.Where(dtoV => dtoV.Name.ToLower() == variantName.ToLower()).FirstOrDefault();

        //        v.Price = matchingDtoVariant.Price;
        //        v.PriceFoil = matchingDtoVariant.PriceFoil;
        //        v.ImageUrl = matchingDtoVariant.Image; //why not update this too
        //    });
            
        //    _cardContext.CardVariants.UpdateRange(existingVariants);

        //    //what if there are new variants to add?(hint hint)

        //    //.Select
        //    //.Select(x => new CardVariant()
        //    //{
        //    //    Id = x.Id,
        //    //    CardId = x.CardId,
        //    //    ImageUrl = x.ImageUrl,
        //    //    CardVariantTypeId = x.CardVariantTypeId,
        //    //    Price = scryfallCard.Prices[x.Type.Name],
        //    //    PriceFoil = scryfallCard.Prices[$"{x.Type.Name}_foil"]
        //    //});


        //    //existingVariants.ForEach(variant =>
        //    //{
        //    //    variant.Price = scryfallCard.Prices[variant.;
        //    //    variant.PriceFoil = 0;
        //    //});


        //    var allExistingLegalities = _cardContext.CardLegalities.Where(x => x.CardId == cardToUpdate.Id).Include(x => x.Format);

        //    //IDK if this will get messed up by case sensitivity
        //    var existingLegalitiesToDelete = allExistingLegalities.Where(x => !dto.Legalities.Contains(x.Format.Name));

        //    var legalityStringsToKeep = allExistingLegalities.Where(x => dto.Legalities.Contains(x.Format.Name)).Select(x => x.Format.Name);

        //    var legalitiesToAdd = dto.Legalities
        //        .Where(x => !legalityStringsToKeep.Contains(x))
        //        .Select(x => new CardLegalityData()
        //        {
        //            CardId = cardToUpdate.Id,
        //            Format = _cardContext.MagicFormats.Where(f => f.Name == x).FirstOrDefault(),
        //        })
        //        .Where(x => x.Format != null)
        //        .ToList();

        //    //var something = dto.Legalities
        //    //    .Where(x => !legalityStringsToKeep.Contains(x))
        //    //    .Select(x => new
        //    //    {
        //    //        x,
        //    //        CardId = cardToUpdate.Id,
        //    //        Format = _cardContext.MagicFormats.Where(f => f.Name == x).FirstOrDefault(),
        //    //    }).ToList();

        //    if (existingLegalitiesToDelete.Any())
        //        _cardContext.CardLegalities.RemoveRange(existingLegalitiesToDelete);
        //    if (legalitiesToAdd.Any())
        //        _cardContext.CardLegalities.AddRange(legalitiesToAdd);
        //    await _cardContext.SaveChangesAsync();

        //}

        

        #endregion

    }
}
