﻿using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
namespace Modulo1_Administracion.Models
{
    public class items_promociones
    {
        [Key]
        public int id_items_promo { get; set; }
        public int? id_promo { get; set; }
        public int? id_items_combo { get; set; }
    }
}
