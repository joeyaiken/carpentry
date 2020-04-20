using Carpentry.Service.Interfaces;
using Carpentry.Service.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Carpentry.Service.Implementations
{
    public class DeckService : IDeckService
    {
        public async Task<int> AddDeck(DeckPropertiesDto props)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateDeck(DeckPropertiesDto props)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteDeck(int deckId)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<DeckPropertiesDto>> GetDeckOverviews()
        {
            throw new NotImplementedException();
        }

        public async Task<DeckDetailDto> GetDeckDetail(int deckId)
        {
            throw new NotImplementedException();
        }

        public async Task AddDeckCard(DeckCardDto dto)
        {
            throw new NotImplementedException();
        }

        public async Task AddDeckCardBatch(IEnumerable<DeckCardDto> dto)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateDeckCard(DeckCardDto card)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteDeckCard(int deckCardId)
        {
            throw new NotImplementedException();
        }
    }
}
