namespace Modulo1_Administracion.Models
{
    public class items_combo_menu
    {
        public int id_combo { get; set; }
        public int id_items_menu { get; set; }
        public string? descripcion { get; set; }
        public decimal? precio { get; set; }
        public string? imagen { get; set; }
        public int? id_estado { get; set; }
    }
}
