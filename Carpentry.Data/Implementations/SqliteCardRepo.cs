using Carpentry.Data.LegacyDataContext;
using Carpentry.Data.Interfaces;
using Carpentry.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Carpentry.Data.QueryParameters;
using Carpentry.Data.LegacyModels;

namespace Carpentry.Data.Implementations
{
    public class SqliteCardRepo : ILegacyCardRepo
    {

        //readonly ScryfallDataContext _scryContext;
        readonly SqliteDataContext _cardContext;
        private readonly ILogger<SqliteCardRepo> _logger;

        private const bool _obsoleteShouldThrowError = false;


        public SqliteCardRepo(SqliteDataContext cardContext, ILogger<SqliteCardRepo> logger)
        {
            _cardContext = cardContext;
            _logger = logger;
        }

        public async Task<CardSet> GetSetByCode(string setCode)
        {
            if(setCode == null)
            {
                return null;
            }

            CardSet setResult = await _cardContext.Sets.FirstOrDefaultAsync(x => x.Code.ToLower() == setCode.ToLower());

            return setResult;
        }

        #region Legacy

        #region Card related methdos

        public async Task AddCardDefinition(ScryfallMagicCard scryfallCard)
        {
            //ScryfallMagicCard scryfallCard = JsonConvert.DeserializeObject<ScryfallMagicCard>(scryfallDBCard.StringData);

            //besides just adding the card, also need to add

            //ColorIdentities
            var scryfallCardColorIdentities = scryfallCard.ColorIdentity ?? new List<string>();
            var relevantColorIdentityManaTypes = _cardContext.ManaTypes.Where(x => scryfallCardColorIdentities.Contains(x.Id.ToString())).ToList();

            //CardColors,
            var scryfallCardColors = scryfallCard.Colors ?? new List<string>();
            var relevantColorManaTypes = _cardContext.ManaTypes.Where(x => scryfallCardColors.Contains(x.Id.ToString())).ToList();


            //CardLegalities, 
            var scryCardLegalities = scryfallCard.Legalities.ToList();

            //NOTE - This ends up ignoring any formats that don't actually exist in the DB
            //as of 11-16-2019, this is the desired effect
            var relevantDbFormats = _cardContext.MagicFormats.Where(x => scryCardLegalities.Contains(x.Name)).ToList();

            //CardVariants
            var scryCardVariants = scryfallCard.Variants.Keys.ToList();
            var relevantDBVariantTypes = _cardContext.VariantTypes.Where(x => scryCardVariants.Contains(x.Name)).ToList();


            var dbSet = _cardContext.Sets.FirstOrDefault(x => x.Code == scryfallCard.Set);
            if (dbSet == null)
            {
                CardSet newSet = new CardSet()
                {
                    Code = scryfallCard.Set,
                    Name = scryfallCard.Set,
                };
                _cardContext.Sets.Add(newSet);
                dbSet = newSet;
            }

            char rarity;
            switch (scryfallCard.Rarity)
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

            var newCard = new LegacyDataContext.Card
            {
                Id = scryfallCard.MultiverseId,
                Cmc = scryfallCard.Cmc,
                ManaCost = scryfallCard.ManaCost,
                Name = scryfallCard.Name,
                Text = scryfallCard.Text,
                Type = scryfallCard.Type,

                //Set
                Set = dbSet, //Should this be the ID instead?

                //Rarity
                RarityId = rarity,

                //Color
                //jank
                CardColors = relevantColorManaTypes.Select(x => new CardColor
                {
                    ManaType = x,
                }).ToList(),


                //Color Identity
                CardColorIdentities = relevantColorIdentityManaTypes.Select(x => new LegacyDataContext.CardColorIdentity
                {
                    ManaType = x,
                }).ToList(),

                //Variants
                //still janky
                Variants = relevantDBVariantTypes.Select(x => new CardVariant
                {
                    ImageUrl = scryfallCard.Variants[x.Name],
                    Price = scryfallCard.Prices[x.Name],
                    PriceFoil = scryfallCard.Prices[$"{x.Name}_foil"],
                    Type = x,
                }).ToList(),

                //Legalities
                //This feels janky
                Legalities = relevantDbFormats.Select(x => new CardLegality { Format = x }).ToList(),

            };

            await _cardContext.Cards.AddAsync(newCard);
            await _cardContext.SaveChangesAsync();
        }


        public async Task AddCardDefinitionBatch(List<ScryfallMagicCard> cardBatch)
        {
            //lets try some nonsense
            IEnumerable<LegacyDataContext.Card> cardsToAdd = cardBatch.Select(scryfallCard => new LegacyDataContext.Card
            {
                Id = scryfallCard.MultiverseId,
                Cmc = scryfallCard.Cmc,
                ManaCost = scryfallCard.ManaCost,
                Name = scryfallCard.Name,
                Text = scryfallCard.Text,
                Type = scryfallCard.Type,

                Set = _cardContext.Sets.FirstOrDefault(x => x.Code == scryfallCard.Set),

                RarityId = GetRarityCode(scryfallCard.Rarity),


                CardColors = scryfallCard.Colors
                    .Join(
                        _cardContext.ManaTypes,
                        color => color,
                        mt => mt.Id.ToString(),
                        (color, mt) => new CardColor { ManaTypeId = mt.Id }).ToList(),

                CardColorIdentities = scryfallCard.ColorIdentity
                    .Join(
                        _cardContext.ManaTypes,
                        color => color,
                        mt => mt.Id.ToString(),
                        (color, mt) => new CardColorIdentity { ManaTypeId = mt.Id }).ToList(),

                Variants = scryfallCard.Variants
                    .Join(
                        _cardContext.VariantTypes,
                        v => v.Key,
                        vt => vt.Name,
                        (v, vt) => new CardVariant
                        {
                            ImageUrl = v.Value,
                            Price = scryfallCard.Prices[v.Key],
                            PriceFoil = scryfallCard.Prices[$"{v.Key}_foil"],
                            CardVariantTypeId = vt.Id,
                        }).ToList(),

                Legalities = scryfallCard.Legalities
                    .Join(
                        _cardContext.MagicFormats,
                        l => l,
                        f => f.Name,
                        (l, f) => new CardLegality
                        {
                            FormatId = f.Id
                        }).ToList(),

            });//.ToList();

            await _cardContext.AddRangeAsync(cardsToAdd);
            await _cardContext.SaveChangesAsync();
        }


        public async Task UpdateCardDefinition(ScryfallMagicCard scryfallCard)
        {
            var cardToUpdate = _cardContext.Cards.FirstOrDefault(x => x.Id == scryfallCard.MultiverseId);

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

                if(scryfallCard.Prices.ContainsKey(variantName))
                    v.Price = scryfallCard.Prices[variantName];

                if (scryfallCard.Prices.ContainsKey($"{variantName}_foil"))
                    v.PriceFoil = scryfallCard.Prices[$"{variantName}_foil"];

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
            var existingLegalitiesToDelete = allExistingLegalities.Where(x => !scryfallCard.Legalities.Contains(x.Format.Name));

            var legalityStringsToKeep = allExistingLegalities.Where(x => scryfallCard.Legalities.Contains(x.Format.Name)).Select(x => x.Format.Name);

            var legalitiesToAdd = scryfallCard.Legalities
                .Where(x => !legalityStringsToKeep.Contains(x))
                .Select(x => new CardLegality()
                {
                    CardId = cardToUpdate.Id,
                    Format = _cardContext.MagicFormats.Where(f => f.Name == x).FirstOrDefault(),
                })
                .Where(x => x.Format != null)
                .ToList();

            var something = scryfallCard.Legalities
                .Where(x => !legalityStringsToKeep.Contains(x))
                .Select(x => new 
                {
                    x,
                    CardId = cardToUpdate.Id,
                    Format = _cardContext.MagicFormats.Where(f => f.Name == x).FirstOrDefault(),
                }).ToList();

            if (existingLegalitiesToDelete.Any())
                _cardContext.CardLegalities.RemoveRange(existingLegalitiesToDelete);
            if(legalitiesToAdd.Any())
                _cardContext.CardLegalities.AddRange(legalitiesToAdd);
            await _cardContext.SaveChangesAsync();

        }

        public IQueryable<LegacyDataContext.Card> QueryCardDefinitions()
        {
            var query = _cardContext.Cards.AsQueryable();
            return query;
        }

        #endregion

        #region Deck related methods

        public async Task<MagicFormat> GetFormatByName(string formatName)
        {
            MagicFormat format = await _cardContext.MagicFormats.Where(x => x.Name.ToLower() == formatName.ToLower()).FirstAsync();
            return format;
        }



