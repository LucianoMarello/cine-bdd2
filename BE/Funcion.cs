namespace BE;

public class Funcion
{
    public const string Formato2D = "2D";
    public const string Formato3D = "3D";
    public static readonly string[] Formatos = { Formato2D, Formato3D };

    public int IdFuncion { get; set; }
    public int IdPelicula { get; set; }
    public int IdSala { get; set; }
    public DateTime FechaHora { get; set; }
    public decimal PrecioEntrada { get; set; }
    public string Formato { get; set; } = Formato2D;

    // Propiedades de navegación: EF las completa cuando se pide con Include.
    public Pelicula Pelicula { get; set; } = null!;
    public Sala Sala { get; set; } = null!;
    public List<Venta> Ventas { get; set; } = new();
}
