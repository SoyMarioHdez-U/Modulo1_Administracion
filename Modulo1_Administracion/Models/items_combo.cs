using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
namespace Modulo1_Administracion.Models
{
    [Keyless]
    public class items_combo
    {
        [Required]
        public int id_combo { get; set; }
        public int id_item_menu { get; set; }
    }
}
