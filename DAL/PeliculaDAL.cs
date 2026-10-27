using BE;
using Microsoft.EntityFrameworkCore;

namespace DAL;

public class PeliculaDAL
{
    public List<Pelicula> Listar()
    {
        using var db = new CineContext();
        return db.Peliculas.AsNoTracking().OrderBy(p => p.Titulo).ToList();
    }

    public Pelicula? Obtener(int id)
    {
        using var db = new CineContext();
        return db.Peliculas.AsNoTracking().FirstOrDefault(p => p.IdPelicula == id);
    }

    public void Agregar(Pelicula pelicula)
    {
        using var db = new CineContext();
        db.Peliculas.Add(pelicula);
        db.SaveChanges();
    }

    public void Modificar(Pelicula pelicula)
    {
        using var db = new CineContext();
        db.Peliculas.Update(pelicula);
        db.SaveChanges();
    }
}
