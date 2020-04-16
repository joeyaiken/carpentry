using Carpentry.Data.DataModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Carpentry.Data.Interfaces
{
    public interface IDataReferenceService
    {
        Task<MagicFormatData> GetMagicFormat(string formatName);
        Task<MagicFormatData> GetMagicFormat(int formatId);
        Task<IEnumerable<MagicFormatData>> GetAllMagicFormats();
        Task<CardVariantTypeData> GetCardVariantTypeByName(string name);
        Task<List<CardVariantTypeData>> GetAllCardVariantTypes();
    }
}
