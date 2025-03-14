﻿using Microsoft.EntityFrameworkCore;
using DozoWeb.Models;

namespace DozoWeb.Data
{
    public class ApplicationDbContext : DbContext
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
        }

    }
}

