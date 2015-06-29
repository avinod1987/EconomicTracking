using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace EconomicTracking.Dal
{
   public class ApplicationPassword
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string AppPass { get; set; }
    }
}
