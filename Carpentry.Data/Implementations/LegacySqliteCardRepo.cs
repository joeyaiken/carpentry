using Carpentry.Data.Interfaces;
using Carpentry.Data.Models;
using Carpentry.Data.DataContextLegacy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Carpentry.Data.Implementations
{
    //The latest itteration of the Sqlite Card Repo!
    //How complex should this be, and how much logic should be moved to the controller layer?
    public class LegacySqliteCardRepo //: ILegacySqliteCardRepo
    {

        readonly LegacySqliteDataContext _S9Context;

        public LegacySqliteCardRepo(LegacySqliteDataContext S9Context)
        {
            _S9Context = S9Context;
        }

        #region private

        private static MagicCardDto MapDBCardToDto(DataContextLegacy.Card card)
        {
            var result = new MagicCardDto
            {
                Cmc = card.Cmc,
                ImageArtCropUrl = card.ImageArtCropUrl,
                ImageUrl = card.ImageUrl,
                ManaCost = card.ManaCost,
                MultiverseId = card.Id,
                Name = card.Name,
                Price = card.Price,
                PriceFoil = card.PriceFoil,
                Rarity = card.Rarity.Name,
                Set = card.Set.Code,
                Text = card.Text,
                Type = card.Type,
                //ColorIdentity = card.ManaCost.Select(x => card.)
                ColorIdentity = card.CardColorIdentities.Select(i => i.ManaType.Name).ToList()
            };

            return result;
        }

        //private static DataContextLegacy.Card MapMagicCardDtoToDB(MagicCardDto dto)
        //{

        //}

        #endregion

        #region Deck related methods

        public async Task<int> AddDeck(DeckProperties props)
        {
            DataContextLegacy.Deck newDeck = new DataContextLegacy.Deck()
            {
                Name = props.Name,
                Format = props.Format,
                Notes = props.Notes,
            };

            await _S9Context.Decks.AddAsync(newDeck);
            await _S9Context.SaveChangesAsync();

            return newDeck.Id;
        }

        public async Task UpdateDeck(DeckProperties deckDto)
        {
            DataContextLegacy.Deck existingDeck = _S9Context.Decks.Where(x => x.Id == deckDto.Id).FirstOrDefault();

            if (existingDeck == null)
            {
                throw new Exception("No deck found matching the specified ID");
            }

            existingDeck.Name = deckDto.Name;
            existingDeck.Format = deckDto.Format;
            existingDeck.Notes = deckDto.Notes;

            existingDeck.BasicW = deckDto.BasicW;
            existingDeck.BasicU = deckDto.BasicU;
            existingDeck.BasicB = deckDto.BasicB;
            existingDeck.BasicR = deckDto.BasicR;
            existingDeck.BasicG = deckDto.BasicG;

            _S9Context.Decks.Update(existingDeck);
            await _S9Context.SaveChangesAsync();
        }

        //In this latest version, when a deck is deleted, all related deck cards should also be deleted
        //Since a deck card just has an association with an inventory card, we can safely delete the deck cards leaving the inventory items intact
        public async Task DeleteDeck(int deckId)
        {
            var deckCardsToDelete = _S9Context.DeckCards.Where(x => x.DeckId == deckId).ToList();
            _S9Context.DeckCards.RemoveRange(deckCardsToDelete);

            var deckToDelete = _S9Context.Decks.Where(x => x.Id == deckId).FirstOrDefault();
            _S9Context.Decks.Remove(deckToDelete);

            await _S9Context.SaveChangesAsync();
        }

        public async Task<IEnumerable<DeckProperties>> SearchDecks()
        {
            var deckList = _S9Context.Decks.Select(d => new DeckProperties()
            {
                Id = d.Id,
                Name = d.Name,
                Notes = d.Notes,
                Format = d.Format
            }).ToList();

            return await Task.FromResult(deckList);
        }
        /*
        public async Task<DeckDto> GetDeck(int deckId)
        {

            //return await LoadLegacyDeck(deckId);

            var targetDeck = _S9Context.Decks.Where(x => x.Id == deckId).FirstOrDefault();

            DeckDto result = new DeckDto
            {
                Cards = new List<Models.Card>(),
                Data = new Dictionary<int, MagicCardDto>(),
                Props = new DeckProperties
                {
                    Format = targetDeck.Format.ToString(),
                    Id = targetDeck.Id,
                    Name = targetDeck.Name,
                    Notes = targetDeck.Notes,
                    BasicW = targetDeck.BasicW,
                    BasicU = targetDeck.BasicU,
                    BasicB = targetDeck.BasicB,
                    BasicR = targetDeck.BasicR,
                    BasicG = targetDeck.BasicG
                }
            };

            var letsSee = _S9Context.DeckCards.Where(x => x.DeckId == targetDeck.Id).Select(x => x.InventoryCard);


            //var cardsFromDb = _context.Cards.Where(x => x.DeckId == targetDeck.Id);
            var cardsFromDb = _S9Context.DeckCards.Where(x => x.DeckId == targetDeck.Id).Select(x => x.InventoryCard);

            List<Models.Card> cards = cardsFromDb.Select(x => new Models.Card
            {
                Id = x.Id,
                IsFoil = x.IsFoil,
                MultiverseId = x.MultiverseId,
                DeckId = deckId
            }).ToList();

            result.Cards = cards;

            result.Data = cardsFromDb
                //.Select(x => JsonConvert.DeserializeObject<MagicCardDto>(x.Data.StringData))
                .Select(x => x.Card)

                .Select(x => new MagicCardDto
                {
                    Cmc = x.Cmc,
                    ImageArtCropUrl = x.ImageArtCropUrl,
                    ImageUrl = x.ImageUrl,
                    ManaCost = x.ManaCost,
                    MultiverseId = x.Id,
                    Name = x.Name,
                    Price = x.Price,
                    PriceFoil = x.PriceFoil,
                    Rarity = x.Rarity.Name,
                    Set = x.Set.Code,
                    Text = x.Text,
                    Type = x.Type,
                    //ColorIdentity = x.ManaCost.Select(x => x.)
                    ColorIdentity = x.CardColorIdentities.Select(i => i.ManaType.Name).ToList()
                })
                //.Select(x => new MagicCardDto
                //{
                //    Cmc = x.Card.Cmc,

                //})
                .AsEnumerable().Distinct(new CardComparer())
                .ToDictionary(x => x.MultiverseId, x => x);

            return await Task.FromResult(result);
            
        }
        */

        //Deletes a deck card, does not delete the associated inventory card
        public async Task DeleteDeckCard(int deckCardId)
        {
            var cardToRemove = _S9Context.DeckCards.Where(x => x.Id == deckCardId).FirstOrDefault();
            if(cardToRemove != null) {
                _S9Context.DeckCards.Remove(cardToRemove);
                await _S9Context.SaveChangesAsync();
            }
        }

        #endregion

        #region Inventory related methods

        public async Task<int> AddCard(CardDto dto)
        {
            if (dto.Card == null || dto.Data == null)
            {
                throw new ArgumentNullException();
            }

            //make sure the card with this MID exist in the DB

            var thisCard = _S9Context.Cards.FirstOrDefault(x => x.Id == dto.Card.MultiverseId);

            if(thisCard == null)
            {
                var matchingSet = _S9Context.Sets.FirstOrDefault(x => x.Code == dto.Data.Set);

                var matchingRarity = _S9Context.Rarities.FirstOrDefault(x => x.Name == dto.Data.Rarity);

                DataContextLegacy.Card newCard = new DataContextLegacy.Card()
                {
                    Id = dto.Data.MultiverseId,

                    Cmc = dto.Data.Cmc,
                    ImageUrl = dto.Data.ImageUrl,
                    ImageArtCropUrl = dto.Data.ImageArtCropUrl,
                    ManaCost = dto.Data.ManaCost,
                    Name = dto.Data.Name,
                    Price = dto.Data.Price,
                    PriceFoil = dto.Data.PriceFoil,

                    //Rarity = parsedScryfallItem.SelectToken("rarity").ToObject<string>(),
                    Rarity = matchingRarity,

                    //Set = parsedScryfallItem.SelectToken("set").ToObject<string>(),
                    Set = matchingSet,

                    Text = dto.Data.Text,
                    Type = dto.Data.Type,

                    //ColorIdentity = parsedScryfallItem.SelectToken("color_identity").ToObject<List<string>>(),
                };

                _S9Context.Cards.Add(newCard);


                //var manaTypeOptions = _S9Context.ManaTypes.ToList();

                dto.Data.ColorIdentity.ForEach(cid =>
                {
                    _S9Context.ColorIdentities.Add(new CardColorIdentity
                    {
                        Card = newCard,
                        ManaType = _S9Context.ManaTypes.FirstOrDefault(x => x.Id.ToString() == cid),
                    });
                });

            }


            /*
             DataContextLegacy.Card newCard = new DataContextLegacy.Card()
                    {
                        Id = parsedLegacyItem.MultiverseId,

                        Cmc = parsedLegacyItem.Cmc,
                        ImageUrl = parsedLegacyItem.ImageUrl,
                        ImageArtCropUrl = parsedLegacyItem.ImageArtCropUrl,
                        ManaCost = parsedLegacyItem.ManaCost,
                        Name = parsedLegacyItem.Name,
                        Price = parsedLegacyItem.Price,
                        PriceFoil = parsedLegacyItem.PriceFoil,

                        //Rarity = parsedScryfallItem.SelectToken("rarity").ToObject<string>(),
                        Rarity = matchingRarity,

                        //Set = parsedScryfallItem.SelectToken("set").ToObject<string>(),
                        Set = thisSet,

                        Text = parsedLegacyItem.Text,
                        Type = parsedLegacyItem.Type,

                        //ColorIdentity = parsedScryfallItem.SelectToken("color_identity").ToObject<List<string>>(),
                    };
             */





            //dbInvCard = _S9Context.InventoryCards.FirstOrDefault(x => x.)
            InventoryCard newInvCard = new InventoryCard
            {
                InventoryCardStatusId = dto.Card.CardStatusId,
                MultiverseId = dto.Card.MultiverseId,
                IsFoil = dto.Card.IsFoil
            };
            _S9Context.InventoryCards.Add(newInvCard);

            await _S9Context.SaveChangesAsync();

            return newInvCard.Id;

            //    await EnsureSetExistsLocally(dto.Data.Set);

            //    //see if a card definition exists for this card
            //    Data.CardDetail cardData = _context.CardDetails.Where(x => x.MultiverseId == dto.Card.MultiverseId).FirstOrDefault();

            //    //if (cardData == null)
            //    //{
            //    //    cardData = new Data.CardDetail()
            //    //    {
            //    //        MultiverseId = dto.Card.MultiverseId,
            //    //        StringData = JsonConvert.SerializeObject(dto.Data, Formatting.None)
            //    //    };
            //    //    _context.CardDetails.Add(cardData);
            //    //}

            //    Data.Card newCard = new Data.Card()
            //    {
            //        MultiverseId = dto.Card.MultiverseId,
            //        IsFoil = dto.Card.IsFoil,
            //        Data = cardData,
            //        DeckId = dto.Card.DeckId
            //    };

            //    if (dto.Card.DeckId != null)
            //    {
            //        var deckToFind = _context.Decks.Where(x => x.Id == dto.Card.DeckId).FirstOrDefault();
            //        newCard.Deck = deckToFind;
            //    }

            //    _context.Cards.Add(newCard);

            //    await _context.SaveChangesAsync();

        }

        public async Task UpdateCard(CardDto dto)
        {
            var dbCard = _S9Context.InventoryCards.FirstOrDefault(x => x.Id == dto.Card.Id);

            dbCard.MultiverseId = dto.Card.MultiverseId;
            dbCard.IsFoil = dto.Card.IsFoil;
            dbCard.InventoryCardStatusId = dto.Card.CardStatusId;

            _S9Context.Update(dbCard);

            await _S9Context.SaveChangesAsync();
        }

        public async Task DeleteCard(int id)
        {
            var deckCardsReferencingThisCard = _S9Context.DeckCards.Where(x => x.DeckId == id).Count();

            if(deckCardsReferencingThisCard > 0)
            {
                throw new Exception("Cannot delete a card that's currently in a deck");
            }

            var cardToRemove = _S9Context.InventoryCards.Where(x => x.Id == id).FirstOrDefault();

            _S9Context.InventoryCards.Remove(cardToRemove);

            await _S9Context.SaveChangesAsync();
        }

        public async Task<List<InventoryQueryResult>> SearchInventory(InventoryQueryParameter param)
        {
            if(param == null)
            {
                throw new ArgumentNullException();
            }

            //first step was to get all of the cards in the inventory, in this case I only added Cards for items in the inventory




            //await CalculateInventoryTotalPrice();



            //var allCards = _context.Cards.Select(x => new Models.Card()
            //{
            //    Id = x.Id,
            //    MultiverseId = x.MultiverseId,
            //    IsFoil = x.IsFoil,
            //    DeckId = x.DeckId
            //}).ToList();

            //only want card details for cards I actually own
            //var cardDetails = _context.Cards.Select(x => x.Data).ToList();




            //var destinctCardDetails = cardDetails.Distinct(new DataCardDetailComparer()).ToList();

            //Expression<Func<MagicCardDto, bool>> filterPredicate = (x => x.Name.Contains(param.Text));

            //var cardDetailQuery = destinctCardDetails.Select(x => JObject.Parse(x.StringData).ToObject<MagicCardDto>());

            var cardDetailQuery = _S9Context.Cards.Where(x => true);

            if (param != null && !string.IsNullOrWhiteSpace(param.Text))
            {
                var lowerText = param.Text.ToLower();
                cardDetailQuery = cardDetailQuery.Where(x =>
                    (x.Text != null && x.Text.ToLower().Contains(lowerText))
                    ||
                    x.Type.ToLower().Contains(lowerText)
                    ||
                    x.Name.ToLower().Contains(lowerText)
                );





                //var nullQuery = cardDetailQuery.Where(x =>
                //    (x.Text == null)
                //    ||
                //    x.Type.ToLower().Contains(lowerText)
                //    ||
                //    x.Name.ToLower().Contains(lowerText)
                //);

                //var cardDetailList = filteredDetailQuery.ToList();

                //int breakpoint = 1;
            }

            //need the distinct names, then the #s for all those names

            /*
             
             .Select(x => new MagicCardDto
                {
                    Cmc = x.Cmc,
                    ImageArtCropUrl = x.ImageArtCropUrl,
                    ImageUrl = x.ImageUrl,
                    ManaCost = x.ManaCost,
                    MultiverseId = x.Id,
                    Name = x.Name,
                    Price = x.Price,
                    PriceFoil = x.PriceFoil,
                    Rarity = x.Rarity.Name,
                    Set = x.Set.Code,
                    Text = x.Text,
                    Type = x.Type,
                    //ColorIdentity = x.ManaCost.Select(x => x.)
                    ColorIdentity = x.CardColorIdentities.Select(i => i.ManaType.Name).ToList()
                })
             */

            //what are the distinct names?
            var distinctPaginatedNames = cardDetailQuery.Include(x => x.CardColorIdentities)
                //.Select(x => MapDBCardToDto(x)) // This should be a SELECT instead of the function
                .Select(x => new MagicCardDto
                {
                    Cmc = x.Cmc,
                    ImageArtCropUrl = x.ImageArtCropUrl,
                    ImageUrl = x.ImageUrl,
                    ManaCost = x.ManaCost,
                    MultiverseId = x.Id,
                    Name = x.Name,
                    Price = x.Price,
                    PriceFoil = x.PriceFoil,
                    Rarity = x.Rarity.Name,
                    Set = x.Set.Code,
                    Text = x.Text,
                    Type = x.Type,
                    //ColorIdentity = x.ManaCost.Select(x => x.)
                    ColorIdentity = x.CardColorIdentities.Select(i => i.ManaType.Name).ToList()
                })
                .GroupBy(x => x.Name)
                .Select(group => new
                {
                    Name = group.Key,
                    Printings = group.ToList(),
                    Mids = group.Select(x => x.MultiverseId).ToList()
                }).OrderBy(x => x.Name);

            if(param.Skip > 0 || param.Take > 0)
            {
                distinctPaginatedNames = distinctPaginatedNames.Skip(param.Skip).Take(param.Take).OrderBy(x => x.Name);
            }
                //.Skip(0).Take(50);

            //option 1: we return an object that's similar to a normalized DB

            //option 2, since this won't have (much?) data redundancy, we'll return a more JSON like object
            //SPOILER: Doing this for now

            //var nextStep = distinctPaginatedNames.Select(x => new
            //{
            //    Name = x.Name,
            //    Data = x.Printings.OrderByDescending(card => card.MultiverseId).First(),
            //    Cards = _context.Cards.Where(c => x.Mids.Contains(c.MultiverseId))
            //}).ToList();

            var queryResults = distinctPaginatedNames.Select(x => new InventoryQueryResult
            {
                Name = x.Name,
                Cards = x.Printings,
                Items = new List<Models.Card>()
                //Items = _context.Cards.Where(c => x.Mids.Contains(c.MultiverseId)).ToList()
            }).ToList();


            queryResults.ForEach(item =>
            {
                var itemMIDs = item.Cards.Select(x => x.MultiverseId).ToList();

                var cardsMatchingThisItem = _S9Context.InventoryCards.Where(x => itemMIDs.Contains(x.MultiverseId));
                item.Items = cardsMatchingThisItem.Select(x => new Models.Card
                {
                    //DeckId = x.DeckId,
                    Id = x.Id,
                    IsFoil = x.IsFoil,
                    MultiverseId = x.MultiverseId
                }).ToList();
                    //var cardsMatchingThisItem = _context.Cards.Where(c => c.m)

            });

            //var cardDataDictionary = destinctCardDetails
            //    .Select(x => JObject.Parse(x.StringData)
            //    .ToObject<MagicCardDto>())
            //    .OrderBy(x => x.MultiverseId)
            //    .Skip(0).Take(500)
            //    .AsEnumerable()
            //    .ToDictionary(x => x.MultiverseId, x => x);

            //var partialCards = allCards
            //    .OrderBy(x => x.MultiverseId)
            //    .Skip(0).Take(500)
            //    .ToList();

            //CardCollectionDto result = new CardCollectionDto()
            //{
            //    Cards = partialCards,
            //    Data = cardDataDictionary
            //};

            return await Task.FromResult(queryResults);
        }

        #endregion
    }
}
