using System.ComponentModel.DataAnnotations;
namespace Modulo1_Administracion.Models
{
    public class items_promo
    {
        public int id_promo
        { get; set; }

        public int? id_item_menu { get; set; }
    }
}
