using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EconomicTracking.Dal
{
    public class SalesQtyModel
    {
        public string CustomerAssemblyId { get; set; }
        public DateTime Date { get; set; }
        public decimal Quantity { get; set; }
    }
}
