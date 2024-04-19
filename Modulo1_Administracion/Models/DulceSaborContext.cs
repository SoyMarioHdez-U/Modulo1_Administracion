using Microsoft.EntityFrameworkCore;
namespace Modulo1_Administracion.Models
{
    public class DulceSaborContext : DbContext
    {
        public DulceSaborContext(DbContextOptions options) : base(options) {
        
        }

        public DbSet<cargos> cargos { get; set; }

    }
}
