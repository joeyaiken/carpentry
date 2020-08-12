using Carpentry.Data.QueryParameters;
using Carpentry.Logic.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Carpentry.Service.Interfaces
{
    public interface ICarpentryCardSearchService
    {
        Task<IEnumerable<MagicCardDto>> SearchInventory(InventoryQueryParameter filters);

        Task<IEnumerable<MagicCardDto>> SearchWeb(NameSearchQueryParameter filters);
    }
}
