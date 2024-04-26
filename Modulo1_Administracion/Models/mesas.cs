using System.ComponentModel.DataAnnotations;
namespace Modulo1_Administracion.Models
{
    public class mesas
    {
        [Key]
        [Display(Name = "Id")]
        public int id_mesa { get; set; }
        [Display(Name = "Cantidad de personas")]
        public int? cantidad_personas { get; set; }
        [Display(Name = "Estado")]
        public int? id_estado { get; set; }
        [Display(Name = "Nombre mesa")]
        public string? nombre_mesa { get; set; }
    }
}
