namespace Modulo1_Administracion.Data
{
    public class obj_promo_estado
    {
        public int id_promo { get; set; }
        public string? nombre { get; set; }
        public decimal? precio { get; set; }
        public DateTime? fecha_inicio { get; set; }
        public DateTime? fecha_final {  get; set; }
        public string estado { get; set; }

    }
}
