using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace EconomicTracking.Dal
{
   public class UserLogin
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string UserLoginUserName { get; set; }

        public string UserLoginPassword { get; set; }
    }
}
