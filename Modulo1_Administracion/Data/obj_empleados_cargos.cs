namespace Modulo1_Administracion.Data
{
    public class obj_empleados_cargos
    {
        public int id_empleado { get; set; }
        public string? nombre { get; set; }
        public string? apellido { get; set; }
        public string? direccion { get; set; }
        public int? telefono { get; set; }
        public string? correo { get; set; }
        public string cargo { get; set; }
        public int? id_cargo { get; set; }
        public string estado { get; set; }
        public int? id_estado { get; set; }

    }
}
