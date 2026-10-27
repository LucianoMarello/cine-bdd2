using BE;
using DAL;

namespace BLL;

public class PeliculaBLL
{
    private readonly PeliculaDAL _dal = new();

    public List<Pelicula> Listar() => _dal.Listar();

    /// <summary>Alta si id es 0; modificación en caso contrario.</summary>
    public void Guardar(int id, string titulo, string genero, int duracion)
    {
        titulo = (titulo ?? string.Empty).Trim();

        if (titulo.Length == 0)
            throw new NegocioException("El título es obligatorio.");
        if (titulo.Length > 100)
            throw new NegocioException("El título no puede superar los 100 caracteres.");
        if (!Pelicula.Generos.Contains(genero))
            throw new NegocioException("Elegí un género de la lista.");
        if (duracion <= 0)
            throw new NegocioException("La duración debe ser mayor a cero.");

        if (id == 0)
        {
            _dal.Agregar(new Pelicula { Titulo = titulo, Genero = genero, Duracion = duracion });
            return;
        }

        Pelicula pelicula = _dal.Obtener(id) ?? throw new NegocioException("La película ya no existe.");
        pelicula.Titulo = titulo;
        pelicula.Genero = genero;
        pelicula.Duracion = duracion;
        _dal.Modificar(pelicula);
    }

    public void CambiarEstado(int id, bool activa)
    {
        Pelicula pelicula = _dal.Obtener(id) ?? throw new NegocioException("La película ya no existe.");
        pelicula.Estado = activa;
        _dal.Modificar(pelicula);
    }
}
