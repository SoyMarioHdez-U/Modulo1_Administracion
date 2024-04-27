﻿using System.ComponentModel.DataAnnotations;

namespace Modulo1_Administracion.Models
{
    public class combo_dos
    {
        [Key]
        public int id_combo { get; set; }
        public string? nombre { get; set; }
        public string? descripcion { get; set; }
        public decimal? precio { get; set; }
        public string? imagen { get; set; }
        public int? id_estado { get; set; }
    }
}