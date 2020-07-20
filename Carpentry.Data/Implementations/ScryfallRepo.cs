﻿using Carpentry.Data.DataContext;
using Carpentry.Data.DataModels;
using Carpentry.Data.DataModels.QueryResults;
using Carpentry.Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carpentry.Data.Implementations
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

        public async Task AddOrUpdateSet(ScryfallSetData setData, bool applyData)
        {
            //do I map or blindly add/update?
            //TODO - Map between models instead of blindly applying

            var existingSet = _scryContext.Sets.Where(x => x.Code.ToLower() == setData.Code.ToLower()).FirstOrDefault();
            //shoot does it default to null or default to a new instance???

            if (existingSet != null) 
            {
                existingSet.Code = setData.Code;
                existingSet.Name = setData.Name;
                existingSet.DataIsParsed = setData.DataIsParsed;
                existingSet.LastUpdated = setData.LastUpdated;
                existingSet.ReleasedAt = setData.ReleasedAt;

                if (applyData)
                {
                    existingSet.CardData = setData.CardData;
                }

                //_scryContext.Sets.Update(setData);
                //setData.Id = existingSet.Id;
                _scryContext.Sets.Update(existingSet);
            }
            else
            {
                //ScryfallSet newSet = new ScryfallSet
                //{

                //}
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
                    //CardData = c.CardData,
                    CardData = includeData ? c.CardData : null,
                    Code = c.Code,
                    DataIsParsed = c.DataIsParsed,
                    Digital = c.Digital,
                    FoilOnly = c.FoilOnly,
                    Id = c.Id,
                    LastUpdated = c.LastUpdated,
                    Name = c.Name,
                    NonfoilOnly = c.NonfoilOnly,
                    ReleasedAt = c.ReleasedAt,
                    SetType = c.SetType,
                })
                .FirstOrDefaultAsync();
            return set;
            
        }

        public async Task DeleteSet(int setId)
        {
            var setToDelete = _scryContext.Sets.FirstOrDefault(x => x.Id == setId);
            
            if(setToDelete != null)
            {
                _scryContext.Sets.Remove(setToDelete);
            }
            await _scryContext.SaveChangesAsync();
        }
        
        public async Task EnsureDatabaseExists()
        {
            await _scryContext.Database.EnsureCreatedAsync();
        }

        public async Task<ScryfallAuditData> GetAuditData()
        {
            var auditData = await _scryContext.ScryfallAuditData.FirstOrDefaultAsync();
            return auditData;
        }

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
