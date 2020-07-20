using Carpentry.Data.DataModels;
using Carpentry.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Carpentry.Data.Interfaces
{
    //This is one of the DataRepo classes that will communicate with the DB
    //TODO - Ensure methods of this interface don't return anything belonging to the Carpentry.DataModels namespace
    public interface ICardDataRepo
    {
        //Sets
        Task<List<string>> GetAllCardSetCodes();
        Task<DateTime?> GetCardSetLastUpdated(string setCode); //why isn't this a GetSetByCode ?
        Task<CardSetData> GetCardSetById(int setId);
        Task<int> AddOrUpdateCardSet(CardSetData setData); //This probably doesn't actually have to return an ID

        //Cards
        Task AddOrUpdateCardDefinition(CardDataDto cardDto);
        //Task<CardData> GetCardById(int multiverseId);
        Task<List<CardData>> GetCardsByName(string cardName);
        
        Task<CardData> GetCardData(int multiverseId);
        Task<CardData> GetCardData(string name, string code);


        Task<List<CardSetData>> GetAllCardSets();

        Task EnsureDatabaseExists();



        //Task<CardSet> GetSetByCode(string setCode);

        //











    }
}
