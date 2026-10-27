namespace BE;

public class Pelicula
{
    /// <summary>
    /// Lista cerrada de géneros: evita que el mismo género quede escrito de
    /// varias formas ("Accion", "acción") y se agrupe mal en el análisis.
    /// </summary>
    public static readonly string[] Generos =
    {
        "Accion", "Animacion", "Aventura", "Ciencia ficcion", "Comedia",
        "Documental", "Drama", "Fantasia", "Romance", "Suspenso", "Terror"
    };

    public int IdPelicula { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string Genero { get; set; } = string.Empty;
    /// <summary>Duración en minutos.</summary>
    public int Duracion { get; set; }
    public bool Estado { get; set; } = true;

    public List<Funcion> Funciones { get; set; } = new();
}
