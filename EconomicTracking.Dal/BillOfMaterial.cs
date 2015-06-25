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
        //public string CurrencyCode { get; set; }
        
        public decimal Exchangerate { get; set; }


        [ForeignKey("Scrap")]
        public string ScrapCode { get; set; }

        public virtual Scrap Scrap { get; set; }

        public string ScrpUOM { get; set; }

        [ForeignKey("Material")]
        public string MaterialCode { get; set; }
        public virtual Material Material { get; set; }

        [ForeignKey("Currency")]
        public string CurrencyCode { get; set; }

        public virtual Currency Currency { get; set; }

        //public virtual ICollection<OverHead> OverHead { get; set; }

    }
    public class RMCode
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]   
        public string RMCodeId { get; set; }

        public string RMName { get; set; }

        public virtual ICollection<BillOfMaterial> billRmcode { get; set; }
    }

    public class OverHead
    {
        [Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [ForeignKey("OverHeadCode")]
        public string OverHeadCd { get; set; }

        public virtual OverHeadCode OverHeadCode { get; set; }

        //public string overheadtype { get; set; }

        public decimal overheadinsetcur { get; set; }

        [ForeignKey("Currency")]
        public string CurrencyCode { get; set; }

        public virtual Currency Currency { get; set; }

        //public string currency { get; set; }

        public decimal overheadINR { get; set; }

        public decimal Exchangerate { get; set; }
        //[NotMapped]
        public virtual CustomerAssembly CustomerAssembly { get; set; }

    }

    public class OverHeadCode
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string OverHeadCd { get; set; }

        public string overheadtype { get; set; }
    }

}
