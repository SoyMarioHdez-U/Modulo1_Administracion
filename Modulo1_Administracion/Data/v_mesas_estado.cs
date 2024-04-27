using System.ComponentModel.DataAnnotations;

namespace Modulo1_Administracion.Data
{
    public class v_mesas_estado
    {
        public int id_mesa { get; set; }

        public int id { get; set; }
        public int? cantidad_personas { get; set; }      
        public int? id_estado { get; set; }
        public string? nombre_mesa { get; set; }
    }
}
