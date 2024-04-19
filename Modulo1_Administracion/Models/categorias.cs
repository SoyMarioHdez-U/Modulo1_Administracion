using System.ComponentModel.DataAnnotations;
namespace Modulo1_Administracion.Models
{
    public class categorias
    {
        [Key]
        public int id_categoria {  get; set; }
        public string? categoria { get; set; }
    }
}