        /// <summary>
        /// Adds a new deck
        /// </summary>
        /// <param name="props"></param>
        /// <returns></returns>
        [Obsolete("Deprecated method, use the one that takes a DataContext.Deck", _obsoleteShouldThrowError)]
        public async Task<int> AddDeck(DeckProperties props)
        {

            //TODO: Props should hold a format ID, not a format name
            //var deckFormat = _cardContext.MagicFormats.Where(x => x.Name.ToLower() == props.Format.ToLower()).First();
            var deckFormat = await GetFormatByName(props.Format);

            Deck newDeck = new Deck()
            {
                Name = props.Name,
                Format = deckFormat,
                Notes = props.Notes,

                BasicW = props.BasicW,
                BasicU = props.BasicU,
                BasicB = props.BasicB,
                BasicR = props.BasicR,
                BasicG = props.BasicG,
            };

            await _cardContext.Decks.AddAsync(newDeck);
            await _cardContext.SaveChangesAsync();

            return newDeck.Id;
        }

        public async Task<int> AddDeck(Deck newDeck)
        {
            if(newDeck.Id > 0)
            {
                throw new ArgumentException("New deck cannot contain an ID");
            }

            //TODO - consider more validation

            await _cardContext.Decks.AddAsync(newDeck);
            await _cardContext.SaveChangesAsync();

            return newDeck.Id;
        }

        /// <summary>
        /// This method just updates the Deck data record, it does not modify the contents of a deck
        /// </summary>
        /// <param name="deckDto">The updated Deck properties</param>
        /// <returns></returns>
        public async Task UpdateDeck(DeckProperties deckDto)
        {
            Deck existingDeck = _cardContext.Decks.Where(x => x.Id == deckDto.Id).FirstOrDefault();

            if (existingDeck == null)
            {
                throw new Exception("No deck found matching the specified ID");
            }

            var deckFormat = _cardContext.MagicFormats.Where(x => x.Name.ToLower() == deckDto.Format.ToLower()).First();

            //TODO: Deck Properties should just hold a format ID instead of the string version of a format

            existingDeck.Name = deckDto.Name;
            existingDeck.Format = deckFormat; // deckDto.Format;
            existingDeck.Notes = deckDto.Notes;
            existingDeck.BasicW = deckDto.BasicW;
            existingDeck.BasicU = deckDto.BasicU;
            existingDeck.BasicB = deckDto.BasicB;
            existingDeck.BasicR = deckDto.BasicR;
            existingDeck.BasicG = deckDto.BasicG;

            _cardContext.Decks.Update(existingDeck);
            await _cardContext.SaveChangesAsync();
        }

        //In this latest version, when a deck is deleted, all related deck cards should also be deleted
        //Since a deck card just has an association with an inventory card, we can safely delete the deck cards leaving the inventory items intact
        public async Task DeleteDeck(int deckId)
        {
            var deckCardsToDelete = _cardContext.DeckCards.Where(x => x.DeckId == deckId).ToList();
            if (deckCardsToDelete.Any())
            {
                _cardContext.DeckCards.RemoveRange(deckCardsToDelete);
            }

            var deckToDelete = _cardContext.Decks.Where(x => x.Id == deckId).FirstOrDefault();
            _cardContext.Decks.Remove(deckToDelete);

            await _cardContext.SaveChangesAsync();
        }

        public async Task AddDeckCard(DeckCardDto dto)
        {
            if (dto.DeckId == 0 || dto.InventoryCard == null || dto.InventoryCard.Id == 0)
            {
                throw new ArgumentNullException("Invalid deck card dto");
            }

            DeckCard newDeckCard = new DeckCard()
            {
                DeckId = dto.DeckId,
                InventoryCardId = dto.InventoryCard.Id,
            };

            await _cardContext.DeckCards.AddAsync(newDeckCard);
            await _cardContext.SaveChangesAsync();
        }

        //public async Task UpdateDeckCard(DeckCardDto dto)
        public async Task UpdateDeckCard(int deckCardId, int deckId, char? categoryId)
        {
            var sourceCard = await _cardContext.DeckCards.Where(x => x.Id == deckCardId).FirstOrDefaultAsync();

            if (sourceCard == null)
            {
                throw new ArgumentNullException();
            }

            sourceCard.DeckId = deckId;
            
            sourceCard.CategoryId = categoryId;
            
            _cardContext.DeckCards.Update(sourceCard);

            await _cardContext.SaveChangesAsync();
        }

        //Deletes a deck card, does not delete the associated inventory card
        public async Task DeleteDeckCard(int deckCardId)
        {
            var cardToRemove = _cardContext.DeckCards.Where(x => x.Id == deckCardId).FirstOrDefault();
            if (cardToRemove != null)
            {
                _cardContext.DeckCards.Remove(cardToRemove);
                await _cardContext.SaveChangesAsync();
            }
            else
            {
                throw new Exception($"Could not find deck card wit ID {deckCardId}");
            }
        }


        public IQueryable<DeckProperties> QueryDeckProperties()
        {
            IQueryable<DeckProperties> query = _cardContext.Decks
                .Select(d => new DeckProperties()
                {
                    Id = d.Id,
                    Name = d.Name,
                    Notes = d.Notes,
                    Format = d.Format.Name,
                    BasicW = d.BasicW,
                    BasicU = d.BasicU,
                    BasicB = d.BasicB,
                    BasicR = d.BasicR,
                    BasicG = d.BasicG,
                });

            return query;
        }

        public IQueryable<DeckCard> QueryDeckCards()
        {
            IQueryable<DeckCard> query = _cardContext.DeckCards.AsQueryable();
            return query;
        }

        public IQueryable<InventoryCard> QueryInventoryCardsForDeck(int deckId)
        {
            IQueryable<InventoryCard> query = _cardContext.DeckCards
                .Where(x => x.DeckId == deckId)
                .Select(x => x.InventoryCard)
                .Include(x => x.Card.Variants);

            return query;
        }

        public IQueryable<InventoryCard> QueryInventoryCards()
        {
            IQueryable<InventoryCard> query = _cardContext.InventoryCards
                .Include(x => x.Card.Variants).AsQueryable();

            return query;
        }




        #endregion

        #region Inventory related methods

        /// <summary>
        /// Adds a new card to the inventory
        /// Does not handle adding deck cards
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<int> AddInventoryCard(InventoryCardDto dto)
        {
            var cardVariant = await _cardContext.VariantTypes.FirstOrDefaultAsync(x => x.Name == dto.VariantType);

            InventoryCard newInventoryCard = new InventoryCard()
            {
                IsFoil = dto.IsFoil,
                InventoryCardStatusId = dto.InventoryCardStatusId,
                MultiverseId = dto.MultiverseId,
                VariantType = cardVariant,
            };

            await _cardContext.InventoryCards.AddAsync(newInventoryCard);
            await _cardContext.SaveChangesAsync();

            return newInventoryCard.Id;
        }

        public async Task AddInventoryCardBatch(IEnumerable<InventoryCardDto> dtoBatch)
        {
            //assuming all card definitions exist already
            var newCards = dtoBatch.Select(x => new InventoryCard()
            {
                IsFoil = x.IsFoil,
                InventoryCardStatusId = x.InventoryCardStatusId,
                MultiverseId = x.MultiverseId,
                VariantType = _cardContext.VariantTypes.FirstOrDefault(v => v.Name == x.VariantType),

            }).ToList();

            await _cardContext.InventoryCards.AddRangeAsync(newCards);
            await _cardContext.SaveChangesAsync();
        }


        /// <summary>
        /// Updates an inventory card
        /// In theory, the only fieds I'd practically want to update would be Status and maybe IsFoil??
        /// This one might need to wait until variants are handled better...
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task UpdateInventoryCard(InventoryCardDto dto)
        {
            var dbCard = _cardContext.InventoryCards.FirstOrDefault(x => x.Id == dto.Id);
            //currently only expecting to change the status with this method
            dbCard.InventoryCardStatusId = dto.InventoryCardStatusId;
            _cardContext.InventoryCards.Update(dbCard);
            await _cardContext.SaveChangesAsync();
        }

        /// <summary>
        /// Deletes a card from the inventory
        /// Can only delete cards that don't belong to a deck
        /// </summary>
        /// <param name="id">Id of card to delete</param>
        /// <returns></returns>
        public async Task DeleteInventoryCard(int id)
        {
            var deckCardsReferencingThisCard = _cardContext.DeckCards.Where(x => x.DeckId == id).Count();

            if (deckCardsReferencingThisCard > 0)
            {
                throw new Exception("Cannot delete a card that's currently in a deck");
            }

            var cardToRemove = _cardContext.InventoryCards.First(x => x.Id == id);

            _cardContext.InventoryCards.Remove(cardToRemove);

            await _cardContext.SaveChangesAsync();
        }

