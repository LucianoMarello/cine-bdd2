using BE;
using Microsoft.EntityFrameworkCore;

namespace DAL;

public class CajaDAL
{
    public List<Caja> Listar()
    {
        using var db = new CineContext();
        return db.Cajas.AsNoTracking().OrderBy(c => c.Nombre).ToList();
    }

    public Caja? Obtener(int id)
    {
        using var db = new CineContext();
        return db.Cajas.AsNoTracking().FirstOrDefault(c => c.IdCaja == id);
    }

    public bool ExisteNombre(string nombre, int exceptoId = 0)
    {
        using var db = new CineContext();
        return db.Cajas.Any(c => c.Nombre == nombre && c.IdCaja != exceptoId);
    }

    public void Agregar(Caja caja)
    {
        using var db = new CineContext();
        db.Cajas.Add(caja);
        db.SaveChanges();
    }

    public void Modificar(Caja caja)
    {
        using var db = new CineContext();
        db.Cajas.Update(caja);
        db.SaveChanges();
    }
}
