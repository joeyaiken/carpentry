using Carpentry.Logic.Models;
using System.Threading.Tasks;

namespace Carpentry.Logic.Interfaces
{
    public interface IDataImportService
    {
        //public async ValidatedCardImportDto ValidateImport([Raw]CardImportDto)
        //public async void AddValidatedImport(ValidatedCardImportDto)

        //deck import
        Task<ValidatedDeckImportDto> ValidateDeckImport(CardImportDto payload);
        Task<int> AddValidatedDeckImport(ValidatedDeckImportDto validatedPayload);

        //inventory import
        Task<ValidatedCarpentryImportDto> ValidateCarpentryImport(CardImportDto payload);
        Task AddValidatedCarpentryImport(ValidatedCarpentryImportDto payload);
    }
}
