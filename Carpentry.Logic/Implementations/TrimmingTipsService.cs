using Carpentry.Logic.Interfaces;
using Carpentry.Logic.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Carpentry.Logic.Implementations
{
    public class TrimmingTipsService : ITrimmingTipsService
    {
        public async Task<List<InventoryOverviewDto>> GetTrimmingTips()
        {
            await Task.CompletedTask;
            throw new NotImplementedException();
        }

        public async Task HideTrimmingTip(InventoryOverviewDto dto)
        {
            await Task.CompletedTask;
            throw new NotImplementedException();
        }
    }
}
