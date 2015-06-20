using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EconomicTracking.Dal
{
    public class Settlement
    {
        [Key]
        public int Id { get; set; }
        public string SettlementRef { get; set; }
        public string CustomerId { get; set; }
        public string CustomerName { get; set; }
        public DateTime SettlementFrom { get; set; }
        public DateTime SettlementTo { get; set; }

        public List<SettlementCommodity> Commodity { get; set; }
        public List<SettlementScarp> Scarp { get; set; }
        public List<SettlementCurrency> Currency { get; set; }

    }
    public class SettlementCommodity
    {
        
        [Key]
        public int Id { get; set; }
        public string MaterialName { get; set; }
        public decimal Rate { get; set; }
         [NotMapped]
        public string Settlement_Id { get; set; }
    }
    public class SettlementScarp
    {
        [Key]
        public int Id { get; set; }
        public string ScrapName { get; set; }
        public decimal Rate { get; set; }
         [NotMapped]
        public string Settlement_Id { get; set; }
    }
    public class SettlementCurrency
    {
        [Key]
        public int Id { get; set; }
        public string CurrencyCode { get; set; }
        public decimal Rate { get; set; }
         [NotMapped]
        public string Settlement_Id { get; set; }
    }
}
