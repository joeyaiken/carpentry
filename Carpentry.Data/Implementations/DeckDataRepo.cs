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
    public class DeckDataRepo //: IDeckDataRepo
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
            //using (var transaction = _cardContext.Database.BeginTransaction())
            //{
                //_cardContext.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[Decks] ON");

                _cardContext.Decks.AddRange(deckList);

                await _cardContext.SaveChangesAsync();

                //_cardContext.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[Decks] OFF");

            //    transaction.Commit();
            //}

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

        //Note: this is only ever used by by [Get Deck Detail], the DTO could / should be used to get the desired info for a deck card
        //The same info in [ThatDevQuery(int deckId)]
        public async Task<List<DeckCardResult>> GetDeckCards(int deckId)
        {
            var deckCards = await _cardContext.DeckCards.Where(dc => dc.DeckId == deckId)
                .Select(x => new DeckCardResult()
                {
                    DeckCardId = x.DeckCardId,
                    DeckId = x.DeckId,
                    Name = x.CardName,

                    Category = x.CategoryId == null ? null : x.Category.Name,
                    InventoryCardId = x.InventoryCardId,
                    
                    Cmc = x.InventoryCard.Card.Cmc,
                    Cost = x.InventoryCard.Card.ManaCost,
                    Img = x.InventoryCard.Card.ImageUrl,
                    IsFoil = x.InventoryCard.IsFoil,
                    CollectorNumber = x.InventoryCard.Card.CollectorNumber,
                    CardId = x.InventoryCard.CardId,
                    //Set = x.InventoryCard.Card.Set.Code,
                    //SetId = x.InventoryCard.Card.SetId,
                    SetCode = x.InventoryCard.Card.Set.Code,
                    Type = x.InventoryCard.Card.Type,
                    ColorIdentity = x.InventoryCard.Card.ColorIdentity,
                    Price = x.InventoryCard.Card.Price,
                    PriceFoil = x.InventoryCard.Card.PriceFoil,
                    Tags = x.Deck.Tags.Where(t => t.CardName == x.CardName).Select(t => t.Description).ToList(),
                }).ToListAsync();

            //get all names with null inventory cards
            var cardNames = deckCards.Where(dc => dc.InventoryCardId == null).Select(dc => dc.Name).Distinct().ToList();

            var relevantCardsByName = (await _cardContext.InventoryCardByName
                .Where(c => cardNames.Contains(c.Name))
                .ToListAsync())
                //.Select(c => new CardData()
                //{
                //    CardId = c.CardId,
                //    Cmc = c.Cmc,
                //    ManaCost = c.ManaCost,
                //    Name = c.Name,
                //    RarityId = c.RarityId,
                //    SetId = c.SetId,
                //    Text = c.Text,
                //    Type = c.Type,
                //    MultiverseId = c.MultiverseId,
                //    Price = c.Price,
                //    PriceFoil = c.PriceFoil,
                //    ImageUrl = c.ImageUrl,
                //    CollectorNumber = c.CollectorNumber,
                //    TixPrice = c.TixPrice,
                //    Color = c.Color,
                //    ColorIdentity = c.ColorIdentity,
                //})
                .ToDictionary(c => c.Name, c => c);

            //match everything!
            //(alternatively, get this all from a view)

            //var result = new List<CardData>();

            foreach (var dc in deckCards)
            {
                //var matchingCard =
                if (dc.InventoryCardId == null)
                {
                    //card by id
                    var match = relevantCardsByName[dc.Name];
                    dc.Cmc = match.Cmc;
                    dc.Cost = match.ManaCost;
                    dc.Img = match.ImageUrl;
                    //dc.IsFoil = false;
                    dc.CollectorNumber = match.CollectorNumber;
                    dc.CardId = match.CardId;
                    dc.Name = match.Name;
                    //dc.Set = match.Set;
                    //dc.SetId = match.SetId;
                    dc.SetCode = match.SetCode;
                    dc.Type = match.Type;
                    dc.ColorIdentity = match.ColorIdentity;
                    dc.Price = match.Price;
                    dc.PriceFoil = match.PriceFoil;
                    //result.Add(relevantCardsByName[dc.CardName]);
                }
            }
     


            return deckCards;
        }

        //public async Task<IQueryable<CardData>> QueryMostRecentPrints()
        //{
        //    //attempting to get the most recent print for each card name
        //    //WAIT, doesn't vwInventoryByName get what I need?  Totals by name and info on the most recent print?
        //}

        //Attempting to get all Cards used in a deck
        //  for any empty deck card, the newest print will be queried for card stats
        //  Will include as many copies as cards in a deck
        public async Task<List<CardData>> ThatDevQuery_(int deckId)
        {
            //get all deck cards
            var deckCards = await _cardContext.DeckCards.Where(dc => dc.DeckId == deckId)
                .Select(x => new DeckCardResult()
                {
                    DeckCardId = x.DeckCardId,
                    Category = x.CategoryId == null ? null : x.Category.Name,
                    InventoryCardId = x.InventoryCardId,
                    Cmc = x.InventoryCard.Card.Cmc,
                    Cost = x.InventoryCard.Card.ManaCost,
                    Img = x.InventoryCard.Card.ImageUrl,
                    IsFoil = x.InventoryCard.IsFoil,
                    CollectorNumber = x.InventoryCard.Card.CollectorNumber,
                    CardId = x.InventoryCard.CardId,
                    Name = x.InventoryCard.Card.Name,
                    //Set = x.InventoryCard.Card.Set.Code,
                    Type = x.InventoryCard.Card.Type,
                }).ToListAsync();

            //get all inventory card IDs
            //var inventoryCardIds = deckCards.Where(dc => dc.InventoryCardId != null).Select(dc => dc.InventoryCardId.Value).ToList();

            //var relevantCardsByInventoryId = await _cardContext.InventoryCards
            //    .Where(ic => inventoryCardIds.Contains(ic.InventoryCardId))
            //    .Include(ic => ic.Card)
            //    .ToDictionaryAsync(ic => ic.InventoryCardId, ic => ic.Card);
                

            //get all names with null inventory cards
            var cardNames = deckCards.Where(dc => dc.InventoryCardId == null).Select(dc => dc.Name).Distinct().ToList();

            var relevantCardsByName = await _cardContext.InventoryCardByName
                .Where(c => cardNames.Contains(c.Name))
                //.Select(c => new CardData()
                //{
                //    CardId = c.CardId,
                //    Cmc = c.Cmc,
                //    ManaCost = c.ManaCost,
                //    Name = c.Name,
                //    RarityId = c.RarityId,
                //    SetId = c.SetId,
                //    Text = c.Text,
                //    Type = c.Type,
                //    MultiverseId = c.MultiverseId,
                //    Price = c.Price,
                //    PriceFoil = c.PriceFoil,
                //    ImageUrl = c.ImageUrl,
                //    CollectorNumber = c.CollectorNumber,
                //    TixPrice = c.TixPrice,
                //    Color = c.Color,
                //    ColorIdentity = c.ColorIdentity,
                //})
                .ToDictionaryAsync(c => c.Name, c => c);

            //match everything!

            //(alternatively, get this all from a view)

            var result = new List<CardData>();

            foreach(var dc in deckCards)
            {
                //var matchingCard =
                if(dc.InventoryCardId == null)
                {
                    //card by id
                    var match = relevantCardsByName[dc.Name];
                    dc.Cmc = match.Cmc;
                    dc.Cost = match.ManaCost;
                    dc.Img = match.ImageUrl;
                    //dc.IsFoil = match.IsFoil;
                    dc.CollectorNumber = match.CollectorNumber;
                    dc.CardId = match.CardId;
                    dc.Name = match.Name;
                    //dc.Set = match.Set;
                    dc.Type = match.Type;
                    
                    //result.Add(relevantCardsByName[dc.CardName]);
                }
                //else
                //{
                //    //card by name
                //    var match = relevantCardsByInventoryId[dc.InventoryCardId.Value];
                //    result.Add(match);
                //}
            }
            return result;
        }

        public async Task<List<char>> GetDeckColorIdentity(int deckId)
        {

            //For deck cards w/o an inventory card, need to get the most recent card by name
            //Maybe I just do this in a view...


            //first, get all cards in the deck (for empty deck cards, get the most recent print)

            var cards = await GetDeckCards(deckId);

            var deckColorStrings = cards.Select(c => c.ColorIdentity).ToList();



            //var deckColorStrings = await _cardContext.DeckCards
            //    .Where(x => x.DeckId == deckId)
            //    .Select(x => x.InventoryCard.Card.ColorIdentity)
            //    .ToListAsync();

            if (deckColorStrings == null)
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




            //This query breaks with empty deck cards
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

        //get card tags for a deck card
        public async Task<List<DeckCardTagData>> GetDeckCardTags(int deckId, string cardName) //cardId / cardName ?
        {
            var tags = await _cardContext.CardTags.Where(t => t.CardName == cardName && t.DeckId == deckId).ToListAsync();
            return tags;
        }

        public async Task<List<DeckCardTagData>> GetDeckCardTags(int deckCardId)
        {
            var deckCard = await _cardContext.DeckCards.FirstOrDefaultAsync(dc => dc.DeckCardId == deckCardId);
            return await GetDeckCardTags(deckCard.DeckId, deckCard.CardName);
        }

        public async Task<List<string>> GetAllDeckCardTags(int? deckId)
        {
            var tagsQuery = _cardContext.CardTags.AsQueryable();
            
            if(deckId != null)
            {
                tagsQuery = tagsQuery.Where(t => t.DeckId == deckId);
            }

            var tags = await tagsQuery.Select(t => t.Description).Distinct().ToListAsync();
            
            return tags;
        }

        public async Task AddDeckCardTag(DeckCardTagData newTag)
        {
            _cardContext.CardTags.Add(newTag);
            await _cardContext.SaveChangesAsync();
        }

        public async Task RemoveDeckCardTag(int deckCardTagId)
        {
            var tag = await _cardContext.CardTags.SingleAsync(t => t.DeckCardTagId == deckCardTagId);
            _cardContext.Remove(tag);
            await _cardContext.SaveChangesAsync();
        }

        #endregion
    }
}
