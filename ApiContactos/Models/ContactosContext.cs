using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ApiContactos.Models;

public partial class ContactosContext : DbContext
{
    public ContactosContext()
    {
    }

    public ContactosContext(DbContextOptions<ContactosContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Contacto> Contactos { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) { }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Contacto>(entity =>
        {
            entity.HasKey(e => e.IdContacto).HasName("PK__Contacto__A4D6BBFA491A7B4E");

            entity.ToTable("Contacto");

            entity.Property(e => e.CelularContacto)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.CorreoContacto)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.NombreContacto)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.Contactos)
                .HasForeignKey(d => d.IdUsuario)
                .HasConstraintName("FK_Contacto_Usuario");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.IdUsuario).HasName("PK__Usuario__5B65BF9700FE453F");

            entity.ToTable("Usuario");

            entity.Property(e => e.Clave)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.CorreoUsuario)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.NombreUsuario)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
