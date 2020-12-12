using Carpentry.Data.QueryParameters;
using Carpentry.Logic.Models;
using Carpentry.Logic.Search;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Carpentry.Service.Interfaces
{
    public interface ICarpentryCardSearchService
    {
        [Obsolete]
        Task<List<CardSearchResultDto>> SearchInventory(CardSearchQueryParameter filters);

        [Obsolete]
        Task<List<MagicCardDto>> SearchWeb(NameSearchQueryParameter filters);
    }
}