        public async Task<IQueryable<LegacyDataContext.Card>> QueryFilteredCards(InventoryQueryParameter filters)
        {
            var cardsQuery = _cardContext.Cards.AsQueryable();

            if (!string.IsNullOrEmpty(filters.Set))
            {
                var matchingSetId = _cardContext.Sets.Where(x => x.Code.ToLower() == filters.Set.ToLower()).Select(x => x.Id).FirstOrDefault();
                cardsQuery = cardsQuery.Where(x => x.SetId == matchingSetId);
            }

            if (filters.StatusId > 0)
            {
                //cardsQuery = cardsQuery.Where(x => x.)
            }

            if (filters.Colors.Any())
            {
                //var allowedColorIDs = filters.Colors.

                var excludedColors = await _cardContext.ManaTypes.Where(x => !filters.Colors.Contains(x.Id.ToString())).Select(x => x.Id).ToListAsync();

                //var includedColors = filters.Colors;

                //only want cards where every color is an included color
                //cardsQuery = cardsQuery.Where(x => !x.CardColorIdentities.Any() || x.CardColorIdentities.Any(color => includedColors.Contains(color.ManaTypeId.ToString())));

                //alternative query, no excluded colors
                cardsQuery = cardsQuery.Where(x => !(x.CardColorIdentities.Any(color => excludedColors.Contains(color.ManaTypeId))));

            }

            if (!string.IsNullOrEmpty(filters.Format))
            {
                //var matchingLegality = _cardContext.MagicFormats.Where(x => x.Name.ToLower() == param.Format.ToLower()).FirstOrDefault();
                var matchingFormatId = await GetFormatIdByName(filters.Format);
                cardsQuery = cardsQuery.Where(x => x.Legalities.Where(l => l.FormatId == matchingFormatId).Any());
            }

            if (filters.ExclusiveColorFilters)
            {
                cardsQuery = cardsQuery.Where(x => x.CardColorIdentities.Count() == filters.Colors.Count());
            }

            if (filters.MultiColorOnly)
            {
                cardsQuery = cardsQuery.Where(x => x.CardColorIdentities.Count() > 1);
            }

            if (!string.IsNullOrEmpty(filters.Type))
            {
                cardsQuery = cardsQuery.Where(x => x.Type.Contains(filters.Type));
            }

            if (filters.Rarity.Any())
            {
                cardsQuery = cardsQuery.Where(x => filters.Rarity.Contains(x.Rarity.Name.ToLower()));

            }

            if (!string.IsNullOrEmpty(filters.Text))
            {
                cardsQuery = cardsQuery.Where(x =>
                    x.Text.ToLower().Contains(filters.Text.ToLower())
                    ||
                    x.Name.ToLower().Contains(filters.Text.ToLower())
                    ||
                    x.Type.ToLower().Contains(filters.Text.ToLower())
                );
            }

            return cardsQuery;
        }

        public async Task<IQueryable<InventoryOverviewDto>> QueryInventoryOverviews(InventoryQueryParameter filters)
        {
            var cardsQuery = await QueryFilteredCards(filters);

            var query = cardsQuery.Select(x => new
            {
                MultiverseId = x.Id,
                x.Name,
                x.Type,
                x.ManaCost,
                Counts = x.InventoryCards.Where(c => c.InventoryCardStatusId == 1).Count(),
                x.Variants.First().ImageUrl,
                x.Cmc,
            }).GroupBy(x => x.Name)
            .Select(x => new InventoryOverviewDto
            {
                Name = x.Key,
                Type = x.First().Type,
                Cost = x.First().ManaCost,
                Img = x.First().ImageUrl,
                Count = x.Sum(card => card.Counts),
                Cmc = x.First().Cmc,
            });

            if (filters.MinCount > 0)
            {
                query = query.Where(x => x.Count >= filters.MinCount);
            }

            if (filters.MaxCount > 0)
            {
                query = query.Where(x => x.Count <= filters.MinCount);
            }

            if (filters.Sort == "count")
            {
                query = query.OrderByDescending(x => x.Count);
            }
            else if (filters.Sort == "name")
            {
                query = query.OrderBy(x => x.Name);
            }
            else if (filters.Sort == "cmc")
            {
                query = query.OrderBy(x => x.Cmc)
                    .ThenBy(x => x.Name);
            }
            else
            {
                query = query.OrderByDescending(x => x.Count);
            }

            return query;
        }

        #endregion

        #region Private Reference lookups


        private static char GetRarityCode(string name)
        {
            char rarity;
            switch (name)
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


        private async Task<int> GetFormatIdByName(string formatName)
        {
            var format = await _cardContext.MagicFormats.Where(x => x.Name.ToLower() == formatName.ToLower()).FirstOrDefaultAsync();
            if (format == null)
            {
                throw new Exception($"Could not find format matching name: {formatName}");
            }
            return format.Id;
        }

        private static FilterDescriptor MakeFilterDescriptor(string name)
        {
            return new FilterDescriptor()
            {
                Name = name,
                Value = name.ToLower(),
            };
        }

        #endregion

        #region Core methods

        public IQueryable<FilterDescriptor> QuerySetFilters()
        {
            IQueryable<FilterDescriptor> query = _cardContext.Sets
                .OrderByDescending(x => x.ReleaseDate)
                .Select(x => new FilterDescriptor()
                {
                    Name = x.Name,
                    //Value = x.Code,
                    Value = x.Id.ToString(),
                });

            return query;
        }

        public IQueryable<FilterDescriptor> QueryTypeFilters()
        {
            List<FilterDescriptor> setFilters = new List<FilterDescriptor>()
            {
                MakeFilterDescriptor("Creature"),
                MakeFilterDescriptor("Instant"),
                MakeFilterDescriptor("Sorcery"),
                MakeFilterDescriptor("Enchantment"),
                MakeFilterDescriptor("Land"),
                MakeFilterDescriptor("Planeswalker"),
                MakeFilterDescriptor("Artifact"),
                MakeFilterDescriptor("Legendary"),
            };

            return setFilters.AsQueryable();
        }

        public IQueryable<FilterDescriptor> QueryFormatFilters()
        {
            IQueryable<FilterDescriptor> query = _cardContext.MagicFormats
                .Select(x => new FilterDescriptor()
                {
                    Name = x.Name,
                    Value = x.Id.ToString(),
                });
            return query;
        }

        public IQueryable<FilterDescriptor> QueryManaColorFilters()
        {
            //TODO - Find a way to make sure this is sorted in the expected order
            IQueryable<FilterDescriptor> query = _cardContext.ManaTypes
                .Select(x => new FilterDescriptor()
                {
                    Name = x.Name,
                    Value = x.Id.ToString(),
                });
            return query;
        }

        public IQueryable<FilterDescriptor> QueryRarityFilters()
        {
            IQueryable<FilterDescriptor> query = _cardContext.Rarities
                .Select(x => new FilterDescriptor()
                {
                    Name = x.Name,
                    Value = x.Id.ToString(),
                });
            return query;
        }

        public IQueryable<FilterDescriptor> QueryCardStatusFilters()
        {
            IQueryable<FilterDescriptor> query = _cardContext.CardStatuses
                .Select(x => new FilterDescriptor()
                {
                    Name = x.Name,
                    Value = x.Id.ToString(),
                });
            return query;
        }

        #endregion

        #endregion
    }

    public class OtherSqliteCardRepo //: ICardRepo
    {
        readonly SqliteDataContext _cardContext;
        private readonly ILogger<OtherSqliteCardRepo> _logger;

        public OtherSqliteCardRepo(
            SqliteDataContext cardContext, 
            ILogger<OtherSqliteCardRepo> logger
            )
        {
            _cardContext = cardContext;
            _logger = logger;
        }

        #region Card related methdos

