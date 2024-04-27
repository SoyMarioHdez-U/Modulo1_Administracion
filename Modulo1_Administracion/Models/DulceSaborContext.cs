using Microsoft.EntityFrameworkCore;
using Modulo1_Administracion.Data;
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
        public DbSet<items_promociones> items_promo { get; set; }
        public DbSet<mesas> mesas { get; set; }
        public DbSet<empleados> empleados { get; set; }
        public DbSet<estados> estados { get; set; }
        public DbSet<v_empleado_cargo> v_empleado_cargo { get; set; }
        public DbSet<obj_items_combo> obj_items_combos { get; set; }
        public DbSet<obj_items_promo> obj_items_promo { get; set; }
        public DbSet<promociones> promociones { get; set; }
        public DbSet<v_itemsPromoCombos> v_itemsPromoCombos { get; set; }

        public DbSet<v_items_menu> v_items_menu { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<items_combo>().HasNoKey();
            modelBuilder.Entity<items_promociones>().HasNoKey();
            // Otras configuraciones de tu modelo

            modelBuilder.Entity<obj_items_combo>().HasNoKey();
            modelBuilder.Entity<obj_items_promo>().HasNoKey();
            modelBuilder.Entity<v_empleado_cargo>().HasNoKey();
            modelBuilder.Entity<v_items_menu>().HasNoKey();
        }
        public DbSet<Modulo1_Administracion.Models.items_menu> items_menu { get; set; } = default!;
    }
}
