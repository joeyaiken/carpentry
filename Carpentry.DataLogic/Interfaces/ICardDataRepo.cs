using Carpentry.CarpentryData.Models;
using Carpentry.CarpentryData.Models.QueryResults;
using Carpentry.DataLogic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Carpentry.DataLogic.Interfaces
{
    public interface ICardDataRepo
    {
        //Sets
        Task<DateTime?> GetCardSetLastUpdated(string setCode); //why isn't this a GetSetByCode ?
        Task<CardSetData> GetCardSetById(int setId);
        Task<CardSetData> GetCardSetByCode(string setCode);
        Task<int> AddOrUpdateCardSet(CardSetData setData); //This probably doesn't actually have to return an ID
        Task<List<CardSetData>> GetAllCardSets();
        IQueryable<SetTotalsResult> QuerySetTotals();

        //Cards
        Task AddCardDataBatch(List<CardDataDto> cards);
        Task UpdateCardDataBatch(List<CardDataDto> cards);
        Task<List<CardData>> GetCardsByName(string cardName);
        Task<CardData> GetCardData(int cardId);
        Task<CardData> GetCardData(string name, string code);
        Task<CardData> GetCardData(string setCode, int collectorNumber);
        Task RemoveAllCardDefinitionsForSetId(int setId);
        IQueryable<CardData> QueryCardDefinitions();

        Task<Dictionary<string, MagicFormatData>> GetAllFormatsByName();
    }
}