        public async Task AddCardDefinition(ScryfallMagicCard scryfallCard)
        {
            //ScryfallMagicCard scryfallCard = JsonConvert.DeserializeObject<ScryfallMagicCard>(scryfallDBCard.StringData);

            //besides just adding the card, also need to add

            //ColorIdentities
            var scryfallCardColorIdentities = scryfallCard.ColorIdentity ?? new List<string>();
            var relevantColorIdentityManaTypes = _cardContext.ManaTypes.Where(x => scryfallCardColorIdentities.Contains(x.Id.ToString())).ToList();

            //CardColors,
            var scryfallCardColors = scryfallCard.Colors ?? new List<string>();
            var relevantColorManaTypes = _cardContext.ManaTypes.Where(x => scryfallCardColors.Contains(x.Id.ToString())).ToList();


            //CardLegalities, 
            var scryCardLegalities = scryfallCard.Legalities.ToList();

            //NOTE - This ends up ignoring any formats that don't actually exist in the DB
            //as of 11-16-2019, this is the desired effect
            var relevantDbFormats = _cardContext.MagicFormats.Where(x => scryCardLegalities.Contains(x.Name)).ToList();

            //CardVariants
            var scryCardVariants = scryfallCard.Variants.Keys.ToList();
            var relevantDBVariantTypes = _cardContext.VariantTypes.Where(x => scryCardVariants.Contains(x.Name)).ToList();


            var dbSet = _cardContext.Sets.FirstOrDefault(x => x.Code == scryfallCard.Set);
            if (dbSet == null)
            {
                CardSet newSet = new CardSet()
                {
                    Code = scryfallCard.Set,
                    Name = scryfallCard.Set,
                };
                _cardContext.Sets.Add(newSet);
                dbSet = newSet;
            }

            char rarity;
            switch (scryfallCard.Rarity)
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

            var newCard = new LegacyDataContext.Card
            {
                Id = scryfallCard.MultiverseId,
                Cmc = scryfallCard.Cmc,
                ManaCost = scryfallCard.ManaCost,
                Name = scryfallCard.Name,
                Text = scryfallCard.Text,
                Type = scryfallCard.Type,

                //Set
                Set = dbSet, //Should this be the ID instead?

                //Rarity
                RarityId = rarity,

                //Color
                //jank
                CardColors = relevantColorManaTypes.Select(x => new CardColor
                {
                    ManaType = x,
                }).ToList(),


                //Color Identity
                CardColorIdentities = relevantColorIdentityManaTypes.Select(x => new LegacyDataContext.CardColorIdentity
                {
                    ManaType = x,
                }).ToList(),

                //Variants
                //still janky
                Variants = relevantDBVariantTypes.Select(x => new CardVariant
                {
                    ImageUrl = scryfallCard.Variants[x.Name],
                    Price = scryfallCard.Prices[x.Name],
                    PriceFoil = scryfallCard.Prices[$"{x.Name}_foil"],
                    Type = x,
                }).ToList(),

                //Legalities
                //This feels janky
                Legalities = relevantDbFormats.Select(x => new CardLegality { Format = x }).ToList(),

            };

            await _cardContext.Cards.AddAsync(newCard);
            await _cardContext.SaveChangesAsync();
        }

        public async Task UpdateCardDefinition(ScryfallMagicCard scryfallCard)
        {
            var cardToUpdate = _cardContext.Cards.FirstOrDefault(x => x.Id == scryfallCard.MultiverseId);

            if(cardToUpdate == null)
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
                v.Price = scryfallCard.Prices[variantName];
                v.PriceFoil = scryfallCard.Prices[$"{variantName}_foil"];
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
            var existingLegalitiesToDelete = allExistingLegalities.Where(x => !scryfallCard.Legalities.Contains(x.Format.Name));

            var legalityStringsToKeep = allExistingLegalities.Where(x => scryfallCard.Legalities.Contains(x.Format.Name)).Select(x => x.Format.Name);

            var legalitiesToAdd = scryfallCard.Legalities
                .Where(x => !legalityStringsToKeep.Contains(x))
                .Select(x => new CardLegality()
                {
                    CardId = cardToUpdate.Id,
                    Format = _cardContext.MagicFormats.Where(f => f.Name == x).FirstOrDefault(),
                }).ToList();


            _cardContext.CardLegalities.RemoveRange(existingLegalitiesToDelete);
            _cardContext.CardLegalities.AddRange(legalitiesToAdd);
            await _cardContext.SaveChangesAsync();

        }

        public IQueryable<LegacyDataContext.Card> QueryCardDefinitions()
        {
            var query = _cardContext.Cards.AsQueryable();
            return query;
        }
        
        #endregion

        #region Deck related methods

        /// <summary>
        /// Adds a new deck
        /// The only way (currently) to add a new deck is through a modal that doesn't show basic lands, so they are omitted
        /// </summary>
        /// <param name="props"></param>
        /// <returns></returns>
        public async Task<int> AddDeck(DeckProperties props)
        {

            //TODO: Props should hold a format ID, not a format name
            var deckFormat = _cardContext.MagicFormats.Where(x => x.Name.ToLower() == props.Format.ToLower()).First();

            Deck newDeck = new Deck()
            {
                Name = props.Name,
                Format = deckFormat,
                Notes = props.Notes,
            };

            await _cardContext.Decks.AddAsync(newDeck);
            await _cardContext.SaveChangesAsync();

            return newDeck.Id;
        }

        /// <summary>
        /// This method just updates the Deck data record, it does not modify the contents of a deck
        /// </summary>
        /// <param name="deckDto">The updated Deck properties</param>
        /// <returns></returns>
        public async Task UpdateDeck(DeckProperties deckDto)
        {
            Deck existingDeck = _cardContext.Decks.Where(x => x.Id == deckDto.Id).FirstOrDefault();

            if (existingDeck == null)
            {
                throw new Exception("No deck found matching the specified ID");
            }

            var deckFormat = _cardContext.MagicFormats.Where(x => x.Name.ToLower() == deckDto.Format.ToLower()).First();

            //TODO: Deck Properties should just hold a format ID instead of the string version of a format

            existingDeck.Name = deckDto.Name;
            existingDeck.Format = deckFormat; // deckDto.Format;
            existingDeck.Notes = deckDto.Notes;
            existingDeck.BasicW = deckDto.BasicW;
            existingDeck.BasicU = deckDto.BasicU;
            existingDeck.BasicB = deckDto.BasicB;
            existingDeck.BasicR = deckDto.BasicR;
            existingDeck.BasicG = deckDto.BasicG;

            _cardContext.Decks.Update(existingDeck);
            await _cardContext.SaveChangesAsync();
        }

        //In this latest version, when a deck is deleted, all related deck cards should also be deleted
        //Since a deck card just has an association with an inventory card, we can safely delete the deck cards leaving the inventory items intact
        public async Task DeleteDeck(int deckId)
        {
            var deckCardsToDelete = _cardContext.DeckCards.Where(x => x.DeckId == deckId).ToList();
            if (deckCardsToDelete.Any())
            {
                _cardContext.DeckCards.RemoveRange(deckCardsToDelete);
            }

            var deckToDelete = _cardContext.Decks.Where(x => x.Id == deckId).FirstOrDefault();
            _cardContext.Decks.Remove(deckToDelete);

            await _cardContext.SaveChangesAsync();
        }

        public async Task AddDeckCard(DeckCardDto dto)
        {
            if (dto.DeckId == 0 || dto.InventoryCard == null || dto.InventoryCard.Id == 0)
            {
                throw new ArgumentNullException("Invalid deck card dto");
            }

            DeckCard newDeckCard = new DeckCard()
            {
                DeckId = dto.DeckId,
                InventoryCardId = dto.InventoryCard.Id,
            };

            await _cardContext.DeckCards.AddAsync(newDeckCard);
            await _cardContext.SaveChangesAsync();
        }

        //public async Task UpdateDeckCard(DeckCardDto dto)
        public async Task UpdateDeckCard(int deckCardId, int deckId)
        {
            var sourceCard = await _cardContext.DeckCards.Where(x => x.Id == deckCardId).FirstOrDefaultAsync();

            if (sourceCard == null)
            {
                throw new ArgumentNullException();
            }

            sourceCard.DeckId = deckId;

            _cardContext.DeckCards.Update(sourceCard);

            await _cardContext.SaveChangesAsync();
        }

        //Deletes a deck card, does not delete the associated inventory card
        public async Task DeleteDeckCard(int deckCardId)
        {
            var cardToRemove = _cardContext.DeckCards.Where(x => x.Id == deckCardId).FirstOrDefault();
            if (cardToRemove != null)
            {
                _cardContext.DeckCards.Remove(cardToRemove);
                await _cardContext.SaveChangesAsync();
            }
            else
            {
                throw new Exception($"Could not find deck card wit ID {deckCardId}");
            }
        }


        public IQueryable<DeckProperties> QueryDeckProperties()
        {
            IQueryable<DeckProperties> query = _cardContext.Decks
                .Select(d => new DeckProperties()
                {
                    Id = d.Id,
                    Name = d.Name,
                    Notes = d.Notes,
                    Format = d.Format.Name,
                    BasicW = d.BasicW,
                    BasicU = d.BasicU,
                    BasicB = d.BasicB,
                    BasicR = d.BasicR,
                    BasicG = d.BasicG,
                });

            return query;
        }

        public IQueryable<DeckCard> QueryDeckCards()
        {
            IQueryable<DeckCard> query = _cardContext.DeckCards.AsQueryable();
            return query;
        }

        public IQueryable<InventoryCard> QueryInventoryCardsForDeck(int deckId)
        {
            IQueryable<InventoryCard> query = _cardContext.DeckCards
                .Where(x => x.DeckId == deckId)
                .Select(x => x.InventoryCard)
                .Include(x => x.Card.Variants);

            return query;
        }



        
        

