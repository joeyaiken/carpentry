using System;
using System.Collections.Generic;

namespace Carpentry.Models
{
    public class NormalizedList<T>
    {
        public NormalizedList() { }
        
        public NormalizedList(List<T> props, Func<T, int> selector)
        {
            ById = new Dictionary<int, T>();
            AllIds = new List<int>();
            
            props.ForEach(prop =>
            {
                var key = selector(prop);
                AllIds.Add(key);
                ById[key] = prop;
            });
        }
        
        public Dictionary<int, T> ById { get; set; }
        
        public List<int> AllIds { get; set; }
    }
}