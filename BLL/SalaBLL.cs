using BE;
using DAL;

namespace BLL;

public class SalaBLL
{
    private readonly SalaDAL _dal = new();

    public List<Sala> Listar() => _dal.Listar();

    /// <summary>Alta si id es 0; modificación en caso contrario.</summary>
    public void Guardar(int id, string nombre, int capacidad)
    {
        nombre = (nombre ?? string.Empty).Trim();

        if (nombre.Length == 0)
            throw new NegocioException("El nombre de la sala es obligatorio.");
        if (nombre.Length > 50)
            throw new NegocioException("El nombre no puede superar los 50 caracteres.");
        if (capacidad <= 0)
            throw new NegocioException("La capacidad debe ser mayor a cero.");
        if (_dal.ExisteNombre(nombre, id))
            throw new NegocioException("Ya existe una sala con ese nombre.");

        if (id == 0)
        {
            _dal.Agregar(new Sala { Nombre = nombre, Capacidad = capacidad });
            return;
        }

        Sala sala = _dal.Obtener(id) ?? throw new NegocioException("La sala ya no existe.");
        sala.Nombre = nombre;
        sala.Capacidad = capacidad;
        _dal.Modificar(sala);
    }

    /// <summary>Baja lógica o reactivación: la sala nunca se borra.</summary>
    public void CambiarEstado(int id, bool activa)
    {
        Sala sala = _dal.Obtener(id) ?? throw new NegocioException("La sala ya no existe.");
        sala.Estado = activa;
        _dal.Modificar(sala);
    }
}
