using BE;
using DAL;

namespace BLL;

public class FuncionBLL
{
    private readonly FuncionDAL _dal = new();
    private readonly PeliculaDAL _peliculaDal = new();
    private readonly SalaDAL _salaDal = new();

    public List<Funcion> Listar() => _dal.Listar();

    public Dictionary<int, int> EntradasVendidasPorFuncion() => _dal.EntradasVendidasPorFuncion();

    /// <summary>Alta si id es 0; modificación en caso contrario.</summary>
    public void Guardar(int id, int idPelicula, int idSala, DateTime fechaHora, decimal precioEntrada, string formato)
    {
        // Se descartan segundos y milisegundos: una función empieza "20:30", no "20:30:17".
        fechaHora = new DateTime(fechaHora.Year, fechaHora.Month, fechaHora.Day, fechaHora.Hour, fechaHora.Minute, 0);

        Pelicula pelicula = _peliculaDal.Obtener(idPelicula) ?? throw new NegocioException("Elegí una película.");
        Sala sala = _salaDal.Obtener(idSala) ?? throw new NegocioException("Elegí una sala.");

        if (!pelicula.Estado)
            throw new NegocioException("La película está dada de baja.");
        if (!sala.Estado)
            throw new NegocioException("La sala está dada de baja.");
        if (fechaHora <= DateTime.Now)
            throw new NegocioException("La fecha y hora de la función debe ser futura.");
        if (precioEntrada < 0)
            throw new NegocioException("El precio no puede ser negativo.");
        if (!Funcion.Formatos.Contains(formato))
            throw new NegocioException("El formato no es válido.");

        if (id != 0 && _dal.TieneVentas(id))
            throw new NegocioException("La función ya tiene entradas vendidas y no se puede modificar.");

        ValidarSolapamiento(id, sala, pelicula, fechaHora);

        if (id == 0)
        {
            _dal.Agregar(new Funcion
            {
                IdPelicula = idPelicula,
                IdSala = idSala,
                FechaHora = fechaHora,
                PrecioEntrada = precioEntrada,
                Formato = formato
            });
            return;
        }

        Funcion funcion = _dal.Obtener(id) ?? throw new NegocioException("La función ya no existe.");
        funcion.IdPelicula = idPelicula;
        funcion.IdSala = idSala;
        funcion.FechaHora = fechaHora;
        funcion.PrecioEntrada = precioEntrada;
        funcion.Formato = formato;
        _dal.Modificar(funcion);
    }

    /// <summary>
    /// Las funciones no tienen baja lógica: se eliminan, pero solo si todavía
    /// no se vendió ninguna entrada.
    /// </summary>
    public void Eliminar(int id)
    {
        if (_dal.TieneVentas(id))
            throw new NegocioException("La función ya tiene entradas vendidas y no se puede eliminar.");

        _dal.Eliminar(id);
    }

    // Dos funciones de la misma sala se pisan si una empieza antes de que
    // termine la otra. La base solo impide el mismo horario exacto (UNIQUE);
    // esto cubre el resto, porque depende de la duración de cada película.
    private void ValidarSolapamiento(int id, Sala sala, Pelicula pelicula, DateTime inicio)
    {
        DateTime fin = inicio.AddMinutes(pelicula.Duracion);

        // Se buscan las funciones de la sala que empiezan hasta 12 horas antes
        // (ninguna película dura más) y hasta el fin de la nueva.
        List<Funcion> cercanas = _dal.ListarDeSalaEntre(sala.IdSala, inicio.AddHours(-12), fin, id);

        Funcion? choque = cercanas.FirstOrDefault(f =>
            f.FechaHora < fin && f.FechaHora.AddMinutes(f.Pelicula.Duracion) > inicio);

        if (choque != null)
        {
            DateTime finChoque = choque.FechaHora.AddMinutes(choque.Pelicula.Duracion);
            throw new NegocioException(
                $"Se superpone con \"{choque.Pelicula.Titulo}\" en {sala.Nombre} " +
                $"({choque.FechaHora:dd/MM/yyyy HH:mm} a {finChoque:HH:mm}).");
        }
    }
}
