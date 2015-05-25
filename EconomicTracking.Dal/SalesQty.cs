using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//public string Customer{get;set;}
namespace EconomicTracking.Dal
{
    public class SalesQty
    {
        [Key]
        public int Id { get; set; }
        public string CustomerAssemblyId { get; set; }
        public DateTime Date { get; set; }
        public decimal Quantity { get; set; }
    }
}
