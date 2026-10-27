using BE;
using Microsoft.EntityFrameworkCore;

namespace DAL;

public class FuncionDAL
{
    /// <summary>Funciones con su película y su sala, de la más nueva a la más vieja.</summary>
    public List<Funcion> Listar()
    {
        using var db = new CineContext();
        return db.Funciones.AsNoTracking()
            .Include(f => f.Pelicula)
            .Include(f => f.Sala)
            .OrderByDescending(f => f.FechaHora)
            .ToList();
    }

    /// <summary>
    /// Funciones que se pueden vender: todavía no empezaron y su película y
    /// su sala están activas. De la más próxima a la más lejana.
    /// </summary>
    public List<Funcion> ListarEnVenta(DateTime ahora)
    {
        using var db = new CineContext();
        return db.Funciones.AsNoTracking()
            .Include(f => f.Pelicula)
            .Include(f => f.Sala)
            .Where(f => f.FechaHora > ahora && f.Pelicula.Estado && f.Sala.Estado)
            .OrderBy(f => f.FechaHora)
            .ToList();
    }

    public Funcion? Obtener(int id)
    {
        using var db = new CineContext();
        return db.Funciones.AsNoTracking().FirstOrDefault(f => f.IdFuncion == id);
    }

    /// <summary>Entradas vendidas por función (solo aparecen las que tienen ventas).</summary>
    public Dictionary<int, int> EntradasVendidasPorFuncion()
    {
        using var db = new CineContext();
        return db.Ventas
            .GroupBy(v => v.IdFuncion)
            .Select(g => new { IdFuncion = g.Key, Entradas = g.Sum(v => v.CantidadEntradas) })
            .ToDictionary(x => x.IdFuncion, x => x.Entradas);
    }

    public bool TieneVentas(int idFuncion)
    {
        using var db = new CineContext();
        return db.Ventas.Any(v => v.IdFuncion == idFuncion);
    }

    /// <summary>
    /// Funciones de una sala que empiezan dentro de un rango, con su película
    /// (hace falta la duración para saber cuándo termina cada una).
    /// </summary>
    public List<Funcion> ListarDeSalaEntre(int idSala, DateTime desde, DateTime hasta, int exceptoId = 0)
    {
        using var db = new CineContext();
        return db.Funciones.AsNoTracking()
            .Include(f => f.Pelicula)
            .Where(f => f.IdSala == idSala && f.FechaHora >= desde && f.FechaHora <= hasta && f.IdFuncion != exceptoId)
            .ToList();
    }

    public void Agregar(Funcion funcion)
    {
        using var db = new CineContext();
        db.Funciones.Add(funcion);
        db.SaveChanges();
    }

    public void Modificar(Funcion funcion)
    {
        using var db = new CineContext();
        db.Funciones.Update(funcion);
        db.SaveChanges();
    }

    public void Eliminar(int id)
    {
        using var db = new CineContext();
        db.Funciones.Where(f => f.IdFuncion == id).ExecuteDelete();
    }
}
