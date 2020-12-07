using System.Collections.Generic;

#nullable disable

namespace danielg_projectOne.DataModel
{
    public partial class Product
    {
        public Product()
        {
            AggInventories = new HashSet<AggInventory>();
            AggOrders = new HashSet<AggOrder>();
        }

        public string Name { get; set; }
        public decimal? Price { get; set; }

        public virtual ICollection<AggInventory> AggInventories { get; set; }
        public virtual ICollection<AggOrder> AggOrders { get; set; }
    }
}
