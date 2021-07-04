using Carpentry.DataLogic.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Carpentry.ScryfallData;
using Carpentry.ScryfallData.Models;
using Carpentry.ScryfallData.Models.QueryResults;

namespace Carpentry.DataLogic.Implementations
{
    public class ScryfallRepo : IScryfallDataRepo
    {
        private readonly ScryfallDataContext _scryContext;

        public ScryfallRepo(
            ScryfallDataContext scryContext
            )
        {
            _scryContext = scryContext;
        }

        public async Task<DateTime?> GetSetDataLastUpdated(string setCode)
        {
            var setLastUpdated = await _scryContext
                .Sets
                .Where(x => x.Code.ToLower() == setCode.ToLower())
                .Select(x => x.LastUpdated)
                .FirstOrDefaultAsync();
            return setLastUpdated;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="setData"></param>
        /// <param name="applyData">
        /// Called with "false" if I only want to update a set definition without clearing out data
        /// In reality, I could probably ignore sets that already exist, but maybe Scryfall will want to update something, idk
        /// </param>
        /// <returns></returns>
        public async Task AddOrUpdateSet(ScryfallSetData setData, bool applyData)
        {
            var existingSet = _scryContext.Sets.Where(x => x.Code.ToLower() == setData.Code.ToLower()).FirstOrDefault();
            if (existingSet != null) 
            {
                existingSet.Code = setData.Code;
                existingSet.Name = setData.Name;
                existingSet.LastUpdated = setData.LastUpdated;
                existingSet.ReleasedAt = setData.ReleasedAt;
                
                if (applyData)
                {
                    existingSet.CardTokens = setData.CardTokens;
                    existingSet.SetCards = setData.SetCards;
                    existingSet.PremiumCards = setData.PremiumCards;
                }

                _scryContext.Sets.Update(existingSet);
            }
            else
            {
                _scryContext.Sets.Add(setData);
            }

            await _scryContext.SaveChangesAsync();
        }

        public async Task<ScryfallSetData> GetSetByCode(string setCode, bool includeData)
        {
            var set = await _scryContext
                .Sets
                .Where(x => x.Code.ToLower() == setCode.ToLower())
                .Select(c => new ScryfallSetData
                {
                    CardCount = c.CardCount,
                    Code = c.Code,
                    Digital = c.Digital,
                    FoilOnly = c.FoilOnly,
                    Id = c.Id,
                    LastUpdated = c.LastUpdated,
                    Name = c.Name,
                    NonfoilOnly = c.NonfoilOnly,
                    ReleasedAt = c.ReleasedAt,
                    SetType = c.SetType,
                    
                    CardTokens = includeData ? c.CardTokens : null,
                    SetCards = includeData ? c.SetCards : null,
                    PremiumCards = includeData ? c.PremiumCards : null,
                })
                .FirstOrDefaultAsync();
            return set;
            
        }

        //public async Task DeleteSet(int setId)
        //{
        //    var setToDelete = _scryContext.Sets.FirstOrDefault(x => x.Id == setId);
            
        //    if(setToDelete != null)
        //    {
        //        _scryContext.Sets.Remove(setToDelete);
        //    }
        //    await _scryContext.SaveChangesAsync();
        //}
        
        public async Task EnsureDatabaseExists()
        {
            await _scryContext.Database.EnsureCreatedAsync();
        }

        /// <summary>
        /// Gets the current ScryfallAuditData information, if it exists
        /// </summary>
        /// <returns></returns>
        public async Task<ScryfallAuditData> GetAuditData()
        {
            var auditData = await _scryContext.ScryfallAuditData.FirstOrDefaultAsync();
            return auditData;
        }

        /// <summary>
        /// Gets the current ScryfallAuditData record (creating one if it doesn't exist), and updates the LastUpdated value
        /// </summary>
        /// <returns></returns>
        public async Task SetAuditData()
        {
            var existingAuditData = await _scryContext.ScryfallAuditData.FirstOrDefaultAsync();

            if(existingAuditData == null)
            {
                var newAuditData = new ScryfallAuditData() { DefinitionsLastUpdated = DateTime.Now };
                _scryContext.ScryfallAuditData.Add(newAuditData);
            }
            else
            {
                existingAuditData.DefinitionsLastUpdated = DateTime.Now;
                _scryContext.ScryfallAuditData.Update(existingAuditData);
            }
            await _scryContext.SaveChangesAsync();
        }

        public async Task<List<ScryfallSetOverview>> GetAvailableSetOverviews()
        {
            var result = await _scryContext.Sets
                .OrderByDescending(x => x.ReleasedAt)
                .Select(x => new ScryfallSetOverview
                {
                    Code = x.Code,
                    Name = x.Name,
                    LastUpdated = x.LastUpdated,
                    CardCount = x.CardCount ?? 0,
                    Digital = x.Digital ?? false,
                    FoilOnly = x.FoilOnly ?? false,
                    NonfoilOnly = x.NonfoilOnly ?? false,
                    SetType = x.SetType,
                    ReleasedAt = x.ReleasedAt,
                })
                .ToListAsync();
            return result;
        }
    }
}
