using BE;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DAL;

/// <summary>
/// Puerta de entrada de Entity Framework a la base CineDB.
/// Cada DbSet representa una tabla. La base se crea con sql/01_crear_bd.sql,
/// no con migraciones: acá solo se le explica a EF cómo es cada tabla.
/// </summary>
public class CineContext : DbContext
{
    public DbSet<Usuario> Usuarios => Set<Usuario>();
    public DbSet<Pelicula> Peliculas => Set<Pelicula>();
    public DbSet<Sala> Salas => Set<Sala>();
    public DbSet<Caja> Cajas => Set<Caja>();
    public DbSet<Funcion> Funciones => Set<Funcion>();
    public DbSet<Venta> Ventas => Set<Venta>();
    public DbSet<ObjetivoSemanalCaja> ObjetivosSemanales => Set<ObjetivoSemanalCaja>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(Conexion.Cadena);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Aplica todas las clases de la carpeta Configuraciones.
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CineContext).Assembly);
    }
}

/// <summary>
/// Lee la cadena de conexión de appsettings.json (está en el proyecto UI
/// y se copia junto al ejecutable).
/// </summary>
internal static class Conexion
{
    private static string? _cadena;

    public static string Cadena => _cadena ??= Leer();

    private static string Leer()
    {
        var config = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false)
            .Build();

        return config.GetConnectionString("CineDB")
            ?? throw new InvalidOperationException(
                "Falta la cadena de conexión 'CineDB' en appsettings.json.");
    }
}
