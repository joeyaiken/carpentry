using Carpentry.CarpentryData;
using Carpentry.CarpentryData.Models;
using Carpentry.DataLogic.QueryResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Carpentry.DataLogic
{

    [Obsolete]
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

        public async Task<DeckData> GetDeckById(int deckId)
        {
            var matchingDeck = await _cardContext.Decks.Where(d => d.DeckId == deckId)
                .Include(d => d.Format)
                .Include(d => d.Cards)
                .FirstOrDefaultAsync();

            return matchingDeck;
        }

        #endregion

        #region Queries

        
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

        #endregion
    }
}
