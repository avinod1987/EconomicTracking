using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//public string SettlementRef{get;set;}


namespace EconomicTracking.Dal
{
    public class BillOfMaterial
    {
        [Key]
        public int Id { get; set; }
        public string CustAssyNo { get; set; }
        public string CustomerPartNo { get; set; }
        public string CustomerPartName { get; set; }
        public string LocalPartNo { get; set; }
        public string LocalPartName { get; set; }
        public int Quantity { get; set; }
        public string UOM { get; set; }
        public string RawMaterial { get; set; }
        public string Commodity { get; set; }
        public decimal BOMQuantity { get; set; }
        public string Scarp { get; set; }
        public decimal ScrapQuantity { get; set; }
        public decimal ChildPartRate { get; set; }
        public decimal ToalCost { get; set; }
        public string CurrencyCode { get; set; }
    }
}
