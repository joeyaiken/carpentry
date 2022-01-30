using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Carpentry.CarpentryData;
using Carpentry.Logic.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Carpentry.Logic
{
    public class TrimmingToolSetTotal
    {
        public int SetId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int TotalCount { get; set; }
    }

    public class TrimmingToolStatusTotal
    {
        public int StatusId { get; set; }
        public string StatusName { get; set; }
        public int TotalCount { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal PricePerCard { get; set; }
    }

    public class TrimmingToolOverview
    {
        public List<TrimmingToolSetTotal> SetTotals { get; set; }
        public TrimmingToolStatusTotal InventoryTotal { get; set; }
        public TrimmingToolStatusTotal TrimmedTrimmingToolsTotal { get; set; }
    }

    public class TrimmedCard
    {
        
    }
    
    public interface ITrimmingToolService
    {
        /// <summary>
        /// Gets a dto containing a list of set totals, and summary info of current trimmed cards
        /// </summary>
        /// <returns></returns>
        Task<TrimmingToolOverview> GetTrimmingToolOverview();
        
        /// <summary>
        /// Gets a list of cards that can be trimmed
        /// </summary>
        Task<List<TrimmingToolSearchResult>> GetTrimmingToolCards(TrimmingToolRequest request);

        /// <summary>
        /// Trims a batch of cards
        /// Trimmed cards are just moved to the sell list
        /// </summary>
        Task TrimCards(List<TrimmedCardDto> cardsToTrim);
        
        /// <summary>
        /// Gets a list of current trimmed cards
        /// </summary>
        Task<IEnumerable<TrimmedCard>> GetCurrentTrimmedCards();

        /// <summary>
        /// Removes a card from trimmed cards (back to inventory)
        /// </summary>
        Task RestoreTrimmedCard(int inventoryCardId);
        
        /// <summary>
        /// Gets an export of current trimmed cards as a string
        /// </summary>
        Task<string> GetTrimmedCardsExport();

        /// <summary>
        /// Deletes all trimmed cards currently on the sell list
        /// </summary>
        Task DeleteCurrentTrimmedCards();
    }
    
    public class TrimmingToolService : ITrimmingToolService
    {
        private readonly CarpentryDataContext _cardContext;
        private readonly ILogger<InventoryService> _logger;

        public TrimmingToolService(CarpentryDataContext cardContext, ILogger<InventoryService> logger)
        {
            _cardContext = cardContext;
            _logger = logger;
        }
        
        /// <summary>
        /// Gets a dto containing a list of set totals, and summary info of current trimmed cards
        /// </summary>
        /// <returns></returns>
        public async Task<TrimmingToolOverview> GetTrimmingToolOverview()
        {
            var result = new TrimmingToolOverview()
            {
                SetTotals = new List<TrimmingToolSetTotal>()
                {
                    new TrimmingToolSetTotal()
                    {
                        
                    },
                    new TrimmingToolSetTotal()
                    {
                        
                    },
                    new TrimmingToolSetTotal()
                    {
                        
                    },
                },
                InventoryTotal = new TrimmingToolStatusTotal()
                {
                    
                },
                TrimmedTrimmingToolsTotal = new TrimmingToolStatusTotal()
                {
                    
                }
            };
            return result;
        }
        
        /// <summary>
        /// Gets a list of cards that can be trimmed
        /// </summary>
        public async Task<List<TrimmingToolSearchResult>> GetTrimmingToolCards(TrimmingToolRequest request)
        {
            //need to replace this with something that doesn't depend on views
            var query = from uniqueCard in _cardContext.InventoryCardByUnique
                        join namedCard in _cardContext.InventoryCardByName
                        on uniqueCard.Name equals namedCard.Name
                        where uniqueCard.SetCode == request.SetCode
                        select new { ByUnique = uniqueCard, ByName = namedCard };

            if (!string.IsNullOrEmpty(request.SearchGroup))
            {
                switch (request.SearchGroup)
                {
                    case "Red":
                        query = query.Where(x => x.ByUnique.ColorIdentity == "R" && (x.ByUnique.RarityId == 'C' || x.ByUnique.RarityId == 'U'));
                        break;
                    case "Blue":
                        query = query.Where(x => x.ByUnique.ColorIdentity == "U" && (x.ByUnique.RarityId == 'C' || x.ByUnique.RarityId == 'U'));
                        break;
                    case "Green":
                        query = query.Where(x => x.ByUnique.ColorIdentity == "G" && (x.ByUnique.RarityId == 'C' || x.ByUnique.RarityId == 'U'));
                        break;
                    case "White":
                        query = query.Where(x => x.ByUnique.ColorIdentity == "W" && (x.ByUnique.RarityId == 'C' || x.ByUnique.RarityId == 'U'));
                        break;
                    case "Black":
                        query = query.Where(x => x.ByUnique.ColorIdentity == "B" && (x.ByUnique.RarityId == 'C' || x.ByUnique.RarityId == 'U'));
                        break;
                    case "Multicolored":
                        query = query.Where(x => x.ByUnique.ColorIdentity.Length > 1 && (x.ByUnique.RarityId == 'C' || x.ByUnique.RarityId == 'U'));
                        break;
                    case "Colorless":
                        query = query.Where(x => x.ByUnique.ColorIdentity.Length == 0 && (x.ByUnique.RarityId == 'C' || x.ByUnique.RarityId == 'U'));
                        break;
                    case "Lands":
                        query = query.Where(x => x.ByUnique.Type.Contains("Land") && (x.ByUnique.RarityId == 'C' || x.ByUnique.RarityId == 'U'));// && !x.Type.Contains()
                        break;
                    case "RareMythic":
                        query = query.Where(x => x.ByUnique.RarityId == 'R' || x.ByUnique.RarityId == 'M');
                        break;
                }
            }

            if (request.MaxPrice > 0)
            {
                //May be an issue if price manages to still be null
                //  might need (decimal?)c.ByUnique.Price ?? 0 <= request.MaxPrice
                query = query.Where(c => (decimal)c.ByUnique.Price <= request.MaxPrice);
            }
            
            switch (request.FilterBy)
            {
                case "inventory":
                    query = query.Where(c => c.ByUnique.InventoryCount >= request.MinCount);
                    break;

                case "owned":
                    query = query.Where(c => (c.ByUnique.InventoryCount + c.ByUnique.DeckCount) >= request.MinCount);
                    break;

                case "total":
                    query = query.Where(c => (c.ByName.InventoryCount + c.ByName.DeckCount) >= request.MinCount);
                    break;
            }

            var queryResult = await query.Select(c => new TrimmingToolQueryResult
            {
                Id = c.ByUnique.Id,
                CardId = c.ByUnique.CardId,
                SetCode = c.ByUnique.SetCode,
                Name = c.ByUnique.Name,
                ImageUrl = c.ByUnique.ImageUrl,
                CollectorNumberStr = c.ByUnique.CollectorNumberStr,
                IsFoil = c.ByUnique.IsFoil,
                Price = (decimal?)c.ByUnique.Price,
                PrintDeckCount = c.ByUnique.DeckCount,
                PrintInventoryCount = c.ByUnique.InventoryCount,
                AllDeckCount = c.ByName.DeckCount,
                AllInventoryCount = c.ByName.InventoryCount,
            }).ToListAsync();
            
            return queryResult.Select(c => new TrimmingToolSearchResult()
            { 
                Id = c.CardId + (c.IsFoil ?? false ? "f":""),
                CardId = c.CardId,
                Name = c.Name,
                IsFoil = c.IsFoil,
                PrintDisplay = $"{c.SetCode} {c.CollectorNumberStr}{(c.IsFoil!.Value ? " (FOIL)":"")}",
                Price = c.Price,
                UnusedCount = c.PrintInventoryCount,
                TotalCount = c.PrintDeckCount + c.PrintInventoryCount,
                AllPrintsCount = c.AllDeckCount + c.AllInventoryCount,
                //TODO - Consider using different logic when not filtering by Inventory
                RecommendedTrimCount = c.PrintInventoryCount - request.MinCount + 1,
                ImageUrl = c.ImageUrl,
            }).ToList();
        }

        /// <summary>
        /// Trims a batch of cards
        /// Trimmed cards are just moved to the sell list
        /// </summary>
        public async Task TrimCards(List<TrimmedCardDto> cardsToTrim)
        {
            //for each card, need to get all matching inventory cards

            //get all ids
            var allIds = cardsToTrim.Select(c => c.CardId).ToArray();

            //get a dictionary of all inventory cards with a card id contained in a provided list
            var relevantCardsById = (await _cardContext.InventoryCards
                    .Where(ic => ic.InventoryCardStatusId == 1 && ic.DeckCards.Count == 0 && allIds.Contains(ic.CardId))
                    .ToListAsync())
                .GroupBy(ic => ic.CardId)
                .ToDictionary(g => g.Key, g => g.ToList());

            foreach(var card in cardsToTrim)
            {
                //  Get matching card by id/foil, ensure enough inventory cards exist to meet the request, update the cards along the way
                var relevantInventoryCards = relevantCardsById[card.CardId].Where(c => c.IsFoil == card.IsFoil).ToArray();

                if(relevantInventoryCards.Length < card.NumberToTrim)
                {
                    //Should never attempt to trim more cards than exist.  Attempting to do so is a red flag
                    throw new Exception($"Not enough unused cards to trim. Card Name: {card.CardName}, Number to trim: {card.NumberToTrim}, Available: {relevantInventoryCards.Length}");
                }

                for(var i = 0; i < card.NumberToTrim; i++)
                {
                    var cardToUpdate = relevantInventoryCards[i];
                    cardToUpdate.InventoryCardStatusId = 3;
                    _cardContext.InventoryCards.Update(cardToUpdate);
                }
            }

            await _cardContext.SaveChangesAsync();
        }
        
        /// <summary>
        /// Gets a list of current trimmed cards
        /// </summary>
        public async Task<IEnumerable<TrimmedCard>> GetCurrentTrimmedCards()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Removes a card from trimmed cards (back to inventory)
        /// </summary>
        public Task RestoreTrimmedCard(int inventoryCardId)
        {
            throw new NotImplementedException();
        }
        
        /// <summary>
        /// Gets an export of current trimmed cards as a string
        /// </summary>
        public async Task<string> GetTrimmedCardsExport()
        {
            throw new NotImplementedException();
        }
        
        /// <summary>
        /// Deletes all trimmed cards currently on the sell list
        /// </summary>
        public async Task DeleteCurrentTrimmedCards()
        {
            throw new NotImplementedException();
        }
    }
}