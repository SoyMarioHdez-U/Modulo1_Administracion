using System.ComponentModel.DataAnnotations;
namespace Modulo1_Administracion.Models
{
    public class items_combo_dos
    {
        [Key]
        public int? id_items_combo { get; set; }
        public int? id_combo { get; set; }
        public int? id_items_menu { get; set; }
    }
}
