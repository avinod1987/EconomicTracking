using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EconomicTracking.Dal
{
    public class CustomerAssembly
    {
        [Key]
        public int Id { get; set; }
        public string CustAssyNo { get; set; }
        public string CustAssyName { get; set; }
        public string LocalAssyNo { get; set; }
        public string LocalAssyName { get; set; }
        public decimal Quantity { get; set; }
        public List<BillOfMaterial> BOM { get; set; }
    }
}
