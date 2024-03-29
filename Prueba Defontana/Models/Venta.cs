﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Prueba_Defontana.Models
{
    public partial class Venta
    {
        public Venta()
        {
            VentaDetalles = new HashSet<VentaDetalle>();
        }

        [Key]
        [Column("ID_Venta")]
        public long IdVenta { get; set; }
        public int Total { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime Fecha { get; set; }
        [Column("ID_Local")]
        public long IdLocal { get; set; }

        [ForeignKey("IdLocal")]
        [InverseProperty("Venta")]
        public virtual Local IdLocalNavigation { get; set; } = null!;
        [InverseProperty("IdVentaNavigation")]
        public virtual ICollection<VentaDetalle> VentaDetalles { get; set; }
    }
}
