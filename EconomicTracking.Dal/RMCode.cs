using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EconomicTracking.Dal
{
    class RMCode
    {
        [Key]
       
        public string RMid { get; set; }

        public string RMName { get; set; }
    }
}
