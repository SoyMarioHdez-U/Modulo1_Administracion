using System.ComponentModel.DataAnnotations;
namespace Modulo1_Administracion.Models
{
    public class items_menu
    {
        [Key]
        [Display(Name = "Id")]
        public int id_item_menu { get; set; }

        [Display(Name = "Nombre")] 
        public string? nombre { get; set; }
        
        [Display(Name = "Descripción")] 
        public string? descripcion { get; set; }

        [Display(Name = "Precio")]
        public decimal? precio { get; set; }

        [Display(Name = "Imagen")]
        public string? imagen { get; set; }
        
        [Display(Name = "Estado")]
        public int? id_estado { get; set; }
        
        [Display(Name = "Categoria")]
        public int? id_categoria { get; set; }
    }
}
