using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EconomicTracking.Dal
{
   public class MasterLogin
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string MasterLoginUserName { get; set; }

        public string MasterLoginPassword { get; set; }
    }
}
