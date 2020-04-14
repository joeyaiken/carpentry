using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Carpentry.Data.DataModels;

namespace Carpentry.Data.Interfaces
{
    //This is one of the DataRepo classes that will communicate with the DB
    //TODO - Ensure methods of this interface don't return anything belonging to the Carpentry.DataModels namespace
    public interface IInventoryDataRepo
    {

        Task<InventoryCardData> GetInventoryCardById(int inventoryCardId);
        
        Task<int> AddInventoryCard(InventoryCardData card);

        Task AddInventoryCardBatch(List<InventoryCardData> cardBatch);

        Task UpdateInventoryCard(InventoryCardData card);

        Task DeleteInventoryCard(int inventoryCardId);

        //TODO - this really needs to be refactored out
        Task<CardVariantTypeData> GetCardVariantTypeByName(string name);



        //data query
        IQueryable<CardData> QueryCardDefinitions();


    }
}
