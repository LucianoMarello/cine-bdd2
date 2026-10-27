namespace BE;

public class Usuario
{
    public const string RolAdmin = "Admin";
    public const string RolCajero = "Cajero";

    public int IdUsuario { get; set; }
    public string NombreUsuario { get; set; } = string.Empty;
    /// <summary>Hash de la contraseña, nunca el texto plano.</summary>
    public string Clave { get; set; } = string.Empty;
    public string Rol { get; set; } = RolCajero;
    public bool Estado { get; set; } = true;

    public List<Venta> Ventas { get; set; } = new();
}
