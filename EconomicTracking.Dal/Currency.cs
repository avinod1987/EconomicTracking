using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EconomicTracking.Dal
{
    public class Currency
    {
        [Key]
        //public int Id { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string CurrencyCode { get; set; }
        public string CurrencyName { get; set; }
        public string CountryName { get; set; }
        public decimal IndianRate { get; set; }
    }
}
