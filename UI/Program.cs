using BLL;

namespace UI;

static class Program
{
    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
        // To customize application configuration such as set high DPI settings or default font,
        // see https://aka.ms/applicationconfiguration.
        ApplicationConfiguration.Initialize();

        // Red de seguridad: si algún error se escapa de los try/catch de las
        // pantallas, se muestra un mensaje en lugar de cerrar la aplicación.
        Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
        Application.ThreadException += (_, e) =>
            MessageBox.Show(
                "Ocurrió un error inesperado. La operación no se completó.\n\n" +
                (e.Exception.InnerException?.Message ?? e.Exception.Message),
                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

        // Primer uso: si no hay ningún administrador, se pide crearlo.
        try
        {
            if (!new UsuarioBLL().HayAdminActivo())
            {
                using var crearAdmin = new FrmCrearAdmin();
                if (crearAdmin.ShowDialog() != DialogResult.OK)
                    return;
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                "No se pudo conectar a la base de datos.\n" +
                "Revisá que SQL Server esté iniciado, que exista CineDB y la cadena de conexión de appsettings.json.\n\n" +
                ex.Message,
                "Error de conexión", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        // Login -> principal. Si se cierra sesión, vuelve al login.
        while (true)
        {
            using (var login = new FrmLogin())
            {
                if (login.ShowDialog() != DialogResult.OK)
                    return;
            }

            var principal = new FrmPrincipal();
            Application.Run(principal);
            Sesion.Cerrar();

            if (!principal.CerroSesion)
                return;
        }
    }
}
