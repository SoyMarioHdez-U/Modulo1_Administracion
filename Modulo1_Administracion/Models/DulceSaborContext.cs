using Microsoft.EntityFrameworkCore;
using Modulo1_Administracion.Models;
namespace Modulo1_Administracion.Models
{
    public class DulceSaborContext : DbContext
    {
        public DulceSaborContext(DbContextOptions options) : base(options) {
            
        }

        public DbSet<cargos> cargos { get; set; }
        public DbSet<categorias> categorias { get; set; }
        public DbSet<combos> combos { get; set; }
        public DbSet<items_combo> items_combo { get; set; }
        public DbSet<items_combo_dos> items_combo_dos { get; set; }
        public DbSet<items_promo> items_promo { get; set; }
        public DbSet<mesas> mesas { get; set; }
        public DbSet<estados> estados { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<items_combo>().HasNoKey();
            modelBuilder.Entity<items_promo>().HasNoKey();
            // Otras configuraciones de tu modelo
        }
        public DbSet<Modulo1_Administracion.Models.items_menu> items_menu { get; set; } = default!;

    }
}
