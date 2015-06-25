using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EconomicTracking.Dal
{
    public class Material
    {
        [Key]
        //public int Id { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string MaterialCode { get; set; }
        public string MaterialName { get; set; }
        [NotMapped]
        public virtual string BillOfMaterial_Id { get; set; }
    }
}
