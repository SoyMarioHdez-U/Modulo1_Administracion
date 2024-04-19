using System.ComponentModel.DataAnnotations;
namespace Modulo1_Administracion.Models
{
    public class mesas
    {
        [Key]
        public int id_mesa { get; set; }
        public int? cantidad_personas { get; set; }
        public int? id_estado { get; set; }
        public string? nombre_mesa { get; set; }
    }
}
