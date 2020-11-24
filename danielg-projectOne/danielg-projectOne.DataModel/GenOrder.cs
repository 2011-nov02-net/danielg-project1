using System;
using System.Collections.Generic;

#nullable disable

namespace danielg_projectOne.DataModel
{
    public partial class GenOrder
    {
        public GenOrder()
        {
            AggOrders = new HashSet<AggOrder>();
        }

        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int StoreId { get; set; }
        public decimal? Cost { get; set; }
        public DateTime Date { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual Store Store { get; set; }
        public virtual ICollection<AggOrder> AggOrders { get; set; }
    }
}
