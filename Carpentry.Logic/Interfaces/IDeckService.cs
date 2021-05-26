﻿using Carpentry.Logic.Models;
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

        Task DissassembleDeck(int deckId);
        Task<int> CloneDeck(int deckId);

        Task AddDeckCard(DeckCardDto dto);
        Task AddDeckCardBatch(IEnumerable<DeckCardDto> dto);
        Task UpdateDeckCard(DeckCardDto card);
        Task DeleteDeckCard(int deckCardId);

        Task<CardTagDetailDto> GetCardTagDetails(int deckId, int cardId);
        Task AddCardTag(CardTagDto cardTag);
        Task RemoveCardTag(int cardTagId);

        Task<List<DeckOverviewDto>> GetDeckOverviews(string format = null, string sortBy = null, bool includeDissasembled = false);
        Task<DeckDetailDto> GetDeckDetail(int deckId);

        Task<string> GetDeckListExport(int deckId, string exportType);


        //Task<ValidatedDeckImportDto> ValidateDeckImport(CardImportDto dto);
        //Task AddValidatedDeckImport(ValidatedDeckImportDto validatedDto);
        //Task<string> ExportDeckList(int deckId);
    }
}
