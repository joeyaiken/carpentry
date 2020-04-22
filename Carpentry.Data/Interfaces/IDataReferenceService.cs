using Carpentry.Data.DataModels;
using Carpentry.Data.QueryResults;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Carpentry.Data.Interfaces
{
    public interface IDataReferenceService
    {
        Task<DataReferenceValue<int>> GetMagicFormat(string formatName);
        Task<DataReferenceValue<int>> GetMagicFormat(int formatId);
        Task<IEnumerable<DataReferenceValue<int>>> GetAllMagicFormats();
        Task<DataReferenceValue<int>> GetCardVariantTypeByName(string name);
        Task<List<DataReferenceValue<int>>> GetAllCardVariantTypes();

        Task<IEnumerable<DataReferenceValue<char>>> GetAllManaColors();
        Task<IEnumerable<DataReferenceValue<char>>> GetAllRarities();
        Task<IEnumerable<DataReferenceValue<int>>> GetAllSets();
        Task<IEnumerable<DataReferenceValue<int>>> GetAllStatuses();
        List<DataReferenceValue<string>> GetAllTypes();


        //Task<DataReferenceValue<int>> GetINTThings();
    }
}
