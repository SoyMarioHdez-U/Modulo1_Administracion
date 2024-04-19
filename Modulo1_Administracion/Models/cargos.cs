using System.ComponentModel.DataAnnotations;
namespace Modulo1_Administracion.Models

{
    public class cargos
    {
        [Key]
        public int id_cargo {  get; set; }

        public string? cargo { get; set; }
    }
}
