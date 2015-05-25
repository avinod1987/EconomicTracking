using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EconomicTracking.Dal
{
    public class Material
    {
        [Key]
        public int Id { get; set; }
        public string MaterialName { get; set; }
    }
}
