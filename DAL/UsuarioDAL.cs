using BE;
using Microsoft.EntityFrameworkCore;

namespace DAL;

public class UsuarioDAL
{
    public bool ExisteAlguno()
    {
        using var db = new CineContext();
        return db.Usuarios.Any();
    }

    /// <summary>exceptoId permite ignorar al propio usuario al modificarlo.</summary>
    public bool ExisteNombre(string nombreUsuario, int exceptoId = 0)
    {
        using var db = new CineContext();
        return db.Usuarios.Any(u => u.NombreUsuario == nombreUsuario && u.IdUsuario != exceptoId);
    }

    public List<Usuario> Listar()
    {
        using var db = new CineContext();
        return db.Usuarios.AsNoTracking().OrderBy(u => u.NombreUsuario).ToList();
    }

    public Usuario? Obtener(int id)
    {
        using var db = new CineContext();
        return db.Usuarios.AsNoTracking().FirstOrDefault(u => u.IdUsuario == id);
    }

    public Usuario? ObtenerPorNombre(string nombreUsuario)
    {
        using var db = new CineContext();
        return db.Usuarios.AsNoTracking().FirstOrDefault(u => u.NombreUsuario == nombreUsuario);
    }

    /// <summary>Cantidad de administradores activos, sin contar a exceptoId.</summary>
    public int ContarAdminsActivos(int exceptoId = 0)
    {
        using var db = new CineContext();
        return db.Usuarios.Count(u => u.Rol == Usuario.RolAdmin && u.Estado && u.IdUsuario != exceptoId);
    }

    public void Agregar(Usuario usuario)
    {
        using var db = new CineContext();
        db.Usuarios.Add(usuario);
        db.SaveChanges();
    }

    public void Modificar(Usuario usuario)
    {
        using var db = new CineContext();
        db.Usuarios.Update(usuario);
        db.SaveChanges();
    }
}
