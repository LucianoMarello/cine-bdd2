using BE;
using DAL;

namespace BLL;

public class CajaBLL
{
    private readonly CajaDAL _dal = new();

    public List<Caja> Listar() => _dal.Listar();

    /// <summary>Alta si id es 0; modificación en caso contrario.</summary>
    public void Guardar(int id, string nombre)
    {
        nombre = (nombre ?? string.Empty).Trim();

        if (nombre.Length == 0)
            throw new NegocioException("El nombre de la caja es obligatorio.");
        if (nombre.Length > 30)
            throw new NegocioException("El nombre no puede superar los 30 caracteres.");
        if (_dal.ExisteNombre(nombre, id))
            throw new NegocioException("Ya existe una caja con ese nombre.");

        if (id == 0)
        {
            _dal.Agregar(new Caja { Nombre = nombre });
            return;
        }

        Caja caja = _dal.Obtener(id) ?? throw new NegocioException("La caja ya no existe.");
        caja.Nombre = nombre;
        _dal.Modificar(caja);
    }

    public void CambiarEstado(int id, bool activa)
    {
        Caja caja = _dal.Obtener(id) ?? throw new NegocioException("La caja ya no existe.");
        caja.Estado = activa;
        _dal.Modificar(caja);
    }
}
