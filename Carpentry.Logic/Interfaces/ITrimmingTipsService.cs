using Carpentry.Logic.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Carpentry.Logic.Interfaces
{
    public interface ITrimmingTipsService
    {
        Task<List<InventoryOverviewDto>> GetTrimmingTips();
        Task HideTrimmingTip(InventoryOverviewDto dto);
    }
}
