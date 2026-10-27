namespace BE;

public class Venta
{
    public static readonly string[] FormasPago = { "Efectivo", "Debito", "Credito", "QR" };

    public int IdVenta { get; set; }
    public int IdFuncion { get; set; }
    public int IdCaja { get; set; }
    public int IdUsuario { get; set; }
    public DateTime FechaVenta { get; set; }
    public int CantidadEntradas { get; set; }
    /// <summary>Precio de la entrada al momento de vender (copia de Funcion.PrecioEntrada).</summary>
    public decimal PrecioUnitario { get; set; }
    /// <summary>Debe ser CantidadEntradas × PrecioUnitario; la base lo valida con un CHECK.</summary>
    public decimal Total { get; set; }
    public string FormaPago { get; set; } = string.Empty;

    public Funcion Funcion { get; set; } = null!;
    public Caja Caja { get; set; } = null!;
    public Usuario Usuario { get; set; } = null!;
}
