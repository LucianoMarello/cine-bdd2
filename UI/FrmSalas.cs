using BE;
using BLL;

namespace UI;

// Todos los ABM siguen este mismo esquema:
//   - Grilla a la izquierda, datos a la derecha.
//   - Elegir una fila carga sus datos para editar.
//   - "Nuevo" limpia el formulario; "Guardar" da de alta o modifica según
//     haya o no una fila elegida.
//   - "Dar de baja" es una baja lógica (estado = 0): no borra la fila.
public partial class FrmSalas : Form
{
    private readonly SalaBLL _bll = new();
    private List<Sala> _salas = new();
    private Sala? _seleccionada;
    private bool _cargando;

    public FrmSalas()
    {
        InitializeComponent();
    }

    private void FrmSalas_Load(object sender, EventArgs e) => Ui.Intentar(Recargar);

    private void Recargar()
    {
        _cargando = true;
        _salas = _bll.Listar();
        dgv.DataSource = _salas
            .Select(s => new { Id = s.IdSala, s.Nombre, s.Capacidad, Estado = s.Estado ? "Activa" : "Inactiva" })
            .ToList();
        _cargando = false;
        Limpiar();
    }

    private void Limpiar()
    {
        _seleccionada = null;
        dgv.ClearSelection();
        txtNombre.Clear();
        Ui.Poner(numCapacidad, numCapacidad.Minimum);
        btnEstado.Text = "Dar de baja";
        btnEstado.Enabled = false;
        txtNombre.Focus();
    }

    private void dgv_SelectionChanged(object sender, EventArgs e)
    {
        int? id = Ui.IdSeleccionado(dgv);
        if (_cargando || id == null)
            return;

        _seleccionada = _salas.First(s => s.IdSala == id);
        txtNombre.Text = _seleccionada.Nombre;
        Ui.Poner(numCapacidad, _seleccionada.Capacidad);
        btnEstado.Text = _seleccionada.Estado ? "Dar de baja" : "Reactivar";
        btnEstado.Enabled = true;
    }

    private void btnNuevo_Click(object sender, EventArgs e) => Limpiar();

    private void btnGuardar_Click(object sender, EventArgs e) => Ui.Intentar(() =>
    {
        _bll.Guardar(_seleccionada?.IdSala ?? 0, txtNombre.Text, (int)numCapacidad.Value);
        Recargar();
    });

    private void btnEstado_Click(object sender, EventArgs e) => Ui.Intentar(() =>
    {
        if (_seleccionada == null)
            return;

        bool activar = !_seleccionada.Estado;
        if (!activar && !Ui.Confirmar($"¿Dar de baja la sala \"{_seleccionada.Nombre}\"?"))
            return;

        _bll.CambiarEstado(_seleccionada.IdSala, activar);
        Recargar();
    });
}
