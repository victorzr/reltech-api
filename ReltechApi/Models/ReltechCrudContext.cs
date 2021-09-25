using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace ReltechApi.Models
{
    public partial class ReltechCrudContext : DbContext
    {
        public ReltechCrudContext()
        {
        }

        public ReltechCrudContext(DbContextOptions<ReltechCrudContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Organizaciones> Organizaciones { get; set; }
        public virtual DbSet<Usuarios> Usuarios { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Spanish_Latin America.1252");

            modelBuilder.Entity<Organizaciones>(entity =>
            {
                entity.ToTable("organizaciones");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Direccion)
                    .HasMaxLength(200)
                    .HasColumnName("direccion");

                entity.Property(e => e.Nombre)
                    .HasMaxLength(100)
                    .HasColumnName("nombre");
            });

            modelBuilder.Entity<Usuarios>(entity =>
            {
                entity.ToTable("usuarios");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Cedula)
                    .HasMaxLength(15)
                    .HasColumnName("cedula");

                entity.Property(e => e.CorreoElectronico)
                    .HasMaxLength(100)
                    .HasColumnName("correo_electronico");

                entity.Property(e => e.FechaNacimiento)
                    .HasColumnType("date")
                    .HasColumnName("fecha_nacimiento");

                entity.Property(e => e.NombreCompleto)
                    .HasMaxLength(200)
                    .HasColumnName("nombre_completo");

                entity.Property(e => e.OrganizacionId).HasColumnName("organizacion_id");

                entity.Property(e => e.Telefono)
                    .HasMaxLength(25)
                    .HasColumnName("telefono");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
