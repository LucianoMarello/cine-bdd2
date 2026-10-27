using BE;
using BLL;

namespace UI;

public partial class FrmCajas : Form
{
    private readonly CajaBLL _bll = new();
    private List<Caja> _cajas = new();
    private Caja? _seleccionada;
    private bool _cargando;

    public FrmCajas()
    {
        InitializeComponent();
    }

    private void FrmCajas_Load(object sender, EventArgs e) => Ui.Intentar(Recargar);

    private void Recargar()
    {
        _cargando = true;
        _cajas = _bll.Listar();
        dgv.DataSource = _cajas
            .Select(c => new { Id = c.IdCaja, c.Nombre, Estado = c.Estado ? "Activa" : "Inactiva" })
            .ToList();
        _cargando = false;
        Limpiar();
    }

    private void Limpiar()
    {
        _seleccionada = null;
        dgv.ClearSelection();
        txtNombre.Clear();
        btnEstado.Text = "Dar de baja";
        btnEstado.Enabled = false;
        txtNombre.Focus();
    }

    private void dgv_SelectionChanged(object sender, EventArgs e)
    {
        int? id = Ui.IdSeleccionado(dgv);
        if (_cargando || id == null)
            return;

        _seleccionada = _cajas.First(c => c.IdCaja == id);
        txtNombre.Text = _seleccionada.Nombre;
        btnEstado.Text = _seleccionada.Estado ? "Dar de baja" : "Reactivar";
        btnEstado.Enabled = true;
    }

    private void btnNuevo_Click(object sender, EventArgs e) => Limpiar();

    private void btnGuardar_Click(object sender, EventArgs e) => Ui.Intentar(() =>
    {
        _bll.Guardar(_seleccionada?.IdCaja ?? 0, txtNombre.Text);
        Recargar();
    });

    private void btnEstado_Click(object sender, EventArgs e) => Ui.Intentar(() =>
    {
        if (_seleccionada == null)
            return;

        bool activar = !_seleccionada.Estado;
        if (!activar && !Ui.Confirmar($"¿Dar de baja la caja \"{_seleccionada.Nombre}\"?"))
            return;

        _bll.CambiarEstado(_seleccionada.IdCaja, activar);
        Recargar();
    });
}
