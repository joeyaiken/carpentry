using Carpentry.Logic.Models;
using System.Threading.Tasks;

namespace Carpentry.Logic.Interfaces
{
    public interface ICardImportService
    {
        //public async ValidatedCardImportDto ValidateImport([Raw]CardImportDto)
        Task<ValidatedArenaImportDto> ValidateArenaImport(CardImportDto payload);

        Task<ValidatedCarpentryImportDto> ValidateCarpentryImport(CardImportDto payload);

        //public async void AddValidatedImport(ValidatedCardImportDto)
        Task AddValidatedArenaImport(ValidatedArenaImportDto validatedPayload);

        Task AddValidatedCarpentryImport(ValidatedCarpentryImportDto payload);





    }
}
