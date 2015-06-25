using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EconomicTracking.Dal
{
    public class Scrap
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string ScrapCode { get; set; }
        public string ScrapName { get; set; }

        [NotMapped]
        public virtual string BillOfMaterial_Id { get; set; }

    }
}