        #endregion

        #region Inventory related methods

        /// <summary>
        /// Adds a new card to the inventory
        /// Does not handle adding deck cards
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<int> AddInventoryCard(InventoryCardDto dto)
        {
            var cardVariant = await _cardContext.VariantTypes.FirstOrDefaultAsync(x => x.Name == dto.VariantType);

            InventoryCard newInventoryCard = new InventoryCard()
            {
                IsFoil = dto.IsFoil,
                InventoryCardStatusId = dto.InventoryCardStatusId,
                MultiverseId = dto.MultiverseId,
                VariantType = cardVariant,
            };

            await _cardContext.InventoryCards.AddAsync(newInventoryCard);
            await _cardContext.SaveChangesAsync();

            return newInventoryCard.Id;
        }

        public async Task AddInventoryCardBatch(IEnumerable<InventoryCardDto> dtoBatch)
        {
            //assuming all card definitions exist already
            var newCards = dtoBatch.Select(x => new InventoryCard()
            {
                IsFoil = x.IsFoil,
                InventoryCardStatusId = x.InventoryCardStatusId,
                MultiverseId = x.MultiverseId,
                VariantType = _cardContext.VariantTypes.FirstOrDefault(v => v.Name == x.VariantType),

            }).ToList();

            await _cardContext.InventoryCards.AddRangeAsync(newCards);
            await _cardContext.SaveChangesAsync();
        }


        /// <summary>
        /// Updates an inventory card
        /// In theory, the only fieds I'd practically want to update would be Status and maybe IsFoil??
        /// This one might need to wait until variants are handled better...
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task UpdateInventoryCard(InventoryCardDto dto)
        {
            var dbCard = _cardContext.InventoryCards.FirstOrDefault(x => x.Id == dto.Id);
            //currently only expecting to change the status with this method
            dbCard.InventoryCardStatusId = dto.InventoryCardStatusId;
            _cardContext.InventoryCards.Update(dbCard);
            await _cardContext.SaveChangesAsync();
        }

        /// <summary>
        /// Deletes a card from the inventory
        /// Can only delete cards that don't belong to a deck
        /// </summary>
        /// <param name="id">Id of card to delete</param>
        /// <returns></returns>
        public async Task DeleteInventoryCard(int id)
        {
            var deckCardsReferencingThisCard = _cardContext.DeckCards.Where(x => x.DeckId == id).Count();

            if (deckCardsReferencingThisCard > 0)
            {
                throw new Exception("Cannot delete a card that's currently in a deck");
            }

            var cardToRemove = _cardContext.InventoryCards.First(x => x.Id == id);

            _cardContext.InventoryCards.Remove(cardToRemove);

            await _cardContext.SaveChangesAsync();
        }

        public async Task<IQueryable<LegacyDataContext.Card>> QueryFilteredCards(InventoryQueryParameter filters)
        {
            var cardsQuery = _cardContext.Cards.AsQueryable();

            if (!string.IsNullOrEmpty(filters.Set))
            {
                var matchingSetId = _cardContext.Sets.Where(x => x.Code.ToLower() == filters.Set.ToLower()).Select(x => x.Id).FirstOrDefault();
                cardsQuery = cardsQuery.Where(x => x.SetId == matchingSetId);
            }

            if (filters.StatusId > 0)
            {
                //cardsQuery = cardsQuery.Where(x => x.)
            }

            if (filters.Colors.Any())
            {
                //var allowedColorIDs = filters.Colors.

                var excludedColors = await _cardContext.ManaTypes.Where(x => !filters.Colors.Contains(x.Id.ToString())).Select(x => x.Id).ToListAsync();

                //var includedColors = filters.Colors;

                //only want cards where every color is an included color
                //cardsQuery = cardsQuery.Where(x => !x.CardColorIdentities.Any() || x.CardColorIdentities.Any(color => includedColors.Contains(color.ManaTypeId.ToString())));

                //alternative query, no excluded colors
                cardsQuery = cardsQuery.Where(x => !(x.CardColorIdentities.Any(color => excludedColors.Contains(color.ManaTypeId))));

            }

            if (!string.IsNullOrEmpty(filters.Format))
            {
                //var matchingLegality = _cardContext.MagicFormats.Where(x => x.Name.ToLower() == param.Format.ToLower()).FirstOrDefault();
                var matchingFormatId = await GetFormatIdByName(filters.Format);
                cardsQuery = cardsQuery.Where(x => x.Legalities.Where(l => l.FormatId == matchingFormatId).Any());
            }

            if (filters.ExclusiveColorFilters)
            {
                cardsQuery = cardsQuery.Where(x => x.CardColorIdentities.Count() == filters.Colors.Count());
            }

            if (filters.MultiColorOnly)
            {
                cardsQuery = cardsQuery.Where(x => x.CardColorIdentities.Count() > 1);
            }

            if (!string.IsNullOrEmpty(filters.Type))
            {
                cardsQuery = cardsQuery.Where(x => x.Type.Contains(filters.Type));
            }

            if (filters.Rarity.DefaultIfEmpty().Any())
            {
                cardsQuery = cardsQuery.Where(x => filters.Rarity.Contains(x.Rarity.Name.ToLower()));

            }

            if (!string.IsNullOrEmpty(filters.Text))
            {
                cardsQuery = cardsQuery.Where(x =>
                    x.Text.ToLower().Contains(filters.Text.ToLower())
                    ||
                    x.Name.ToLower().Contains(filters.Text.ToLower())
                    ||
                    x.Type.ToLower().Contains(filters.Text.ToLower())
                );
            }

            return cardsQuery;
        }

        public async Task<IQueryable<InventoryOverviewDto>> QueryInventoryOverviews(InventoryQueryParameter filters)
        {
            var cardsQuery = await QueryFilteredCards(filters);

            var query = cardsQuery.Select(x => new
            {
                MultiverseId = x.Id,
                x.Name,
                x.Type,
                x.ManaCost,
                Counts = x.InventoryCards.Where(c => c.InventoryCardStatusId == 1).Count(),
                x.Variants.First().ImageUrl,
            }).GroupBy(x => x.Name)
            .Select(x => new InventoryOverviewDto
            {
                Name = x.Key,
                Type = x.First().Type,
                Cost = x.First().ManaCost,
                Img = x.First().ImageUrl,
                Count = x.Sum(card => card.Counts)
            });

            if (filters.MinCount > 0)
            {
                query = query.Where(x => x.Count >= filters.MinCount);
            }

            if (filters.MaxCount > 0)
            {
                query = query.Where(x => x.Count <= filters.MinCount);
            }

            return query;
        }

        #endregion

        #region Private Reference lookups

        private async Task<int> GetFormatIdByName(string formatName)
        {
            var format = await _cardContext.MagicFormats.Where(x => x.Name.ToLower() == formatName.ToLower()).FirstOrDefaultAsync();
            if (format == null)
            {
                throw new Exception($"Could not find format matching name: {formatName}");
            }
            return format.Id;
        }

        private static FilterDescriptor MakeFilterDescriptor(string name)
        {
            return new FilterDescriptor()
            {
                Name = name,
                Value = name.ToLower(),
            };
        }

        #endregion

        #region Core methods

        public IQueryable<FilterDescriptor> QuerySetFilters()
        {
            //TODO - Can't get this sorting by release until I can make a DB schema update
            IQueryable<FilterDescriptor> query = _cardContext.Sets
                .Select(x => new FilterDescriptor()
                {
                    Name = x.Name,
                    //Value = x.Code,
                    Value = x.Id.ToString(),
                });

            return query;
        }

        public IQueryable<FilterDescriptor> QueryTypeFilters()
        {
            List<FilterDescriptor> setFilters = new List<FilterDescriptor>()
            {
                MakeFilterDescriptor("Creature"),
                MakeFilterDescriptor("Instant"),
                MakeFilterDescriptor("Sorcery"),
                MakeFilterDescriptor("Enchantment"),
                MakeFilterDescriptor("Land"),
                MakeFilterDescriptor("Planeswalker"),
                MakeFilterDescriptor("Artifact"),
                MakeFilterDescriptor("Legendary"),
            };

            return setFilters.AsQueryable();
        }

        public IQueryable<FilterDescriptor> QueryFormatFilters()
        {
            IQueryable<FilterDescriptor> query = _cardContext.MagicFormats
                .Select(x => new FilterDescriptor()
                {
                    Name = x.Name,
                    Value = x.Id.ToString(),
                });
            return query;
        }

        public IQueryable<FilterDescriptor> QueryManaColorFilters()
        {
            //TODO - Find a way to make sure this is sorted in the expected order
            IQueryable<FilterDescriptor> query = _cardContext.ManaTypes
                .Select(x => new FilterDescriptor()
                {
                    Name = x.Name,
                    Value = x.Id.ToString(),
                });
            return query;
        }

