using Carpentry.Data.DataModels;
using Carpentry.Data.QueryResults;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Carpentry.Data.Interfaces
{
    //All about data-objects that shouldn't need day-to-day modification
    public interface ICoreDataRepo
    {
        //TryAdd
        Task TryAddCardRarity(CardRarityData data);
        Task TryAddDeckCardCategory(DeckCardCategoryData data);
        Task TryAddInventoryCardStatus(InventoryCardStatusData data);
        Task TryAddMagicFormat(MagicFormatData data);

        //Reference Values
        //Task<List<DataReferenceValue<int>>> GetAllCardVariantTypes();
        //Task<DataReferenceValue<int>> GetCardVariantTypeByName(string name);
        Task<IEnumerable<DataReferenceValue<int>>> GetAllMagicFormats();
        Task<DataReferenceValue<int>> GetMagicFormat(string formatName);
        Task<DataReferenceValue<int>> GetMagicFormat(int formatId);
        //Task<IEnumerable<DataReferenceValue<char>>> GetAllManaColors();
        Task<IEnumerable<DataReferenceValue<char>>> GetAllRarities();
        Task<IEnumerable<DataReferenceValue<string>>> GetAllSets();
        Task<IEnumerable<DataReferenceValue<int>>> GetAllStatuses();
        //List<DataReferenceValue<string>> GetAllTypes();


        //DB check
        Task EnsureDatabaseExists();
    }
}
