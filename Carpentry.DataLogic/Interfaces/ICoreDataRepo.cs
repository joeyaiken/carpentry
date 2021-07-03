using Carpentry.CarpentryData.Models;
using Carpentry.DataLogic.QueryResults;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Carpentry.DataLogic.Interfaces
{
    //All about data-objects that shouldn't need day-to-day modification
    [Obsolete]
    public interface ICoreDataRepo
    {
        //TryAdd
        [Obsolete]
        Task TryAddCardRarity(CardRarityData data);
        [Obsolete]
        Task TryAddDeckCardCategory(DeckCardCategoryData data);
        [Obsolete]
        Task TryAddInventoryCardStatus(InventoryCardStatusData data);
        [Obsolete]
        Task TryAddMagicFormat(MagicFormatData data);

        //Reference Values
        //Task<List<DataReferenceValue<int>>> GetAllCardVariantTypes();
        //Task<DataReferenceValue<int>> GetCardVariantTypeByName(string name);
        [Obsolete]
        Task<IEnumerable<DataReferenceValue<int>>> GetAllMagicFormats();
        [Obsolete]
        Task<DataReferenceValue<int>> GetMagicFormat(string formatName);
        [Obsolete]
        Task<DataReferenceValue<int>> GetMagicFormat(int formatId);
        //Task<IEnumerable<DataReferenceValue<char>>> GetAllManaColors();
        [Obsolete]
        Task<IEnumerable<DataReferenceValue<char>>> GetAllRarities();
        [Obsolete]
        Task<IEnumerable<DataReferenceValue<string>>> GetAllSets();
        [Obsolete]
        Task<IEnumerable<DataReferenceValue<int>>> GetAllStatuses();
        //List<DataReferenceValue<string>> GetAllTypes();


        //DB check
        [Obsolete]
        Task EnsureDatabaseExists();
    }
}
