using Carpentry.Data.DataContext;
using Carpentry.Data.Models;
using Carpentry.Data.QueryParameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Carpentry.Data.Interfaces
{
    //TODO - delete this
    public interface ICardRepo
    {
        //Task<CardSet> GetSetByCode(string setCode);

        Task<List<string>> GetAllCardSetCodes();

        Task<DateTime?> GetCardSetLastUpdated(string setCode);

        Task<int> AddOrUpdateCardSet(CardSet setData); //This probably doesn't actually have to return an ID

        





        //Check if the DB card exists

        //if not, add it

        //if it does, update Price and Legality info


        //Thoughts:
        //For a given card, I don't want to worry about quirks of the DB format for applying a card
        //I want to submit a DTO that represents a card definition


        Task AddOrUpdateCardDefinition(CardDataDto cardDto);
    }
}
