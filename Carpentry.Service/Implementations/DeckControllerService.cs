using Carpentry.Service.Interfaces;
using Carpentry.Service.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Carpentry.Logic.Models;
using Carpentry.Logic.Interfaces;
using Carpentry.Data.Interfaces;
using Carpentry.Data.QueryResults;

namespace Carpentry.Service.Implementations
{
    public class DeckControllerService : IDeckControllerService
    {

        public IDataReferenceService _referenceService;

        public IDeckService _deckService;

        public DeckControllerService(
            IDataReferenceService referenceService,
            IDeckService deckService
            )
        {
            _referenceService = referenceService;
            _deckService = deckService;
        }
        public async Task<int> AddDeck(DeckPropertiesDto props)
        {
            DataReferenceValue<int> deckFormat = await _referenceService.GetMagicFormat(props.Format);

            DeckProperties deckModel = new DeckProperties()
            {
                Name = props.Name,
                FormatId = deckFormat.Id,
                Notes = props.Notes,

                BasicW = props.BasicW,
                BasicU = props.BasicU,
                BasicB = props.BasicB,
                BasicR = props.BasicR,
                BasicG = props.BasicG,
            };

            int newId = await _deckService.AddDeck(deckModel);

            return newId;
        }

        public async Task UpdateDeck(DeckPropertiesDto props)
        {
            DataReferenceValue<int> deckFormat = await _referenceService.GetMagicFormat(props.Format);

            DeckProperties deckModel = new DeckProperties()
            {
                Id = props.Id,
                Name = props.Name,
                FormatId = deckFormat.Id,
                Notes = props.Notes,

                BasicW = props.BasicW,
                BasicU = props.BasicU,
                BasicB = props.BasicB,
                BasicR = props.BasicR,
                BasicG = props.BasicG,
            };

            await _deckService.UpdateDeck(deckModel);
        }

        public async Task DeleteDeck(int deckId)
        {
            await _deckService.DeleteDeck(deckId);
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
