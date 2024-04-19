using System.ComponentModel.DataAnnotations;
namespace Modulo1_Administracion.Models
{
    public class estados
    {
        [Key]
        public int id_estado {  get; set; }
        public string nombre { get; set; }
        public string tipo_estado { get; set; }
    }
}
