using BE;
using Microsoft.EntityFrameworkCore;

namespace DAL;

public class VentaDAL
{
    /// <summary>
    /// Registra la venta solo si la función tiene lugar. Devuelve false (sin
    /// guardar nada) si la cantidad pedida supera las entradas disponibles.
    ///
    /// El control de cupo está acá y no en la BLL porque tiene que ocurrir
    /// dentro de la MISMA transacción que el INSERT. Si se consultara el cupo
    /// primero y se insertara después, dos cajas vendiendo a la vez podrían
    /// leer "quedan 2", vender 2 cada una y sobrevender la sala.
    ///
    /// Cómo se evita: al empezar, se bloquea la fila de la función con
    /// UPDLOCK + HOLDLOCK. La segunda caja que quiera vender para esa misma
    /// función queda esperando hasta que la primera termine (COMMIT), y recién
    /// ahí lee el cupo ya actualizado. Las ventas de otras funciones no se
    /// frenan entre sí.
    /// </summary>
    public bool RegistrarSiHayCupo(Venta venta, out int disponibles)
    {
        using var db = new CineContext();
        using var transaccion = db.Database.BeginTransaction();

        int capacidad = db.Database
            .SqlQuery<int>($@"SELECT s.capacidad AS Value
                              FROM Funcion f WITH (UPDLOCK, HOLDLOCK)
                              JOIN Sala s ON s.id_sala = f.id_sala
                              WHERE f.id_funcion = {venta.IdFuncion}")
            .ToList()
            .Single();

        int vendidas = db.Ventas
            .Where(v => v.IdFuncion == venta.IdFuncion)
            .Sum(v => (int?)v.CantidadEntradas) ?? 0;

        disponibles = capacidad - vendidas;

        if (venta.CantidadEntradas > disponibles)
        {
            transaccion.Rollback();
            return false;
        }

        db.Ventas.Add(venta);
        db.SaveChanges();
        transaccion.Commit();
        return true;
    }

    /// <summary>
    /// Ventas entre dos fechas (hasta es exclusivo). En idCaja, idPelicula e
    /// idUsuario, 0 significa "sin filtrar".
    /// </summary>
    public List<Venta> Listar(DateTime desde, DateTime hasta, int idCaja = 0, int idPelicula = 0, int idUsuario = 0)
    {
        using var db = new CineContext();

        IQueryable<Venta> consulta = db.Ventas.AsNoTracking()
            .Include(v => v.Funcion).ThenInclude(f => f.Pelicula)
            .Include(v => v.Funcion).ThenInclude(f => f.Sala)
            .Include(v => v.Caja)
            .Include(v => v.Usuario)
            .Where(v => v.FechaVenta >= desde && v.FechaVenta < hasta);

        if (idCaja != 0)
            consulta = consulta.Where(v => v.IdCaja == idCaja);
        if (idPelicula != 0)
            consulta = consulta.Where(v => v.Funcion.IdPelicula == idPelicula);
        if (idUsuario != 0)
            consulta = consulta.Where(v => v.IdUsuario == idUsuario);

        return consulta.OrderByDescending(v => v.FechaVenta).ToList();
    }
}
