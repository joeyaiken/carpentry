using Carpentry.Data.DataContext;
using Carpentry.Data.DataModels;
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

        public async Task AddOrUpdateSet(ScryfallSet setData)
        {
            //do I map or blindly add/update?
            //TODO - Map between models instead of blindly applying

            var existingSet = _scryContext.Sets.Where(x => x.Code.ToLower() == setData.Code.ToLower()).FirstOrDefault();
            //shoot does it default to null or default to a new instance???

            if (existingSet != null) 
            {
                //existingSet.Code = setData.Code;
                //existingSet.Name = setData.Name;
                //existingSet.DataIsParsed = setData.DataIsParsed;
                //existingSet.LastUpdated = setData.LastUpdated;
                //existingSet.ReleasedAt = setData.ReleasedAt;
                //existingSet.CardData = setData.CardData;
                //_scryContext.Sets.Update(setData);
                setData.Id = existingSet.Id;
                _scryContext.Sets.Update(setData);
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

        public async Task<ScryfallSet> GetSetByCode(string setCode)
        {
            var set = await _scryContext
                .Sets
                .Where(x => x.Code.ToLower() == setCode.ToLower())
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
    }
}