        public IQueryable<FilterDescriptor> QueryRarityFilters()
        {
            IQueryable<FilterDescriptor> query = _cardContext.Rarities
                .Select(x => new FilterDescriptor()
                {
                    Name = x.Name,
                    Value = x.Id.ToString(),
                });
            return query;
        }

        public IQueryable<FilterDescriptor> QueryCardStatusFilters()
        {
            IQueryable<FilterDescriptor> query = _cardContext.CardStatuses
                .Select(x => new FilterDescriptor()
                {
                    Name = x.Name,
                    Value = x.Id.ToString(),
                });
            return query;
        }

        #endregion
    }
}

//namespace Sqlite.Implementations
//{
//    public class SqliteCardRepo : ICardRepo
//    {
//        readonly SqliteDataContext _cardContext;
//        private readonly ILogger<SqliteCardRepo> _logger;

//        public SqliteCardRepo(
//            SqliteDataContext cardContext, 
//            ILogger<SqliteCardRepo> logger
//            )
//        {
//            _cardContext = cardContext;
//            _logger = logger;
//        }

//        #region Card related methdos

//        public async Task AddCardDefinition(ScryfallMagicCard scryfallCard)
//        {
//            //ScryfallMagicCard scryfallCard = JsonConvert.DeserializeObject<ScryfallMagicCard>(scryfallDBCard.StringData);

//            //besides just adding the card, also need to add

//            //ColorIdentities
//            var scryfallCardColorIdentities = scryfallCard.ColorIdentity ?? new List<string>();
//            var relevantColorIdentityManaTypes = _cardContext.ManaTypes.Where(x => scryfallCardColorIdentities.Contains(x.Id.ToString())).ToList();

//            //CardColors,
//            var scryfallCardColors = scryfallCard.Colors ?? new List<string>();
//            var relevantColorManaTypes = _cardContext.ManaTypes.Where(x => scryfallCardColors.Contains(x.Id.ToString())).ToList();


//            //CardLegalities, 
//            var scryCardLegalities = scryfallCard.Legalities.ToList();

//            //NOTE - This ends up ignoring any formats that don't actually exist in the DB
//            //as of 11-16-2019, this is the desired effect
//            var relevantDbFormats = _cardContext.MagicFormats.Where(x => scryCardLegalities.Contains(x.Name)).ToList();

//            //CardVariants
//            var scryCardVariants = scryfallCard.Variants.Keys.ToList();
//            var relevantDBVariantTypes = _cardContext.VariantTypes.Where(x => scryCardVariants.Contains(x.Name)).ToList();


//            var dbSet = _cardContext.Sets.FirstOrDefault(x => x.Code == scryfallCard.Set);
//            if (dbSet == null)
//            {
//                CardSet newSet = new CardSet()
//                {
//                    Code = scryfallCard.Set,
//                    Name = scryfallCard.Set,
//                };
//                _cardContext.Sets.Add(newSet);
//                dbSet = newSet;
//            }

//            char rarity;
//            switch (scryfallCard.Rarity)
//            {
//                case "mythic":
//                    rarity = 'M';
//                    break;

//                case "rare":
//                    rarity = 'R';
//                    break;
//                case "uncommon":
//                    rarity = 'U';
//                    break;
//                case "common":
//                    rarity = 'C';
//                    break;
//                default:
//                    throw new Exception("Error reading scryfall rarity");

//            }

//            var newCard = new DataContext.Card
//            {
//                Id = scryfallCard.MultiverseId,
//                Cmc = scryfallCard.Cmc,
//                ManaCost = scryfallCard.ManaCost,
//                Name = scryfallCard.Name,
//                Text = scryfallCard.Text,
//                Type = scryfallCard.Type,

//                //Set
//                Set = dbSet, //Should this be the ID instead?

//                //Rarity
//                RarityId = rarity,

//                //Color
//                //jank
//                CardColors = relevantColorManaTypes.Select(x => new CardColor
//                {
//                    ManaType = x,
//                }).ToList(),


//                //Color Identity
//                CardColorIdentities = relevantColorIdentityManaTypes.Select(x => new DataContext.CardColorIdentity
//                {
//                    ManaType = x,
//                }).ToList(),

//                //Variants
//                //still janky
//                Variants = relevantDBVariantTypes.Select(x => new CardVariant
//                {
//                    ImageUrl = scryfallCard.Variants[x.Name],
//                    Price = scryfallCard.Prices[x.Name],
//                    PriceFoil = scryfallCard.Prices[$"{x.Name}_foil"],
//                    Type = x,
//                }).ToList(),

//                //Legalities
//                //This feels janky
//                Legalities = relevantDbFormats.Select(x => new CardLegality { Format = x }).ToList(),

//            };

//            await _cardContext.Cards.AddAsync(newCard);
//            await _cardContext.SaveChangesAsync();
//        }

//        private static char GetRarityCode(string name)
//        {
//            char rarity;
//            switch (name)
//            {
//                case "mythic":
//                    rarity = 'M';
//                    break;

//                case "rare":
//                    rarity = 'R';
//                    break;
//                case "uncommon":
//                    rarity = 'U';
//                    break;
//                case "common":
//                    rarity = 'C';
//                    break;
//                default:
//                    throw new Exception("Error reading scryfall rarity");

//            }
//            return rarity;
//        }

//        public async Task AddCardDefinitionBatch(List<ScryfallMagicCard> cardBatch)
//        {
//            //lets try some nonsense
//            IEnumerable<LegacyDataContext.Card> cardsToAdd = cardBatch.Select(scryfallCard => new DataContext.Card
//            {
//                Id = scryfallCard.MultiverseId,
//                Cmc = scryfallCard.Cmc,
//                ManaCost = scryfallCard.ManaCost,
//                Name = scryfallCard.Name,
//                Text = scryfallCard.Text,
//                Type = scryfallCard.Type,

//                Set = _cardContext.Sets.FirstOrDefault(x => x.Code == scryfallCard.Set),

//                RarityId = GetRarityCode(scryfallCard.Rarity),


//                CardColors = scryfallCard.Colors
//                    .Join(
//                        _cardContext.ManaTypes,
//                        color => color,
//                        mt => mt.Id.ToString(),
//                        (color, mt) => new CardColor { ManaTypeId = mt.Id }).ToList(),

//                CardColorIdentities = scryfallCard.ColorIdentity
//                    .Join(
//                        _cardContext.ManaTypes,
//                        color => color,
//                        mt => mt.Id.ToString(),
//                        (color, mt) => new CardColorIdentity { ManaTypeId = mt.Id }).ToList(),

//                Variants = scryfallCard.Variants
//                    .Join(
//                        _cardContext.VariantTypes,
//                        v => v.Key,
//                        vt => vt.Name,
//                        (v, vt) => new CardVariant
//                        {
//                            ImageUrl = v.Value,
//                            Price = scryfallCard.Prices[v.Key],
//                            PriceFoil = scryfallCard.Prices[$"{v.Key}_foil"],
//                            CardVariantTypeId = vt.Id,
//                        }).ToList(),

//                Legalities = scryfallCard.Legalities
//                    .Join(
//                        _cardContext.MagicFormats,
//                        l => l,
//                        f => f.Name,
//                        (l, f) => new CardLegality
//                        {
//                            FormatId = f.Id
//                        }).ToList(),

//            });//.ToList();

//            await _cardContext.AddRangeAsync(cardsToAdd);
//            await _cardContext.SaveChangesAsync();
//        }

//        public async Task UpdateCardDefinition(ScryfallMagicCard scryfallCard)
//        {
//            var cardToUpdate = _cardContext.Cards.FirstOrDefault(x => x.Id == scryfallCard.MultiverseId);

//            if(cardToUpdate == null)
//            {
//                throw new Exception("Could not find the card to update");
//            }

//            //So all of the things I have to update don't actually exist on the Cards table

//            //I need to possibly add/remove legalities, and I need to update the price on variants

//            var existingVariants = _cardContext.CardVariants
//                .Where(x => x.CardId == cardToUpdate.Id).Include(x => x.Type);

//            await existingVariants.ForEachAsync(v =>
//            {
//                string variantName = v.Type.Name;
//                v.Price = scryfallCard.Prices[variantName];
//                v.PriceFoil = scryfallCard.Prices[$"{variantName}_foil"];
//            });

//            _cardContext.CardVariants.UpdateRange(existingVariants);


//            //.Select
//            //.Select(x => new CardVariant()
//            //{
//            //    Id = x.Id,
//            //    CardId = x.CardId,
//            //    ImageUrl = x.ImageUrl,
//            //    CardVariantTypeId = x.CardVariantTypeId,
//            //    Price = scryfallCard.Prices[x.Type.Name],
//            //    PriceFoil = scryfallCard.Prices[$"{x.Type.Name}_foil"]
//            //});


//            //existingVariants.ForEach(variant =>
//            //{
//            //    variant.Price = scryfallCard.Prices[variant.;
//            //    variant.PriceFoil = 0;
//            //});


//            var allExistingLegalities = _cardContext.CardLegalities.Where(x => x.CardId == cardToUpdate.Id).Include(x => x.Format);

//            //IDK if this will get messed up by case sensitivity
//            var existingLegalitiesToDelete = allExistingLegalities.Where(x => !scryfallCard.Legalities.Contains(x.Format.Name));

//            var legalityStringsToKeep = allExistingLegalities.Where(x => scryfallCard.Legalities.Contains(x.Format.Name)).Select(x => x.Format.Name);

//            var legalitiesToAdd = scryfallCard.Legalities
//                .Where(x => !legalityStringsToKeep.Contains(x))
//                .Select(x => new CardLegality()
//                {
//                    CardId = cardToUpdate.Id,
//                    Format = _cardContext.MagicFormats.Where(f => f.Name == x).FirstOrDefault(),
//                }).ToList();


//            _cardContext.CardLegalities.RemoveRange(existingLegalitiesToDelete);
//            _cardContext.CardLegalities.AddRange(legalitiesToAdd);
//            await _cardContext.SaveChangesAsync();

//        }

//        public IQueryable<LegacyDataContext.Card> QueryCardDefinitions()
//        {
//            var query = _cardContext.Cards.AsQueryable();
//            return query;
//        }
        
//        #endregion

//        #region Deck related methods

//        /// <summary>
//        /// Adds a new deck
//        /// The only way (currently) to add a new deck is through a modal that doesn't show basic lands, so they are omitted
//        /// </summary>
//        /// <param name="props"></param>
//        /// <returns></returns>
//        public async Task<int> AddDeck(DeckProperties props)
//        {

//            //TODO: Props should hold a format ID, not a format name
//            var deckFormat = _cardContext.MagicFormats.Where(x => x.Name.ToLower() == props.Format.ToLower()).First();

//            Deck newDeck = new Deck()
//            {
//                Name = props.Name,
//                Format = deckFormat,
//                Notes = props.Notes,
//            };

//            await _cardContext.Decks.AddAsync(newDeck);
//            await _cardContext.SaveChangesAsync();

//            return newDeck.Id;
//        }

//        /// <summary>
//        /// This method just updates the Deck data record, it does not modify the contents of a deck
//        /// </summary>
//        /// <param name="deckDto">The updated Deck properties</param>
//        /// <returns></returns>
//        public async Task UpdateDeck(DeckProperties deckDto)
//        {
//            Deck existingDeck = _cardContext.Decks.Where(x => x.Id == deckDto.Id).FirstOrDefault();

//            if (existingDeck == null)
//            {
//                throw new Exception("No deck found matching the specified ID");
//            }

//            var deckFormat = _cardContext.MagicFormats.Where(x => x.Name.ToLower() == deckDto.Format.ToLower()).First();

//            //TODO: Deck Properties should just hold a format ID instead of the string version of a format

//            existingDeck.Name = deckDto.Name;
//            existingDeck.Format = deckFormat; // deckDto.Format;
//            existingDeck.Notes = deckDto.Notes;
//            existingDeck.BasicW = deckDto.BasicW;
//            existingDeck.BasicU = deckDto.BasicU;
//            existingDeck.BasicB = deckDto.BasicB;
//            existingDeck.BasicR = deckDto.BasicR;
//            existingDeck.BasicG = deckDto.BasicG;

//            _cardContext.Decks.Update(existingDeck);
//            await _cardContext.SaveChangesAsync();
//        }

//        //In this latest version, when a deck is deleted, all related deck cards should also be deleted
//        //Since a deck card just has an association with an inventory card, we can safely delete the deck cards leaving the inventory items intact
//        public async Task DeleteDeck(int deckId)
//        {
//            var deckCardsToDelete = _cardContext.DeckCards.Where(x => x.DeckId == deckId).ToList();
//            if (deckCardsToDelete.Any())
//            {
//                _cardContext.DeckCards.RemoveRange(deckCardsToDelete);
//            }

//            var deckToDelete = _cardContext.Decks.Where(x => x.Id == deckId).FirstOrDefault();
//            _cardContext.Decks.Remove(deckToDelete);

//            await _cardContext.SaveChangesAsync();
//        }

//        public async Task AddDeckCard(DeckCardDto dto)
//        {
//            if (dto.DeckId == 0 || dto.InventoryCard == null || dto.InventoryCard.Id == 0)
//            {
//                throw new ArgumentNullException("Invalid deck card dto");
//            }

//            DeckCard newDeckCard = new DeckCard()
//            {
//                DeckId = dto.DeckId,
//                InventoryCardId = dto.InventoryCard.Id,
//            };

//            await _cardContext.DeckCards.AddAsync(newDeckCard);
//            await _cardContext.SaveChangesAsync();
//        }

//        //public async Task UpdateDeckCard(DeckCardDto dto)
//        public async Task UpdateDeckCard(int deckCardId, int deckId)
//        {
//            var sourceCard = await _cardContext.DeckCards.Where(x => x.Id == deckCardId).FirstOrDefaultAsync();

//            if (sourceCard == null)
//            {
//                throw new ArgumentNullException();
//            }

//            sourceCard.DeckId = deckId;

//            _cardContext.DeckCards.Update(sourceCard);

//            await _cardContext.SaveChangesAsync();
//        }

//        //Deletes a deck card, does not delete the associated inventory card
//        public async Task DeleteDeckCard(int deckCardId)
//        {
//            var cardToRemove = _cardContext.DeckCards.Where(x => x.Id == deckCardId).FirstOrDefault();
//            if (cardToRemove != null)
//            {
//                _cardContext.DeckCards.Remove(cardToRemove);
//                await _cardContext.SaveChangesAsync();
//            }
//            else
//            {
//                throw new Exception($"Could not find deck card wit ID {deckCardId}");
//            }
//        }


//        public IQueryable<DeckProperties> QueryDeckProperties()
//        {
//            IQueryable<DeckProperties> query = _cardContext.Decks
//                .Select(d => new DeckProperties()
//                {
//                    Id = d.Id,
//                    Name = d.Name,
//                    Notes = d.Notes,
//                    Format = d.Format.Name,
//                    BasicW = d.BasicW,
//                    BasicU = d.BasicU,
//                    BasicB = d.BasicB,
//                    BasicR = d.BasicR,
//                    BasicG = d.BasicG,
//                });

//            return query;
//        }

//        public IQueryable<DeckCard> QueryDeckCards()
//        {
//            IQueryable<DeckCard> query = _cardContext.DeckCards.AsQueryable();
//            return query;
//        }

//        public IQueryable<InventoryCard> QueryInventoryCardsForDeck(int deckId)
//        {
//            IQueryable<InventoryCard> query = _cardContext.DeckCards
//                .Where(x => x.DeckId == deckId)
//                .Select(x => x.InventoryCard)
//                .Include(x => x.Card.Variants);

//            return query;
//        }



        
        

//        #endregion

//        #region Inventory related methods

//        /// <summary>
//        /// Adds a new card to the inventory
//        /// Does not handle adding deck cards
//        /// </summary>
//        /// <param name="dto"></param>
//        /// <returns></returns>
//        public async Task<int> AddInventoryCard(InventoryCardDto dto)
//        {
//            var cardVariant = await _cardContext.VariantTypes.FirstOrDefaultAsync(x => x.Name == dto.VariantType);

//            InventoryCard newInventoryCard = new InventoryCard()
//            {
//                IsFoil = dto.IsFoil,
//                InventoryCardStatusId = dto.InventoryCardStatusId,
//                MultiverseId = dto.MultiverseId,
//                VariantType = cardVariant,
//            };

//            await _cardContext.InventoryCards.AddAsync(newInventoryCard);
//            await _cardContext.SaveChangesAsync();

//            return newInventoryCard.Id;
//        }

//        public async Task AddInventoryCardBatch(IEnumerable<InventoryCardDto> dtoBatch)
//        {
//            //assuming all card definitions exist already
//            var newCards = dtoBatch.Select(x => new InventoryCard()
//            {
//                IsFoil = x.IsFoil,
//                InventoryCardStatusId = x.InventoryCardStatusId,
//                MultiverseId = x.MultiverseId,
//                VariantType = _cardContext.VariantTypes.FirstOrDefault(v => v.Name == x.VariantType),

//            }).ToList();

//            await _cardContext.InventoryCards.AddRangeAsync(newCards);
//            await _cardContext.SaveChangesAsync();
//        }


//        /// <summary>
//        /// Updates an inventory card
//        /// In theory, the only fieds I'd practically want to update would be Status and maybe IsFoil??
//        /// This one might need to wait until variants are handled better...
//        /// </summary>
//        /// <param name="dto"></param>
//        /// <returns></returns>
//        public async Task UpdateInventoryCard(InventoryCardDto dto)
//        {
//            var dbCard = _cardContext.InventoryCards.FirstOrDefault(x => x.Id == dto.Id);
//            //currently only expecting to change the status with this method
//            dbCard.InventoryCardStatusId = dto.InventoryCardStatusId;
//            _cardContext.InventoryCards.Update(dbCard);
//            await _cardContext.SaveChangesAsync();
//        }

//        /// <summary>
//        /// Deletes a card from the inventory
//        /// Can only delete cards that don't belong to a deck
//        /// </summary>
//        /// <param name="id">Id of card to delete</param>
//        /// <returns></returns>
//        public async Task DeleteInventoryCard(int id)
//        {
//            var deckCardsReferencingThisCard = _cardContext.DeckCards.Where(x => x.DeckId == id).Count();

//            if (deckCardsReferencingThisCard > 0)
//            {
//                throw new Exception("Cannot delete a card that's currently in a deck");
//            }

//            var cardToRemove = _cardContext.InventoryCards.First(x => x.Id == id);

//            _cardContext.InventoryCards.Remove(cardToRemove);

//            await _cardContext.SaveChangesAsync();
//        }

//        public async Task<IQueryable<LegacyDataContext.Card>> QueryFilteredCards(InventoryQueryParameter filters)
//        {
//            var cardsQuery = _cardContext.Cards.AsQueryable();

//            if (!string.IsNullOrEmpty(filters.Set))
//            {
//                var matchingSetId = _cardContext.Sets.Where(x => x.Code.ToLower() == filters.Set.ToLower()).Select(x => x.Id).FirstOrDefault();
//                cardsQuery = cardsQuery.Where(x => x.SetId == matchingSetId);
//            }

//            if (filters.StatusId > 0)
//            {
//                //cardsQuery = cardsQuery.Where(x => x.)
//            }

//            if (filters.Colors.Any())
//            {
//                //var allowedColorIDs = filters.Colors.

//                var excludedColors = await _cardContext.ManaTypes.Where(x => !filters.Colors.Contains(x.Id.ToString())).Select(x => x.Id).ToListAsync();

//                //var includedColors = filters.Colors;

//                //only want cards where every color is an included color
//                //cardsQuery = cardsQuery.Where(x => !x.CardColorIdentities.Any() || x.CardColorIdentities.Any(color => includedColors.Contains(color.ManaTypeId.ToString())));

//                //alternative query, no excluded colors
//                cardsQuery = cardsQuery.Where(x => !(x.CardColorIdentities.Any(color => excludedColors.Contains(color.ManaTypeId))));

//            }

//            if (!string.IsNullOrEmpty(filters.Format))
//            {
//                //var matchingLegality = _cardContext.MagicFormats.Where(x => x.Name.ToLower() == param.Format.ToLower()).FirstOrDefault();
//                var matchingFormatId = await GetFormatIdByName(filters.Format);
//                cardsQuery = cardsQuery.Where(x => x.Legalities.Where(l => l.FormatId == matchingFormatId).Any());
//            }

//            if (filters.ExclusiveColorFilters)
//            {
//                cardsQuery = cardsQuery.Where(x => x.CardColorIdentities.Count() == filters.Colors.Count());
//            }

//            if (filters.MultiColorOnly)
//            {
//                cardsQuery = cardsQuery.Where(x => x.CardColorIdentities.Count() > 1);
//            }

//            if (!string.IsNullOrEmpty(filters.Type))
//            {
//                cardsQuery = cardsQuery.Where(x => x.Type.Contains(filters.Type));
//            }

//            if (filters.Rarity.DefaultIfEmpty().Any())
//            {
//                cardsQuery = cardsQuery.Where(x => filters.Rarity.Contains(x.Rarity.Name.ToLower()));

//            }

//            if (!string.IsNullOrEmpty(filters.Text))
//            {
//                cardsQuery = cardsQuery.Where(x =>
//                    x.Text.ToLower().Contains(filters.Text.ToLower())
//                    ||
//                    x.Name.ToLower().Contains(filters.Text.ToLower())
//                    ||
//                    x.Type.ToLower().Contains(filters.Text.ToLower())
//                );
//            }

//            return cardsQuery;
//        }

//        public async Task<IQueryable<InventoryOverviewDto>> QueryInventoryOverviews(InventoryQueryParameter filters)
//        {
//            var cardsQuery = await QueryFilteredCards(filters);

//            var query = cardsQuery.Select(x => new
//            {
//                MultiverseId = x.Id,
//                x.Name,
//                x.Type,
//                x.ManaCost,
//                Counts = x.InventoryCards.Where(c => c.InventoryCardStatusId == 1).Count(),
//                x.Variants.First().ImageUrl,
//            }).GroupBy(x => x.Name)
//            .Select(x => new InventoryOverviewDto
//            {
//                Name = x.Key,
//                Type = x.First().Type,
//                Cost = x.First().ManaCost,
//                Img = x.First().ImageUrl,
//                Count = x.Sum(card => card.Counts)
//            });

//            if (filters.MinCount > 0)
//            {
//                query = query.Where(x => x.Count >= filters.MinCount);
//            }

//            if (filters.MaxCount > 0)
//            {
//                query = query.Where(x => x.Count <= filters.MinCount);
//            }

//            return query;
//        }

//        #endregion

//        #region Private Reference lookups

//        private async Task<int> GetFormatIdByName(string formatName)
//        {
//            var format = await _cardContext.MagicFormats.Where(x => x.Name.ToLower() == formatName.ToLower()).FirstOrDefaultAsync();
//            if (format == null)
//            {
//                throw new Exception($"Could not find format matching name: {formatName}");
//            }
//            return format.Id;
//        }

//        private static FilterDescriptor MakeFilterDescriptor(string name)
//        {
//            return new FilterDescriptor()
//            {
//                Name = name,
//                Value = name.ToLower(),
//            };
//        }

//        #endregion

//        #region Core methods

//        public IQueryable<FilterDescriptor> QuerySetFilters()
//        {
//            //TODO - Can't get this sorting by release until I can make a DB schema update
//            IQueryable<FilterDescriptor> query = _cardContext.Sets
//                .Select(x => new FilterDescriptor()
//                {
//                    Name = x.Name,
//                    //Value = x.Code,
//                    Value = x.Id.ToString(),
//                });

//            return query;
//        }

//        public IQueryable<FilterDescriptor> QueryTypeFilters()
//        {
//            List<FilterDescriptor> setFilters = new List<FilterDescriptor>()
//            {
//                MakeFilterDescriptor("Creature"),
//                MakeFilterDescriptor("Instant"),
//                MakeFilterDescriptor("Sorcery"),
//                MakeFilterDescriptor("Enchantment"),
//                MakeFilterDescriptor("Land"),
//                MakeFilterDescriptor("Planeswalker"),
//                MakeFilterDescriptor("Artifact"),
//                MakeFilterDescriptor("Legendary"),
//            };

//            return setFilters.AsQueryable();
//        }

//        public IQueryable<FilterDescriptor> QueryFormatFilters()
//        {
//            IQueryable<FilterDescriptor> query = _cardContext.MagicFormats
//                .Select(x => new FilterDescriptor()
//                {
//                    Name = x.Name,
//                    Value = x.Id.ToString(),
//                });
//            return query;
//        }

//        public IQueryable<FilterDescriptor> QueryManaColorFilters()
//        {
//            //TODO - Find a way to make sure this is sorted in the expected order
//            IQueryable<FilterDescriptor> query = _cardContext.ManaTypes
//                .Select(x => new FilterDescriptor()
//                {
//                    Name = x.Name,
//                    Value = x.Id.ToString(),
//                });
//            return query;
//        }

//        public IQueryable<FilterDescriptor> QueryRarityFilters()
//        {
//            IQueryable<FilterDescriptor> query = _cardContext.Rarities
//                .Select(x => new FilterDescriptor()
//                {
//                    Name = x.Name,
//                    Value = x.Id.ToString(),
//                });
//            return query;
//        }

//        public IQueryable<FilterDescriptor> QueryCardStatusFilters()
//        {
//            IQueryable<FilterDescriptor> query = _cardContext.CardStatuses
//                .Select(x => new FilterDescriptor()
//                {
//                    Name = x.Name,
//                    Value = x.Id.ToString(),
//                });
//            return query;
//        }

//        #endregion
//    }
//}
