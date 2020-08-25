using Carpentry.Logic.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Carpentry.Logic.Interfaces
{
    public interface IDeckService
    {
        Task<int> AddDeck(DeckPropertiesDto props);
        Task AddImportedDeckBatch(List<DeckPropertiesDto> decks);
        Task UpdateDeck(DeckPropertiesDto props);
        Task DeleteDeck(int deckId);


        Task AddDeckCard(DeckCardDto dto);
        Task AddDeckCardBatch(IEnumerable<DeckCardDto> dto);
        Task UpdateDeckCard(DeckCardDto card);
        Task DeleteDeckCard(int deckCardId);


        Task<List<DeckOverviewDto>> GetDeckOverviews();
        Task<DeckDetailDto> GetDeckDetail(int deckId);


        //Task<ValidatedDeckImportDto> ValidateDeckImport(CardImportDto dto);
        //Task AddValidatedDeckImport(ValidatedDeckImportDto validatedDto);
        //Task<string> ExportDeckList(int deckId);
    }
}
