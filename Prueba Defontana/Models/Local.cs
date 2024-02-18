using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Prueba_Defontana.Models
{
    [Table("Local")]
    public partial class Local
    {
        public Local()
        {
            Venta = new HashSet<Venta>();
        }

        [Key]
        [Column("ID_Local")]
        public long IdLocal { get; set; }
        [StringLength(20)]
        [Unicode(false)]
        public string Nombre { get; set; } = null!;
        [StringLength(20)]
        [Unicode(false)]
        public string Direccion { get; set; } = null!;

        [InverseProperty("IdLocalNavigation")]
        public virtual ICollection<Venta> Venta { get; set; }
    }
}
