using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EconomicTracking.Dal
{
    public class SettlementModel
    {
        public int Id { get; set; }
        public string SettlementRef { get; set; }
        public string CustomerId { get; set; }
        public string CustomerName { get; set; }
        public DateTime SettlementFrom { get; set; }
        public DateTime SettlementTo { get; set; }
        public decimal CommodityTotal { get; set; }
        public decimal ScrapTotal { get; set; }
        public List<SettlementCommodityModel> Commodity { get; set; }
        public List<SettlementScarpModel> Scarp { get; set; }
        public List<SettlementCurrencyModel> Currency { get; set; }

    }
    public class SettlementCommodityModel
    {
        public int Id { get; set; }
        public string MaterialName { get; set; }
        public decimal Rate { get; set; }
        public string Settlement_Id { get; set; }
    }
    public class SettlementScarpModel
    {
        public int Id { get; set; }
        public string ScrapName { get; set; }
        public decimal Rate { get; set; }
        public string Settlement_Id { get; set; }
    }
    public class SettlementCurrencyModel
    {
        public int Id { get; set; }
        public string CurrencyCode { get; set; }
        public decimal Rate { get; set; }
        public string Settlement_Id { get; set; }
    }
}
