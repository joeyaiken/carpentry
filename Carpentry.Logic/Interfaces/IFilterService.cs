using Carpentry.Logic.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Carpentry.Logic.Interfaces
{
    public interface IFilterService
    {
        Task<AppFiltersDto> GetAppFilterValues();
    }
}
