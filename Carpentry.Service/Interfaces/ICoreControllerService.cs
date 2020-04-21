using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Carpentry.Service.Models;

namespace Carpentry.Service.Interfaces
{
    //Maybe this can also be what the TOOL apps calls??
    public interface ICoreControllerService
    {
        Task<AppFiltersDto> GetAppFilterValues();
    }
}
