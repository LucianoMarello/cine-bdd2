using BLL;

namespace UI;

public partial class FrmPrincipal : Form
{
    /// <summary>True si se cerró con "Cerrar sesión": Program vuelve a mostrar el login.</summary>
    public bool CerroSesion { get; private set; }

    public FrmPrincipal()
    {
        InitializeComponent();
    }

    private void FrmPrincipal_Load(object sender, EventArgs e)
    {
        var usuario = Sesion.UsuarioActual;
        lblUsuario.Text = usuario == null
            ? "Sin sesión"
            : $"Usuario: {usuario.NombreUsuario}  |  Rol: {usuario.Rol}";

        // Permisos por rol: el Cajero solo ve Ventas.
        mnuAdministracion.Visible = Sesion.EsAdmin;
    }

    private void mnuCerrarSesion_Click(object sender, EventArgs e)
    {
        CerroSesion = true;
        Close();
    }

    private void mnuSalir_Click(object sender, EventArgs e)
    {
        Close();
    }

    private void AbrirAbm_Click(object sender, EventArgs e)
    {
        // El menú ya está oculto para el Cajero; esto es una segunda barrera.
        if (!Sesion.EsAdmin)
            return;

        using Form abm =
            sender == mnuPeliculas ? new FrmPeliculas() :
            sender == mnuSalas ? new FrmSalas() :
            sender == mnuFunciones ? new FrmFunciones() :
            sender == mnuCajas ? new FrmCajas() :
            sender == mnuObjetivos ? new FrmObjetivos() :
            new FrmUsuarios();

        abm.ShowDialog(this);
    }

    // Ventas: disponible para Admin y Cajero.
    private void AbrirVentas_Click(object sender, EventArgs e)
    {
        using Form ventas = sender == mnuNuevaVenta ? new FrmVenta() : new FrmVentasListado();
        ventas.ShowDialog(this);
    }
}
