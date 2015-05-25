using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EconomicTracking.Dal
{
    class overheads
    {
        [Key]
        public int Id { get; set; }

        public string overheadtype { get; set; }

        public decimal overheadinsetcur { get; set; }

        public string currency { get; set; }

        public decimal overheadINR { get; set; }

    }
}
