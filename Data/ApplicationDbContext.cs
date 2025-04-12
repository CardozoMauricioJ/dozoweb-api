using DozoWeb.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DozoWeb.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUser, IdentityRole, string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Cerveceria> Cervecerias { get; set; }

        public DbSet<Opinion> Opiniones { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configurar la relación uno a muchos entre Cerveceria y Opinion
            modelBuilder.Entity<Opinion>()
                .HasOne(o => o.Cerveceria) // Una Opinion tiene una Cerveceria
                .WithMany(c => c.Opiniones) // Una Cerveceria tiene muchas Opiniones
                .HasForeignKey(o => o.CerveceriaId) // Opinion tiene una FK a Cerveceria
                .OnDelete(DeleteBehavior.Cascade); // Elimina opiniones si se elimina la cervecería

            modelBuilder.Entity<Cerveceria>()
                .Property(c => c.PrecioPromedio)
                .HasPrecision(10, 2); // 10 dígitos en total, 2 decimales
        }

    }
}

