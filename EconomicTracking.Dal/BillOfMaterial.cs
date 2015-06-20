using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
        //[ForeignKey("RMCode")]
        public string RMCodeId { get; set; }
        public virtual RMCode rmcode { get; set; }
        public string RawMaterial { get; set; }
        public string Commodity { get; set; }
        public decimal BOMQuantity { get; set; }
        public decimal TotalRMqty { get; set; }
        public string Scarp { get; set; }
        
        public decimal ScrapQuantity { get; set; }
        
        public decimal ChildPartRate { get; set; }
        
        public decimal ToalCost { get; set; }
        
        public decimal Scraptotalqty { get; set; }
        
        public decimal TotalcostinPurCurr { get; set; }
        
        public decimal ChildpartCost { get; set; }
        public string CurrencyCode { get; set; }
        
        public decimal Exchangerate { get; set; }

    }
    public class RMCode
    {
        [Key]
     [DatabaseGenerated(DatabaseGeneratedOption.None)]   
        public string RMCodeId { get; set; }

        public string RMName { get; set; }

        public virtual ICollection<BillOfMaterial> billRmcode { get; set; }
    }
}
