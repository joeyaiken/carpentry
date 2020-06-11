using Carpentry.Logic.Models;
using System.Threading.Tasks;

namespace Carpentry.Logic.Interfaces
{
    public interface ICardImportService
    {
        //public async ValidatedCardImportDto ValidateImport([Raw]CardImportDto)
        Task<ValidatedCardImportDto> ValidateImport(CardImportDto payload);

        //public async void AddValidatedImport(ValidatedCardImportDto)
        Task AddValidatedImport(ValidatedCardImportDto validatedPayload);
    }
}
