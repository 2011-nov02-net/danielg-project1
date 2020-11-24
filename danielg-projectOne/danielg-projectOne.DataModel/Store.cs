using System;
using System.Collections.Generic;

#nullable disable

namespace danielg_projectOne.DataModel
{
    public partial class Store
    {
        public Store()
        {
            AggInventories = new HashSet<AggInventory>();
            GenOrders = new HashSet<GenOrder>();
        }

        public int Id { get; set; }
        public string Location { get; set; }

        public virtual ICollection<AggInventory> AggInventories { get; set; }
        public virtual ICollection<GenOrder> GenOrders { get; set; }
    }
}
