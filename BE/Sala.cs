namespace BE;

public class Sala
{
    public int IdSala { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public int Capacidad { get; set; }
    public bool Estado { get; set; } = true;

    public List<Funcion> Funciones { get; set; } = new();
}
