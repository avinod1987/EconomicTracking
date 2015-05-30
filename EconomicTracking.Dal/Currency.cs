using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EconomicTracking.Dal
{
    public class Currency
    {
        [Key]
        public int Id { get; set; }
        public string CurrencyCode { get; set; }
        public string CountryName { get; set; }
        [DataType("decimal(18,4)")]
        public decimal IndianRate { get; set; }
    }
}
