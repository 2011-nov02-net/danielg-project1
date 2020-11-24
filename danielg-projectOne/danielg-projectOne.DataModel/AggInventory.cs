using System;
using System.Collections.Generic;

#nullable disable

namespace danielg_projectOne.DataModel
{
    public partial class AggInventory
    {
        public int StoreId { get; set; }
        public string Product { get; set; }
        public int InStock { get; set; }

        public virtual Product ProductNavigation { get; set; }
        public virtual Store Store { get; set; }
    }
}
