using BE;
using DAL;

namespace BLL;

public class ObjetivoSemanalCajaBLL
{
    private readonly ObjetivoSemanalCajaDAL _dal = new();
    private readonly CajaDAL _cajaDal = new();

    public List<ObjetivoSemanalCaja> Listar() => _dal.Listar();

    /// <summary>Lunes de la semana a la que pertenece una fecha.</summary>
    public static DateTime LunesDeLaSemana(DateTime fecha)
    {
        int diasDesdeLunes = ((int)fecha.DayOfWeek + 6) % 7;
        return fecha.Date.AddDays(-diasDesdeLunes);
    }

    /// <summary>
    /// Alta si id es 0; modificación en caso contrario. Las semanas van de
    /// lunes a domingo: fecha_fin se calcula, no se pide.
    /// </summary>
    public void Guardar(int id, int idCaja, DateTime fechaInicio, decimal montoObjetivo)
    {
        fechaInicio = fechaInicio.Date;

        Caja caja = _cajaDal.Obtener(idCaja) ?? throw new NegocioException("Elegí una caja.");

        if (!caja.Estado)
            throw new NegocioException("La caja está dada de baja.");
        if (fechaInicio.DayOfWeek != DayOfWeek.Monday)
            throw new NegocioException("La semana tiene que empezar un lunes.");
        if (montoObjetivo <= 0)
            throw new NegocioException("El monto objetivo debe ser mayor a cero.");
        if (_dal.Existe(idCaja, fechaInicio, id))
            throw new NegocioException("Esa caja ya tiene un objetivo cargado para esa semana.");

        DateTime fechaFin = fechaInicio.AddDays(6);

        if (id == 0)
        {
            _dal.Agregar(new ObjetivoSemanalCaja
            {
                IdCaja = idCaja,
                FechaInicio = fechaInicio,
                FechaFin = fechaFin,
                MontoObjetivo = montoObjetivo
            });
            return;
        }

        ObjetivoSemanalCaja objetivo = _dal.Obtener(id) ?? throw new NegocioException("El objetivo ya no existe.");
        objetivo.IdCaja = idCaja;
        objetivo.FechaInicio = fechaInicio;
        objetivo.FechaFin = fechaFin;
        objetivo.MontoObjetivo = montoObjetivo;
        _dal.Modificar(objetivo);
    }

    public void Eliminar(int id) => _dal.Eliminar(id);
}
