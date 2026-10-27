namespace BE;

public class ObjetivoSemanalCaja
{
    public int IdObjetivo { get; set; }
    public int IdCaja { get; set; }
    public DateTime FechaInicio { get; set; }
    public DateTime FechaFin { get; set; }
    public decimal MontoObjetivo { get; set; }

    public Caja Caja { get; set; } = null!;
}
