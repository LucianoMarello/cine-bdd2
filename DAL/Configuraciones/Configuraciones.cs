using BE;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configuraciones;

// Mapeo (Fluent API) entre las clases de BE y las tablas de CineDB.
// Hace falta porque los nombres no coinciden: en C# las propiedades van en
// PascalCase (IdUsuario) y en la base las columnas en snake_case (id_usuario).
// Las reglas (CHECK, UNIQUE, DEFAULT) viven en la base; acá solo se declaran
// nombres, tipos y relaciones.

public class UsuarioConfig : IEntityTypeConfiguration<Usuario>
{
    public void Configure(EntityTypeBuilder<Usuario> b)
    {
        b.ToTable("Usuario");
        b.HasKey(u => u.IdUsuario);
        b.Property(u => u.IdUsuario).HasColumnName("id_usuario");
        b.Property(u => u.NombreUsuario).HasColumnName("nombre_usuario").HasMaxLength(50).IsUnicode(false);
        b.Property(u => u.Clave).HasColumnName("clave").HasMaxLength(255).IsUnicode(false);
        b.Property(u => u.Rol).HasColumnName("rol").HasMaxLength(30).IsUnicode(false);
        b.Property(u => u.Estado).HasColumnName("estado");
    }
}

public class PeliculaConfig : IEntityTypeConfiguration<Pelicula>
{
    public void Configure(EntityTypeBuilder<Pelicula> b)
    {
        b.ToTable("Pelicula");
        b.HasKey(p => p.IdPelicula);
        b.Property(p => p.IdPelicula).HasColumnName("id_pelicula");
        b.Property(p => p.Titulo).HasColumnName("titulo").HasMaxLength(100).IsUnicode(false);
        b.Property(p => p.Genero).HasColumnName("genero").HasMaxLength(50).IsUnicode(false);
        b.Property(p => p.Duracion).HasColumnName("duracion");
        b.Property(p => p.Estado).HasColumnName("estado");
    }
}

public class SalaConfig : IEntityTypeConfiguration<Sala>
{
    public void Configure(EntityTypeBuilder<Sala> b)
    {
        b.ToTable("Sala");
        b.HasKey(s => s.IdSala);
        b.Property(s => s.IdSala).HasColumnName("id_sala");
        b.Property(s => s.Nombre).HasColumnName("nombre").HasMaxLength(50).IsUnicode(false);
        b.Property(s => s.Capacidad).HasColumnName("capacidad");
        b.Property(s => s.Estado).HasColumnName("estado");
    }
}

public class CajaConfig : IEntityTypeConfiguration<Caja>
{
    public void Configure(EntityTypeBuilder<Caja> b)
    {
        b.ToTable("Caja");
        b.HasKey(c => c.IdCaja);
        b.Property(c => c.IdCaja).HasColumnName("id_caja");
        b.Property(c => c.Nombre).HasColumnName("nombre").HasMaxLength(30).IsUnicode(false);
        b.Property(c => c.Estado).HasColumnName("estado");
    }
}

public class FuncionConfig : IEntityTypeConfiguration<Funcion>
{
    public void Configure(EntityTypeBuilder<Funcion> b)
    {
        b.ToTable("Funcion");
        b.HasKey(f => f.IdFuncion);
        b.Property(f => f.IdFuncion).HasColumnName("id_funcion");
        b.Property(f => f.IdPelicula).HasColumnName("id_pelicula");
        b.Property(f => f.IdSala).HasColumnName("id_sala");
        b.Property(f => f.FechaHora).HasColumnName("fecha_hora").HasColumnType("datetime");
        b.Property(f => f.PrecioEntrada).HasColumnName("precio_entrada").HasColumnType("decimal(10,2)");
        b.Property(f => f.Formato).HasColumnName("formato").HasMaxLength(10).IsUnicode(false);

        // Restrict = no borrar en cascada, igual que las FK de la base.
        b.HasOne(f => f.Pelicula).WithMany(p => p.Funciones)
            .HasForeignKey(f => f.IdPelicula).OnDelete(DeleteBehavior.Restrict);
        b.HasOne(f => f.Sala).WithMany(s => s.Funciones)
            .HasForeignKey(f => f.IdSala).OnDelete(DeleteBehavior.Restrict);
    }
}

public class VentaConfig : IEntityTypeConfiguration<Venta>
{
    public void Configure(EntityTypeBuilder<Venta> b)
    {
        b.ToTable("Venta");
        b.HasKey(v => v.IdVenta);
        b.Property(v => v.IdVenta).HasColumnName("id_venta");
        b.Property(v => v.IdFuncion).HasColumnName("id_funcion");
        b.Property(v => v.IdCaja).HasColumnName("id_caja");
        b.Property(v => v.IdUsuario).HasColumnName("id_usuario");
        b.Property(v => v.FechaVenta).HasColumnName("fecha_venta").HasColumnType("datetime");
        b.Property(v => v.CantidadEntradas).HasColumnName("cantidad_entradas");
        b.Property(v => v.PrecioUnitario).HasColumnName("precio_unitario").HasColumnType("decimal(10,2)");
        b.Property(v => v.Total).HasColumnName("total").HasColumnType("decimal(12,2)");
        b.Property(v => v.FormaPago).HasColumnName("forma_pago").HasMaxLength(20).IsUnicode(false);

        b.HasOne(v => v.Funcion).WithMany(f => f.Ventas)
            .HasForeignKey(v => v.IdFuncion).OnDelete(DeleteBehavior.Restrict);
        b.HasOne(v => v.Caja).WithMany(c => c.Ventas)
            .HasForeignKey(v => v.IdCaja).OnDelete(DeleteBehavior.Restrict);
        b.HasOne(v => v.Usuario).WithMany(u => u.Ventas)
            .HasForeignKey(v => v.IdUsuario).OnDelete(DeleteBehavior.Restrict);
    }
}

public class ObjetivoSemanalCajaConfig : IEntityTypeConfiguration<ObjetivoSemanalCaja>
{
    public void Configure(EntityTypeBuilder<ObjetivoSemanalCaja> b)
    {
        b.ToTable("Objetivo_semanal_caja");
        b.HasKey(o => o.IdObjetivo);
        b.Property(o => o.IdObjetivo).HasColumnName("id_objetivo");
        b.Property(o => o.IdCaja).HasColumnName("id_caja");
        b.Property(o => o.FechaInicio).HasColumnName("fecha_inicio").HasColumnType("date");
        b.Property(o => o.FechaFin).HasColumnName("fecha_fin").HasColumnType("date");
        b.Property(o => o.MontoObjetivo).HasColumnName("monto_objetivo").HasColumnType("decimal(12,2)");

        b.HasOne(o => o.Caja).WithMany(c => c.Objetivos)
            .HasForeignKey(o => o.IdCaja).OnDelete(DeleteBehavior.Restrict);
    }
}
