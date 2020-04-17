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

        //This probably doesn't actually have to return an ID
        public async Task<int> AddOrUpdateCardSet(CardSetData setData)
        {
            //TODO Actually map between DTOs instead of blindly taking the obj
            var existingSet = _cardContext.Sets.Where(x => x.Code.ToLower() == setData.Code.ToLower()).FirstOrDefault();
            if (existingSet != null)
            {
                existingSet.Cards = setData.Cards;
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

        public async Task AddOrUpdateCardDefinition(CardDataDto cardData)
        {
            if (cardData.MultiverseId == 121138)
            {
                int breakpoint = 1;
            }

            CardData cardFromDb = _cardContext.Cards.Where(x => x.Id == cardData.MultiverseId).FirstOrDefault();

            if (cardFromDb == null)
            {
                await AddCardDefinition(cardData);
            }
            else
            {
                await UpdateCardDefinition(cardData);
            }

        }

        //CardDataDto
        //private async Task AddCardDefinition(ScryfallMagicCard scryfallCard)
        public async Task AddCardDefinition(CardDataDto dto)
        {
            //ScryfallMagicCard scryfallCard = JsonConvert.DeserializeObject<ScryfallMagicCard>(scryfallDBCard.StringData);

            //besides just adding the card, also need to add

            //ColorIdentities
            List<string> dtoColorIdentities = dto.ColorIdentity ?? new List<string>();
            List<ManaTypeData> relevantColorIdentityManaTypes = _cardContext.ManaTypes
                .AsEnumerable()
                .Where(x => dtoColorIdentities.Contains(x.Id.ToString()))
                .ToList();

            //CardColors,
            List<string> dtoColors = dto.Colors ?? new List<string>();
            List<ManaTypeData> relevantColorManaTypes = _cardContext.ManaTypes
                .AsEnumerable()
                .Where(x => dtoColors.Contains(x.Id.ToString()))
                .ToList();

            //CardLegalities, 
            List<string> scryCardLegalities = dto.Legalities.ToList();

            //NOTE - This ends up ignoring any formats that don't actually exist in the DB
            //as of 11-16-2019, this is the desired effect
            List<MagicFormatData> relevantDbFormats = _cardContext.MagicFormats
                .AsEnumerable()
                .Where(x => scryCardLegalities.Contains(x.Name))
                .ToList();

            //CardVariants
            List<string> dtoVariantNames = dto.Variants.Select(x => x.Name).ToList();
            List<CardVariantTypeData> relevantDBVariantTypes = _cardContext.VariantTypes
                .AsEnumerable()
                .Where(x => dtoVariantNames.Contains(x.Name))
                .ToList();


            var dbSet = _cardContext.Sets.FirstOrDefault(x => x.Code == dto.Set);
            if (dbSet == null)
            {
                CardSetData newSet = new CardSetData()
                {
                    Code = dto.Set,
                    Name = dto.Set,
                };
                _cardContext.Sets.Add(newSet);
                dbSet = newSet;
            }

            char rarity;
            switch (dto.Rarity)
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
            
            var newCard = new CardData
            {
                Id = dto.MultiverseId,
                Cmc = dto.Cmc,
                ManaCost = dto.ManaCost,
                Name = dto.Name,
                Text = dto.Text,
                Type = dto.Type,

                //Set
                Set = dbSet, //Should this be the ID instead?

                //Rarity
                RarityId = rarity,

                //Color
                //jank
                CardColors = relevantColorManaTypes.Select(x => new CardColorData
                {
                    ManaType = x,
                }).ToList(),


                //Color Identity
                CardColorIdentities = relevantColorIdentityManaTypes.Select(x => new CardColorIdentityData
                {
                    ManaType = x,
                }).ToList(),

                //Variants
                Variants = dto.Variants.Select(x => new CardVariantData()
                {
                    ImageUrl = x.Image,
                    Price = x.Price,
                    PriceFoil = x.PriceFoil,
                    Type = relevantDBVariantTypes.FirstOrDefault(v => v.Name == x.Name)
                }).ToList(),

                //Legalities
                //This is weird, but I only want to add legalities that exist in the DB
                Legalities = relevantDbFormats.Select(format => new CardLegalityData
                {
                    Format = format
                    //Format = relevantDbFormats.FirstOrDefault(f => f.Name == format.Name),
                }).ToList(),

                //Legalities = dto.Legalities.Select(l => new CardLegalityData
                //{
                //    Format = relevantDbFormats.FirstOrDefault(f => f.Name == l),
                //}).ToList(),


                ////still janky
                //Variants = relevantDBVariantTypes.Select(x => new CardVariantData
                //{
                //    ImageUrl = dto.Variants[x.Name],
                //    Price = dto.Prices[x.Name],
                //    PriceFoil = dto.Prices[$"{x.Name}_foil"],
                //    Type = x,
                //}).ToList(),

                ////Legalities
                ////This feels janky
                //Legalities = relevantDbFormats.Select(x => new CardLegalityData { Format = x }).ToList(),

            };

            _cardContext.Cards.Add(newCard);
            await _cardContext.SaveChangesAsync();
        }

        //private async Task UpdateCardDefinition(ScryfallMagicCard scryfallCard)
        public async Task UpdateCardDefinition(CardDataDto dto)
        {
            var cardToUpdate = _cardContext.Cards.FirstOrDefault(x => x.Id == dto.MultiverseId);

            if (cardToUpdate == null)
            {
                throw new Exception("Could not find the card to update");
            }

            //So all of the things I have to update don't actually exist on the Cards table

            //I need to possibly add/remove legalities, and I need to update the price on variants

            var existingVariants = _cardContext.CardVariants
                .Where(x => x.CardId == cardToUpdate.Id).Include(x => x.Type);

            await existingVariants.ForEachAsync(v =>
            {
                string variantName = v.Type.Name;

                CardVariantDto matchingDtoVariant = dto.Variants.Where(dtoV => dtoV.Name.ToLower() == variantName.ToLower()).FirstOrDefault();

                v.Price = matchingDtoVariant.Price;
                v.PriceFoil = matchingDtoVariant.PriceFoil;
                v.ImageUrl = matchingDtoVariant.Image; //why not update this too
            });

            _cardContext.CardVariants.UpdateRange(existingVariants);


            //.Select
            //.Select(x => new CardVariant()
            //{
            //    Id = x.Id,
            //    CardId = x.CardId,
            //    ImageUrl = x.ImageUrl,
            //    CardVariantTypeId = x.CardVariantTypeId,
            //    Price = scryfallCard.Prices[x.Type.Name],
            //    PriceFoil = scryfallCard.Prices[$"{x.Type.Name}_foil"]
            //});


            //existingVariants.ForEach(variant =>
            //{
            //    variant.Price = scryfallCard.Prices[variant.;
            //    variant.PriceFoil = 0;
            //});


            var allExistingLegalities = _cardContext.CardLegalities.Where(x => x.CardId == cardToUpdate.Id).Include(x => x.Format);

            //IDK if this will get messed up by case sensitivity
            var existingLegalitiesToDelete = allExistingLegalities.Where(x => !dto.Legalities.Contains(x.Format.Name));

            var legalityStringsToKeep = allExistingLegalities.Where(x => dto.Legalities.Contains(x.Format.Name)).Select(x => x.Format.Name);

            var legalitiesToAdd = dto.Legalities
                .Where(x => !legalityStringsToKeep.Contains(x))
                .Select(x => new CardLegalityData()
                {
                    CardId = cardToUpdate.Id,
                    Format = _cardContext.MagicFormats.Where(f => f.Name == x).FirstOrDefault(),
                })
                .Where(x => x.Format != null)
                .ToList();

            //var something = dto.Legalities
            //    .Where(x => !legalityStringsToKeep.Contains(x))
            //    .Select(x => new
            //    {
            //        x,
            //        CardId = cardToUpdate.Id,
            //        Format = _cardContext.MagicFormats.Where(f => f.Name == x).FirstOrDefault(),
            //    }).ToList();

            if (existingLegalitiesToDelete.Any())
                _cardContext.CardLegalities.RemoveRange(existingLegalitiesToDelete);
            if (legalitiesToAdd.Any())
                _cardContext.CardLegalities.AddRange(legalitiesToAdd);
            await _cardContext.SaveChangesAsync();

        }




        public async Task<CardData> GetCardById(int multiverseId)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<CardData>> GetCardsByName(string cardName)
        {
            var result = await _cardContext.Cards.Where(x => x.Name == cardName).ToListAsync();
            return result;
        }

        public async Task EnsureDatabaseExists()
        {
            await _cardContext.Database.EnsureCreatedAsync();
        }

        #region Card related methdos

        //public async Task AddCardDefinition(ScryfallMagicCard scryfallCard)
        //{
        //    //ScryfallMagicCard scryfallCard = JsonConvert.DeserializeObject<ScryfallMagicCard>(scryfallDBCard.StringData);

        //    //besides just adding the card, also need to add

        //    //ColorIdentities
        //    var scryfallCardColorIdentities = scryfallCard.ColorIdentity ?? new List<string>();
        //    var relevantColorIdentityManaTypes = _cardContext.ManaTypes.Where(x => scryfallCardColorIdentities.Contains(x.Id.ToString())).ToList();

        //    //CardColors,
        //    var scryfallCardColors = scryfallCard.Colors ?? new List<string>();
        //    var relevantColorManaTypes = _cardContext.ManaTypes.Where(x => scryfallCardColors.Contains(x.Id.ToString())).ToList();


        //    //CardLegalities, 
        //    var scryCardLegalities = scryfallCard.Legalities.ToList();

        //    //NOTE - This ends up ignoring any formats that don't actually exist in the DB
        //    //as of 11-16-2019, this is the desired effect
        //    var relevantDbFormats = _cardContext.MagicFormats.Where(x => scryCardLegalities.Contains(x.Name)).ToList();

        //    //CardVariants
        //    var scryCardVariants = scryfallCard.Variants.Keys.ToList();
        //    var relevantDBVariantTypes = _cardContext.VariantTypes.Where(x => scryCardVariants.Contains(x.Name)).ToList();


        //    var dbSet = _cardContext.Sets.FirstOrDefault(x => x.Code == scryfallCard.Set);
        //    if (dbSet == null)
        //    {
        //        CardSet newSet = new CardSet()
        //        {
        //            Code = scryfallCard.Set,
        //            Name = scryfallCard.Set,
        //        };
        //        _cardContext.Sets.Add(newSet);
        //        dbSet = newSet;
        //    }

        //    char rarity;
        //    switch (scryfallCard.Rarity)
        //    {
        //        case "mythic":
        //            rarity = 'M';
        //            break;

        //        case "rare":
        //            rarity = 'R';
        //            break;
        //        case "uncommon":
        //            rarity = 'U';
        //            break;
        //        case "common":
        //            rarity = 'C';
        //            break;
        //        default:
        //            throw new Exception("Error reading scryfall rarity");

        //    }

        //    var newCard = new LegacyDataContext.Card
        //    {
        //        Id = scryfallCard.MultiverseId,
        //        Cmc = scryfallCard.Cmc,
        //        ManaCost = scryfallCard.ManaCost,
        //        Name = scryfallCard.Name,
        //        Text = scryfallCard.Text,
        //        Type = scryfallCard.Type,

        //        //Set
        //        Set = dbSet, //Should this be the ID instead?

        //        //Rarity
        //        RarityId = rarity,

        //        //Color
        //        //jank
        //        CardColors = relevantColorManaTypes.Select(x => new CardColor
        //        {
        //            ManaType = x,
        //        }).ToList(),


        //        //Color Identity
        //        CardColorIdentities = relevantColorIdentityManaTypes.Select(x => new LegacyDataContext.CardColorIdentity
        //        {
        //            ManaType = x,
        //        }).ToList(),

        //        //Variants
        //        //still janky
        //        Variants = relevantDBVariantTypes.Select(x => new CardVariant
        //        {
        //            ImageUrl = scryfallCard.Variants[x.Name],
        //            Price = scryfallCard.Prices[x.Name],
        //            PriceFoil = scryfallCard.Prices[$"{x.Name}_foil"],
        //            Type = x,
        //        }).ToList(),

        //        //Legalities
        //        //This feels janky
        //        Legalities = relevantDbFormats.Select(x => new CardLegality { Format = x }).ToList(),

        //    };

        //    await _cardContext.Cards.AddAsync(newCard);
        //    await _cardContext.SaveChangesAsync();
        //}


        //public async Task AddCardDefinitionBatch(List<ScryfallMagicCard> cardBatch)
        //{
        //    //lets try some nonsense
        //    IEnumerable<LegacyDataContext.Card> cardsToAdd = cardBatch.Select(scryfallCard => new LegacyDataContext.Card
        //    {
        //        Id = scryfallCard.MultiverseId,
        //        Cmc = scryfallCard.Cmc,
        //        ManaCost = scryfallCard.ManaCost,
        //        Name = scryfallCard.Name,
        //        Text = scryfallCard.Text,
        //        Type = scryfallCard.Type,

        //        Set = _cardContext.Sets.FirstOrDefault(x => x.Code == scryfallCard.Set),

        //        RarityId = GetRarityCode(scryfallCard.Rarity),


        //        CardColors = scryfallCard.Colors
        //            .Join(
        //                _cardContext.ManaTypes,
        //                color => color,
        //                mt => mt.Id.ToString(),
        //                (color, mt) => new CardColor { ManaTypeId = mt.Id }).ToList(),

        //        CardColorIdentities = scryfallCard.ColorIdentity
        //            .Join(
        //                _cardContext.ManaTypes,
        //                color => color,
        //                mt => mt.Id.ToString(),
        //                (color, mt) => new CardColorIdentity { ManaTypeId = mt.Id }).ToList(),

        //        Variants = scryfallCard.Variants
        //            .Join(
        //                _cardContext.VariantTypes,
        //                v => v.Key,
        //                vt => vt.Name,
        //                (v, vt) => new CardVariant
        //                {
        //                    ImageUrl = v.Value,
        //                    Price = scryfallCard.Prices[v.Key],
        //                    PriceFoil = scryfallCard.Prices[$"{v.Key}_foil"],
        //                    CardVariantTypeId = vt.Id,
        //                }).ToList(),

        //        Legalities = scryfallCard.Legalities
        //            .Join(
        //                _cardContext.MagicFormats,
        //                l => l,
        //                f => f.Name,
        //                (l, f) => new CardLegality
        //                {
        //                    FormatId = f.Id
        //                }).ToList(),

        //    });//.ToList();

        //    await _cardContext.AddRangeAsync(cardsToAdd);
        //    await _cardContext.SaveChangesAsync();
        //}


        //private async Task UpdateCardDefinition(CardDataDto scryfallCard)
        //{
        //    var cardToUpdate = _cardContext.Cards.FirstOrDefault(x => x.Id == scryfallCard.MultiverseId);

        //    if (cardToUpdate == null)
        //    {
        //        throw new Exception("Could not find the card to update");
        //    }

        //    //So all of the things I have to update don't actually exist on the Cards table

        //    //I need to possibly add/remove legalities, and I need to update the price on variants


        //    //scryfallCard.Variants.ForEach(v =>
        //    //{



        //    //});






        //    var existingVariants = _cardContext.CardVariants
        //        .Where(x => x.CardId == cardToUpdate.Id).Include(x => x.Type);


        //    await existingVariants.ForEachAsync(v =>
        //    {

        //        string variantName = v.Type.Name;

        //        //var matchingType = [card].[variants].Where(name matches)
        //        var matchingVariant = scryfallCard.Variants.FirstOrDefault(x => x.Name == variantName);

        //        if(matchingVariant != null)
        //        {
        //            v.Price = matchingVariant.Price;
        //            v.PriceFoil = matchingVariant.PriceFoil;



        //        }
        //        else
        //        {
        //            _logger.LogWarning($"Variant in DB was not found on scryfall card. Card: {scryfallCard.Name} Set: {scryfallCard.Set} Variant: {variantName}");
        //        }

        //        //if (scryfallCard.Prices.ContainsKey(variantName))
        //        //    v.Price = scryfallCard.Prices[variantName];

        //        //if (scryfallCard.Prices.ContainsKey($"{variantName}_foil"))
        //        //    v.PriceFoil = scryfallCard.Prices[$"{variantName}_foil"];

        //    });

        //    //what happens if an existing variant doesn't exist on the new card?

        //    //What happens if a variant exists on the new card but not in the DB?




        //    _cardContext.CardVariants.UpdateRange(existingVariants);


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
        //    var existingLegalitiesToDelete = allExistingLegalities.Where(x => !scryfallCard.Legalities.Contains(x.Format.Name));

        //    var legalityStringsToKeep = allExistingLegalities.Where(x => scryfallCard.Legalities.Contains(x.Format.Name)).Select(x => x.Format.Name);

        //    var legalitiesToAdd = scryfallCard.Legalities
        //        .Where(x => !legalityStringsToKeep.Contains(x))
        //        .Select(x => new CardLegality()
        //        {
        //            CardId = cardToUpdate.Id,
        //            Format = _cardContext.MagicFormats.Where(f => f.Name == x).FirstOrDefault(),
        //        })
        //        .Where(x => x.Format != null)
        //        .ToList();

        //    //var something = scryfallCard.Legalities
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

        ////public IQueryable<LegacyDataContext.Card> QueryCardDefinitions()
        ////{
        ////    var query = _cardContext.Cards.AsQueryable();
        ////    return query;
        ////}

        #endregion
    }
}
