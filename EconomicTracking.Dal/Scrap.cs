using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EconomicTracking.Dal
{
    public class Scrap
    {
        [Key]
        public int Id { get; set; }
        public string ScrapName { get; set; }
    }
}
