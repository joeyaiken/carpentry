using Carpentry.Data.DataContext;
using Carpentry.Data.DataModels;
using Carpentry.Data.Interfaces;
using Carpentry.Data.QueryResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carpentry.Data.Implementations
{
    public class DeckDataRepo : IDeckDataRepo
    {
        #region Constructor & globals

        //readonly ScryfallDataContext _scryContext;
        private readonly CarpentryDataContext _cardContext;
        private readonly ILogger<DeckDataRepo> _logger;

        public DeckDataRepo(CarpentryDataContext cardContext, ILogger<DeckDataRepo> logger)
        {
            _cardContext = cardContext;
            _logger = logger;
        }

        #endregion

        #region Deck crud

        public async Task<int> AddDeck(DeckData newDeck)
        {
            _cardContext.Decks.Add(newDeck);
            await _cardContext.SaveChangesAsync();
            return newDeck.DeckId;
        }

        public async Task AddImportedDeckBatch(IEnumerable<DeckData> deckList)
        {
            using (var transaction = _cardContext.Database.BeginTransaction())
            {
                _cardContext.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[Decks] ON");

                _cardContext.Decks.AddRange(deckList);

                await _cardContext.SaveChangesAsync();

                _cardContext.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[Decks] OFF");

                transaction.Commit();
            }

        }

        public async Task UpdateDeck(DeckData deck)
        {
            //Deck existingDeck = _cardContext.Decks.Where(x => x.Id == deck.Id).FirstOrDefault();

            //if (existingDeck == null)
            //{
            //    throw new Exception("No deck found matching the specified ID");
            //}

            //var deckFormat = _cardContext.MagicFormats.Where(x => x.Name.ToLower() == deckDto.Format.ToLower()).First();

            //TODO: Deck Properties should just hold a format ID instead of the string version of a format

            //existingDeck.Name = deckDto.Name;
            //existingDeck.Format = deckFormat; // deckDto.Format;
            //existingDeck.Notes = deckDto.Notes;
            //existingDeck.BasicW = deckDto.BasicW;
            //existingDeck.BasicU = deckDto.BasicU;
            //existingDeck.BasicB = deckDto.BasicB;
            //existingDeck.BasicR = deckDto.BasicR;
            //existingDeck.BasicG = deckDto.BasicG;

            _cardContext.Decks.Update(deck);
            await _cardContext.SaveChangesAsync();
        }

        /// <summary>
        /// Deletes a Deck, and any associated Deck Cards
        /// </summary>
        /// <param name="deckId"></param>
        /// <returns></returns>
        public async Task DeleteDeck(int deckId)
        {
            var deckCardsToDelete = _cardContext.DeckCards.Where(x => x.DeckId == deckId).ToList();
            if (deckCardsToDelete.Any())
            {
                _cardContext.DeckCards.RemoveRange(deckCardsToDelete);
            }

            var deckToDelete = _cardContext.Decks.Where(x => x.DeckId == deckId).FirstOrDefault();
            _cardContext.Decks.Remove(deckToDelete);

            await _cardContext.SaveChangesAsync();
        }

        public async Task<DeckData> GetDeckById(int deckId)
        {
            var matchingDeck = await _cardContext.Decks.Where(x => x.DeckId == deckId)
                .Include(x => x.Format)
                .FirstOrDefaultAsync();
            return matchingDeck;
        }

        /// <summary>
        /// Not case sensitive
        /// </summary>
        /// <param name="deckName"></param>
        /// <returns></returns>
        public async Task<DeckData> GetDeckByName(string deckName)
        {
            var matchingDeck = await _cardContext.Decks.Where(x => x.Name.ToLower() == deckName.ToLower())
                .Include(x => x.Format)
                .FirstOrDefaultAsync();
            return matchingDeck;
        }

        public async Task<IEnumerable<DeckData>> GetAllDecks()
        {
            var result = await _cardContext.Decks.Include(x => x.Format).ToListAsync();

            //var count = await _cardContext.InventoryCards.CountAsync();

            return result;
        }

        #endregion

        #region Deck Card crud

        public async Task AddDeckCard(DeckCardData newDeckCard)
        {
            //if (dto.DeckId == 0 || dto.InventoryCard == null || dto.InventoryCard.Id == 0)
            //{
            //    throw new ArgumentNullException("Invalid deck card dto");
            //}

            //DeckCard newDeckCard = new DeckCard()
            //{
            //    DeckId = dto.DeckId,
            //    InventoryCardId = dto.InventoryCard.Id,
            //};

            _cardContext.DeckCards.Add(newDeckCard);
            await _cardContext.SaveChangesAsync();
        }

        public async Task UpdateDeckCard(DeckCardData deckCard)
        {
            _cardContext.DeckCards.Update(deckCard);

            await _cardContext.SaveChangesAsync();
        }

        //Deletes a deck card, does not delete the associated inventory card
        public async Task DeleteDeckCard(int deckCardId)
        {
            var cardToRemove = _cardContext.DeckCards.Where(x => x.DeckCardId == deckCardId).FirstOrDefault();
            if (cardToRemove != null)
            {
                _cardContext.DeckCards.Remove(cardToRemove);
                await _cardContext.SaveChangesAsync();
            }
            else
            {
                throw new Exception($"Could not find deck card with ID {deckCardId}");
            }
        }

        public async Task<DeckCardData> GetDeckCardById(int deckCardId)
        {
            var matchingDeckCard = await _cardContext.DeckCards.Where(x => x.DeckCardId == deckCardId).FirstOrDefaultAsync();
            return matchingDeckCard;
        }

        public async Task<DeckCardData> GetDeckCardByInventoryId(int inventoryCardId)
        {
            var matchingDeckCard = await _cardContext.DeckCards.Where(x => x.InventoryCardId == inventoryCardId).FirstOrDefaultAsync();
            return matchingDeckCard;
        }

        #endregion

        #region Queries

        public async Task<List<DeckCardResult>> GetDeckCards(int deckId)
        {
            var deckCards = await _cardContext.DeckCards
                .Where(x => x.DeckId == deckId)
                .Select(x => new DeckCardResult()
                {
                    Id = x.DeckCardId,
                    Category = x.CategoryId == null ? null : x.Category.Name,
                    Cmc = x.InventoryCard.Card.Cmc,
                    Cost = x.InventoryCard.Card.ManaCost,
                    //Img = x.InventoryCard.Card.Variants.FirstOrDefault(v => v.CardVariantTypeId == 1).ImageUrl,
                    Img = x.InventoryCard.Card.ImageUrl,
                    IsFoil = x.InventoryCard.IsFoil,
                    CollectorNumber = x.InventoryCard.Card.CollectorNumber,
                    CardId = x.InventoryCard.CardId,
                    Name = x.InventoryCard.Card.Name,
                    Set = x.InventoryCard.Card.Set.Code,
                    Type = x.InventoryCard.Card.Type,
                    //VariantType = x.InventoryCard.VariantType.Name,
                }).ToListAsync();

            return deckCards;
        }

        public async Task<List<char>> GetDeckColorIdentity(int deckId)
        {

            //For deck cards w/o an inventory card, need to get the most recent card by name
            //Maybe I just do this in a view...


            var deckColorStrings = await _cardContext.DeckCards
                .Where(x => x.DeckId == deckId)
                .Select(x => x.InventoryCard.Card.ColorIdentity)
                .ToListAsync();

            if(deckColorStrings == null)
            {
                return new List<char>();
            }

            var deckCardColors = deckColorStrings
                .SelectMany(x => x.ToCharArray())
                .Distinct()
                .ToList();

            var dbDeck = _cardContext.Decks.Where(x => x.DeckId == deckId).FirstOrDefault();

            if (dbDeck.BasicW > 0 && !deckCardColors.Contains('W'))
            {
                deckCardColors.Add('W');
            }

            if (dbDeck.BasicU > 0 && !deckCardColors.Contains('U'))
            {
                deckCardColors.Add('U');
            }

            if (dbDeck.BasicB > 0 && !deckCardColors.Contains('B'))
            {
                deckCardColors.Add('B');
            }

            if (dbDeck.BasicR > 0 && !deckCardColors.Contains('R'))
            {
                deckCardColors.Add('R');
            }

            if (dbDeck.BasicG > 0 && !deckCardColors.Contains('G'))
            {
                deckCardColors.Add('G');
            }

            return deckCardColors;
        }

        public async Task<int> GetDeckCardCount(int deckId)
        {
            int basicLandCount = await _cardContext.Decks.Where(x => x.DeckId == deckId).Select(deck => deck.BasicW + deck.BasicU + deck.BasicB + deck.BasicR + deck.BasicG).FirstOrDefaultAsync();
            int cardCount = await _cardContext.DeckCards.Where(x => x.DeckId == deckId).CountAsync();
            return basicLandCount + cardCount;
        }

        public async Task<IEnumerable<DeckCardStatResult>> GetDeckCardStats(int deckId)
        {
            var query = _cardContext.DeckCards.Where(x => x.DeckId == deckId)
                .Select(x => new
                {
                    Card = x.InventoryCard.Card,
                    //Variant = x.InventoryCard.Card.Variants
                    //    .Where(cardVariant => cardVariant.CardVariantTypeId == x.InventoryCard.VariantTypeId)
                    //    .FirstOrDefault(),

                    DeckCard = x,
                    IsFoil = x.InventoryCard.IsFoil,
                    //ColorIdentity = x.InventoryCard.Card.CardColorIdentities.SelectMany<char>(ci => ci.ManaTypeId)
                    ColorIdentity = x.InventoryCard.Card.ColorIdentity.ToCharArray(),
                }).ToList();

            List<DeckCardStatResult> results = query.Select(x => new DeckCardStatResult()
            {
                CategoryId = x.DeckCard.CategoryId,
                Cmc = x.Card.Cmc,
                ColorIdentity = x.ColorIdentity.Length == 0 ? new List<string>() : x.ColorIdentity.Select(c => c.ToString()).ToList(),
                Price = (x.IsFoil ? x.Card.PriceFoil : x.Card.Price),
                Type = x.Card.Type,
            }).ToList();

            return results;

            //List<DeckCardStatResult> results = _cardContext.DeckCards.Where(x => x.DeckId == deckId)
            //    .Select(x => new DeckCardStatResult()
            //    {
            //        CategoryId = x.CategoryId,
            //        Cmc = x.InventoryCard.Card.Cmc,
            //        ColorIdentity = null,
            //        //SHIT this doesn't properly grab foils

            //        Price = x.InventoryCard.Card.Variants
            //            .Where(cardVariant => cardVariant.CardVariantTypeId == x.InventoryCard.VariantTypeId).FirstOrDefault()
            //        Type = x.InventoryCard.Card.Type,


            //    }).ToList();
        }

        #endregion
    }
}
