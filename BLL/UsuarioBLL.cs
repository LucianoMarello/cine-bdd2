using BE;
using DAL;

namespace BLL;

public class UsuarioBLL
{
    public const int LargoMinimoClave = 8;

    private readonly UsuarioDAL _dal = new();

    /// <summary>
    /// False en el primer uso. Se pregunta por un administrador activo y no
    /// por "algún usuario" porque los scripts de datos de prueba cargan
    /// cajeros (sin clave utilizable) antes de que exista el administrador.
    /// </summary>
    public bool HayAdminActivo() => _dal.ContarAdminsActivos() > 0;

    public List<Usuario> Listar() => _dal.Listar();

    /// <summary>
    /// Valida las credenciales y, si son correctas, deja la sesión iniciada.
    /// El mensaje de error es el mismo para usuario inexistente, clave
    /// incorrecta o usuario inactivo: así no se le da pistas a quien prueba
    /// nombres de usuario.
    /// </summary>
    public void Login(string nombreUsuario, string clave)
    {
        const string mensaje = "Usuario o clave incorrectos.";

        if (string.IsNullOrWhiteSpace(nombreUsuario) || string.IsNullOrEmpty(clave))
            throw new NegocioException("Ingresá usuario y clave.");

        Usuario? usuario = _dal.ObtenerPorNombre(nombreUsuario.Trim());

        if (usuario == null || !usuario.Estado || !HashClave.Verificar(clave, usuario.Clave))
            throw new NegocioException(mensaje);

        Sesion.Iniciar(usuario);
    }

    /// <summary>
    /// Crea el primer administrador. Solo funciona si no hay ninguno activo.
    /// </summary>
    public void CrearAdminInicial(string nombreUsuario, string clave)
    {
        if (HayAdminActivo())
            throw new NegocioException("Ya existe un administrador: el administrador inicial solo se crea una vez.");

        Crear(nombreUsuario, clave, Usuario.RolAdmin);
    }

    public void Crear(string nombreUsuario, string clave, string rol)
    {
        nombreUsuario = ValidarNombre(nombreUsuario, 0);
        ValidarClave(clave);
        ValidarRol(rol);

        _dal.Agregar(new Usuario
        {
            NombreUsuario = nombreUsuario,
            Clave = HashClave.Generar(clave),
            Rol = rol,
            Estado = true
        });
    }

    /// <summary>
    /// Modifica nombre y rol. Si nuevaClave viene vacía, la clave no cambia;
    /// si viene con texto, se reemplaza (reseteo de clave).
    /// </summary>
    public void Modificar(int id, string nombreUsuario, string rol, string? nuevaClave)
    {
        Usuario usuario = _dal.Obtener(id) ?? throw new NegocioException("El usuario ya no existe.");

        nombreUsuario = ValidarNombre(nombreUsuario, id);
        ValidarRol(rol);

        if (rol != usuario.Rol)
        {
            if (EsElUsuarioActual(id))
                throw new NegocioException("No podés cambiar tu propio rol.");
            if (usuario.Rol == Usuario.RolAdmin && usuario.Estado)
                ExigirOtroAdminActivo(id);
        }

        usuario.NombreUsuario = nombreUsuario;
        usuario.Rol = rol;

        if (!string.IsNullOrEmpty(nuevaClave))
        {
            ValidarClave(nuevaClave);
            usuario.Clave = HashClave.Generar(nuevaClave);
        }

        _dal.Modificar(usuario);
    }

    public void CambiarEstado(int id, bool activo)
    {
        Usuario usuario = _dal.Obtener(id) ?? throw new NegocioException("El usuario ya no existe.");

        if (!activo)
        {
            if (EsElUsuarioActual(id))
                throw new NegocioException("No podés darte de baja a vos mismo.");
            if (usuario.Rol == Usuario.RolAdmin && usuario.Estado)
                ExigirOtroAdminActivo(id);
        }

        usuario.Estado = activo;
        _dal.Modificar(usuario);
    }

    private static bool EsElUsuarioActual(int id) => Sesion.UsuarioActual?.IdUsuario == id;

    // Si no quedara ningún administrador activo, nadie podría volver a
    // administrar el sistema.
    private void ExigirOtroAdminActivo(int id)
    {
        if (_dal.ContarAdminsActivos(exceptoId: id) == 0)
            throw new NegocioException("Tiene que quedar al menos un administrador activo.");
    }

    private string ValidarNombre(string nombreUsuario, int id)
    {
        nombreUsuario = (nombreUsuario ?? string.Empty).Trim();

        if (nombreUsuario.Length == 0)
            throw new NegocioException("El nombre de usuario es obligatorio.");
        if (nombreUsuario.Length > 50)
            throw new NegocioException("El nombre de usuario no puede superar los 50 caracteres.");
        if (_dal.ExisteNombre(nombreUsuario, id))
            throw new NegocioException("Ya existe un usuario con ese nombre.");

        return nombreUsuario;
    }

    private static void ValidarClave(string clave)
    {
        if (string.IsNullOrEmpty(clave) || clave.Length < LargoMinimoClave)
            throw new NegocioException($"La clave debe tener al menos {LargoMinimoClave} caracteres.");
    }

    private static void ValidarRol(string rol)
    {
        if (rol != Usuario.RolAdmin && rol != Usuario.RolCajero)
            throw new NegocioException("El rol no es válido.");
    }
}
