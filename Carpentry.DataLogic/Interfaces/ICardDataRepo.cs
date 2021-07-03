using Carpentry.CarpentryData.Models;
using Carpentry.CarpentryData.Models.QueryResults;
using Carpentry.DataLogic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Carpentry.DataLogic.Interfaces
{
    [Obsolete]
    public interface ICardDataRepo
    {
        //Sets
        [Obsolete]
        Task<DateTime?> GetCardSetLastUpdated(string setCode); //why isn't this a GetSetByCode ?
        [Obsolete]
        Task<CardSetData> GetCardSetById(int setId);
        [Obsolete]
        Task<CardSetData> GetCardSetByCode(string setCode);
        [Obsolete]
        Task<int> AddOrUpdateCardSet(CardSetData setData); //This probably doesn't actually have to return an ID
        [Obsolete]
        Task<List<CardSetData>> GetAllCardSets();
        [Obsolete]
        IQueryable<SetTotalsResult> QuerySetTotals();

        //Cards
        [Obsolete]
        Task AddCardDataBatch(List<CardDataDto> cards);
        [Obsolete]
        Task UpdateCardDataBatch(List<CardDataDto> cards);
        [Obsolete]
        Task<List<CardData>> GetCardsByName(string cardName);
        [Obsolete]
        Task<CardData> GetCardData(int cardId);
        [Obsolete]
        Task<CardData> GetCardData(string name, string code);
        [Obsolete]
        Task<CardData> GetCardData(string setCode, int collectorNumber);
        [Obsolete]
        Task RemoveAllCardDefinitionsForSetId(int setId);
        [Obsolete]
        IQueryable<CardData> QueryCardDefinitions();

        [Obsolete]
        Task<Dictionary<string, MagicFormatData>> GetAllFormatsByName();
    }
}

