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
        public string RMUOM { get; set; }
        public string RawMaterial { get; set; }
        public string Commodity { get; set; }
        public decimal BOMQuantity { get; set; }
        [DataType("decimal(18,4)")]
        public decimal TotalRMqty { get; set; }
        public string Scarp { get; set; }
        [DataType("decimal(18,4)")]
        public decimal ScrapQuantity { get; set; }
        [DataType("decimal(18,4)")]
        public decimal ChildPartRate { get; set; }
        [DataType("decimal(18,4)")]
        public decimal ToalCost { get; set; }
        [DataType("decimal(18,4)")]
        public decimal Scraptotalqty { get; set; }
        [DataType("decimal(18,4)")]
        public decimal TotalcostinPurCurr { get; set; }
        [DataType("decimal(18,4)")]
        public decimal ChildpartCost { get; set; }
        public string CurrencyCode { get; set; }
        [DataType("decimal(18,4)")]
        public decimal Exchangerate { get; set; }

    }
}
