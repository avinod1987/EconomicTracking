using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EconomicTracking.Dal
{
    public class FinalReport
    {
        [Key]
        public int Id { get; set; }
        public string CustomerAssyId { get; set; }
        public string QuoteReference { get; set; }
        public DateTime InitialQuoteDate { get; set; }
        public DateTime LastQuoteDate { get; set; }
        public string LastSettlementRef { get; set; }
        public string CurrentSettlementRef { get; set; }
        public string FromPeriod { get; set; }
        public string ToPeriod { get; set; }
        public decimal EffectiveSalesQuantity { get; set; }
        public decimal Recovery { get; set; }
        public decimal TotalRecovery { get; set; }
    }
}
