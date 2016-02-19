using System;
using System.Collections.Generic;

namespace PrismContext.Desktop.Data.Models
{
    public interface INestedData
    {
        String Name { get; set; }
        IEnumerable<NestedData> SubDatas { get; set; }
    }

    public class NestedData : INestedData
    {
        public String Name { get; set; }

        public IEnumerable<NestedData> SubDatas { get; set; } 
    }
}
