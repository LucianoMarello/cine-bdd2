using BE;
using DAL;

namespace BLL;

public class VentaBLL
{
    private readonly VentaDAL _dal = new();
    private readonly FuncionDAL _funcionDal = new();
    private readonly PeliculaDAL _peliculaDal = new();
    private readonly SalaDAL _salaDal = new();
    private readonly CajaDAL _cajaDal = new();

    /// <summary>Funciones que se pueden vender ahora.</summary>
    public List<Funcion> ListarFuncionesEnVenta() => _funcionDal.ListarEnVenta(DateTime.Now);

    public Dictionary<int, int> EntradasVendidasPorFuncion() => _funcionDal.EntradasVendidasPorFuncion();

    /// <summary>
    /// Registra una venta a nombre del usuario que inició sesión. El precio
    /// y el total no los decide la pantalla: se calculan acá a partir del
    /// precio de la función.
    /// </summary>
    public Venta Registrar(int idFuncion, int idCaja, int cantidadEntradas, string formaPago)
    {
        Usuario usuario = Sesion.UsuarioActual
            ?? throw new NegocioException("Tenés que iniciar sesión para vender.");

        Funcion funcion = _funcionDal.Obtener(idFuncion) ?? throw new NegocioException("Elegí una función.");
        Caja caja = _cajaDal.Obtener(idCaja) ?? throw new NegocioException("Elegí una caja.");
        Pelicula pelicula = _peliculaDal.Obtener(funcion.IdPelicula)!;
        Sala sala = _salaDal.Obtener(funcion.IdSala)!;

        if (funcion.FechaHora <= DateTime.Now)
            throw new NegocioException("La función ya empezó: no se pueden vender más entradas.");
        if (!pelicula.Estado)
            throw new NegocioException("La película está dada de baja.");
        if (!sala.Estado)
            throw new NegocioException("La sala está dada de baja.");
        if (!caja.Estado)
            throw new NegocioException("La caja está dada de baja.");
        if (cantidadEntradas <= 0)
            throw new NegocioException("La cantidad de entradas debe ser mayor a cero.");
        if (!Venta.FormasPago.Contains(formaPago))
            throw new NegocioException("Elegí una forma de pago.");

        var venta = new Venta
        {
            IdFuncion = idFuncion,
            IdCaja = idCaja,
            IdUsuario = usuario.IdUsuario,
            FechaVenta = DateTime.Now,
            CantidadEntradas = cantidadEntradas,
            PrecioUnitario = funcion.PrecioEntrada,
            Total = cantidadEntradas * funcion.PrecioEntrada,
            FormaPago = formaPago
        };

        if (!_dal.RegistrarSiHayCupo(venta, out int disponibles))
        {
            throw new NegocioException(disponibles <= 0
                ? "La función está agotada."
                : $"No hay lugar suficiente: quedan {disponibles} entradas para esta función.");
        }

        return venta;
    }

    /// <summary>
    /// Ventas entre dos días (ambos incluidos). En idCaja e idPelicula, 0
    /// significa "todas". El Admin ve todas las ventas; el Cajero, solo las suyas.
    /// </summary>
    public List<Venta> Listar(DateTime desde, DateTime hasta, int idCaja = 0, int idPelicula = 0)
    {
        Usuario usuario = Sesion.UsuarioActual
            ?? throw new NegocioException("Tenés que iniciar sesión.");

        if (hasta.Date < desde.Date)
            throw new NegocioException("La fecha \"hasta\" no puede ser anterior a \"desde\".");

        int idUsuario = Sesion.EsAdmin ? 0 : usuario.IdUsuario;

        return _dal.Listar(desde.Date, hasta.Date.AddDays(1), idCaja, idPelicula, idUsuario);
    }
}
