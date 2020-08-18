using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Carpentry.Data.DataModels;
using Carpentry.Data.DataModels.QueryResults;
using Carpentry.Data.Models;
using Carpentry.Data.QueryParameters;
using Carpentry.Data.QueryResults;

namespace Carpentry.Data.Interfaces
{
    //This is one of the DataRepo classes that will communicate with the DB
    public interface IInventoryDataRepo
    {
        Task<int> AddInventoryCard(InventoryCardData card);
        Task AddInventoryCardBatch(List<InventoryCardData> cardBatch);
        Task UpdateInventoryCard(InventoryCardData card);
        Task DeleteInventoryCard(int inventoryCardId);
        //Task<InventoryCardData> GetInventoryCardById(int inventoryCardId);
        Task<InventoryCardData> GetInventoryCard(int inventoryCardId);
        //Task<InventoryCardData> GetInventoryCard(string setCode, int collectorNumber);


        Task<bool> DoInventoryCardsExist(); //??



        //Task<IEnumerable<CardOverviewResult>> GetInventoryOverviews(InventoryQueryParameter param);
        Task<IEnumerable<InventoryCardResult>> GetInventoryCardsByName(string cardName);


        Task<IEnumerable<CardDataDto>> SearchInventoryCards(InventoryQueryParameter filters);


        Task<IEnumerable<CardDataDto>> SearchCardSet(CardSearchQueryParameter filters);//remove dis?




        IQueryable<InventoryCardByNameResult> QueryCardsByName();
        IQueryable<InventoryCardByPrintResult> QueryCardsByPrint();
        IQueryable<InventoryCardByUniqueResult> QueryCardsByUnique();

    }
}
