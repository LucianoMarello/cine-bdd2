using BE;
using Microsoft.EntityFrameworkCore;

namespace DAL;

public class SalaDAL
{
    public List<Sala> Listar()
    {
        using var db = new CineContext();
        return db.Salas.AsNoTracking().OrderBy(s => s.Nombre).ToList();
    }

    public Sala? Obtener(int id)
    {
        using var db = new CineContext();
        return db.Salas.AsNoTracking().FirstOrDefault(s => s.IdSala == id);
    }

    /// <summary>exceptoId permite ignorar la propia sala al modificarla.</summary>
    public bool ExisteNombre(string nombre, int exceptoId = 0)
    {
        using var db = new CineContext();
        return db.Salas.Any(s => s.Nombre == nombre && s.IdSala != exceptoId);
    }

    public void Agregar(Sala sala)
    {
        using var db = new CineContext();
        db.Salas.Add(sala);
        db.SaveChanges();
    }

    public void Modificar(Sala sala)
    {
        using var db = new CineContext();
        db.Salas.Update(sala);
        db.SaveChanges();
    }
}
