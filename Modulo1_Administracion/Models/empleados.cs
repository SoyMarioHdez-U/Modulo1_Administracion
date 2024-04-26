using System.ComponentModel.DataAnnotations;

namespace Modulo1_Administracion.Models
{
    public class empleados
    {

        [Key]
      
        public int id_empleado { get; set; }
 
        public string? nombre { get; set; }

        public string? apellido { get; set; }

        public string? direccion { get; set; }

        public int? telefono { get; set; }

        public string? correo { get; set; }

        public int? id_cargo { get; set; }

        public int? id_estado { get; set; }

    }
}
