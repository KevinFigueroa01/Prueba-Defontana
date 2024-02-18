using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Prueba_Defontana.Models
{
    [Table("Marca")]
    public partial class Marca
    {
        public Marca()
        {
            Productos = new HashSet<Producto>();
        }

        [Key]
        [Column("ID_Marca")]
        public long IdMarca { get; set; }
        [StringLength(20)]
        [Unicode(false)]
        public string Nombre { get; set; } = null!;

        [InverseProperty("IdMarcaNavigation")]
        public virtual ICollection<Producto> Productos { get; set; }
    }
}
