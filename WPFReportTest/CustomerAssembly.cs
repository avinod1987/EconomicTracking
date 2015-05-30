using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EconomicTracking.Dal
{
    public class CustomerAssemblyModel
    {
        public int Id { get; set; }
        //public string Customer { get; set; }
        public string CustAssyNo { get; set; }
        public string CustAssyName { get; set; }
        public string LocalAssyNo { get; set; }
        public string LocalAssyName { get; set; }
        public decimal Quantity { get; set; }
        public string UOM { get; set; }
        public List<BillOfMaterialModel> BOM { get; set; }
    }
}
