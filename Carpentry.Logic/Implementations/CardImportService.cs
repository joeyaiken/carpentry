using Carpentry.Logic.Interfaces;
using Carpentry.Logic.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Carpentry.Logic.Implementations
{
    public class CardImportService : ICardImportService
    {
        
        public async Task<ValidatedCardImportDto> ValidateImport(CardImportDto payload)
        {
            throw new NotImplementedException();
        }

        
        public async Task AddValidatedImport(ValidatedCardImportDto validatedPayload)
        {
            throw new NotImplementedException();
        }

    }
}
