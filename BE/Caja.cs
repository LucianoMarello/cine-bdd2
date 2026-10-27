namespace BE;

public class Caja
{
    public int IdCaja { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public bool Estado { get; set; } = true;

    public List<Venta> Ventas { get; set; } = new();
    public List<ObjetivoSemanalCaja> Objetivos { get; set; } = new();
}
