using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EconomicTracking.Dal
{
    public class BillOfMaterialModel
    {
        public int Id { get; set; }
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
    public class OverHeadModel
    {
        public int Id { get; set; }

        public string OverHeadType { get; set; }

        public decimal OverheadINR { get; set; }

        public decimal OverheadPurrCurr { get; set; }

        public string CurrCode { get; set; }
    }
}
