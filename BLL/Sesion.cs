using BE;

namespace BLL;

/// <summary>
/// Usuario que inició sesión. Es estática porque la aplicación de escritorio
/// tiene un único usuario logueado a la vez.
/// </summary>
public static class Sesion
{
    public static Usuario? UsuarioActual { get; private set; }

    public static bool Iniciada => UsuarioActual != null;
    public static bool EsAdmin => UsuarioActual?.Rol == Usuario.RolAdmin;

    internal static void Iniciar(Usuario usuario) => UsuarioActual = usuario;

    public static void Cerrar() => UsuarioActual = null;
}
