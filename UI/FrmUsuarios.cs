using BE;
using BLL;

namespace UI;

public partial class FrmUsuarios : Form
{
    private readonly UsuarioBLL _bll = new();
    private List<Usuario> _usuarios = new();
    private Usuario? _seleccionado;
    private bool _cargando;

    public FrmUsuarios()
    {
        InitializeComponent();
        cboRol.Items.AddRange(new object[] { Usuario.RolAdmin, Usuario.RolCajero });
    }

    private void FrmUsuarios_Load(object sender, EventArgs e) => Ui.Intentar(Recargar);

    private void Recargar()
    {
        _cargando = true;
        _usuarios = _bll.Listar();
        // La clave (hash) nunca se muestra en la grilla.
        dgv.DataSource = _usuarios
            .Select(u => new { Id = u.IdUsuario, Usuario = u.NombreUsuario, u.Rol, Estado = u.Estado ? "Activo" : "Inactivo" })
            .ToList();
        _cargando = false;
        Limpiar();
    }

    private void Limpiar()
    {
        _seleccionado = null;
        dgv.ClearSelection();
        txtUsuario.Clear();
        cboRol.SelectedItem = Usuario.RolCajero;
        txtClave.Clear();
        btnEstado.Text = "Dar de baja";
        btnEstado.Enabled = false;
        txtUsuario.Focus();
    }

    private void dgv_SelectionChanged(object sender, EventArgs e)
    {
        int? id = Ui.IdSeleccionado(dgv);
        if (_cargando || id == null)
            return;

        _seleccionado = _usuarios.First(u => u.IdUsuario == id);
        txtUsuario.Text = _seleccionado.NombreUsuario;
        cboRol.SelectedItem = _seleccionado.Rol;
        txtClave.Clear();
        btnEstado.Text = _seleccionado.Estado ? "Dar de baja" : "Reactivar";
        btnEstado.Enabled = true;
    }

    private void btnNuevo_Click(object sender, EventArgs e) => Limpiar();

    private void btnGuardar_Click(object sender, EventArgs e) => Ui.Intentar(() =>
    {
        string rol = cboRol.SelectedItem as string ?? string.Empty;

        if (_seleccionado == null)
            _bll.Crear(txtUsuario.Text, txtClave.Text, rol);
        else
            _bll.Modificar(_seleccionado.IdUsuario, txtUsuario.Text, rol, txtClave.Text);

        Recargar();
    });

    private void btnEstado_Click(object sender, EventArgs e) => Ui.Intentar(() =>
    {
        if (_seleccionado == null)
            return;

        bool activar = !_seleccionado.Estado;
        if (!activar && !Ui.Confirmar($"¿Dar de baja al usuario \"{_seleccionado.NombreUsuario}\"? No va a poder iniciar sesión."))
            return;

        _bll.CambiarEstado(_seleccionado.IdUsuario, activar);
        Recargar();
    });
}
