using Carpentry.Data.DataModels;
using Carpentry.Data.DataModels.QueryResults;
using Carpentry.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carpentry.Data.Interfaces
{
    //This is one of the DataRepo classes that will communicate with the DB
    //TODO - Ensure methods of this interface don't return anything belonging to the Carpentry.DataModels namespace
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
        Task<CardData> GetCardData(int multiverseId);
        Task<CardData> GetCardData(string name, string code);
        Task RemoveAllCardDefinitionsForSetId(int setId);
    }
}
