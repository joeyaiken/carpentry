using Carpentry.CarpentryData;
using Carpentry.CarpentryData.Models;
using Carpentry.CarpentryData.Models.QueryResults;
using Carpentry.DataLogic.Exceptions;
using Carpentry.DataLogic.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Carpentry.DataLogic
{
    [Obsolete]
    public interface ICardDataRepo
    {
        //Sets
        
        [Obsolete]
        Task<CardSetData> GetCardSetById(int setId);

        [Obsolete]
        Task<int> AddOrUpdateCardSet(CardSetData setData); //This probably doesn't actually have to return an ID

        [Obsolete]
        IQueryable<SetTotalsResult> QuerySetTotals();

        //Cards
        [Obsolete]
        Task AddCardDataBatch(List<CardDataDto> cards);

        [Obsolete]
        Task UpdateCardDataBatch(List<CardDataDto> cards);


        [Obsolete]
        Task RemoveAllCardDefinitionsForSetId(int setId);
    }

    [Obsolete]
    public class CardDataRepo : ICardDataRepo
    {
        private readonly CarpentryDataContext _cardContext;
        
        public CardDataRepo(CarpentryDataContext cardContext)
        {
            _cardContext = cardContext;
        }

        public async Task<CardSetData> GetCardSetById(int setId)
        {
            CardSetData result = await _cardContext.Sets.FirstOrDefaultAsync(s => s.SetId == setId);
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
                SetId = allSets.Where(s => s.Code == dto.Set).FirstOrDefault().SetId,
                RarityId = GetRarityId(dto.Rarity),
                CollectorNumber = dto.CollectorNumber,
                ImageUrl = dto.ImageUrl,
                Price = dto.Price,
                PriceFoil = dto.PriceFoil,
                TixPrice = dto.TixPrice,
                Color = dto.Colors == null ? null : string.Join("",dto.Colors),
                ColorIdentity = string.Join("",dto.ColorIdentity),
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
                catch(Exception ex)
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

    }
}
