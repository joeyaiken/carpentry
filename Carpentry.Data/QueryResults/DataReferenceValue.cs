using System;
using System.Collections.Generic;
using System.Text;

namespace Carpentry.Data.QueryResults
{
    public class DataReferenceValue<T>
    {

        public T Id { get; set; }

        public string Name { get; set; } //Name | Value | Key
    }
}
