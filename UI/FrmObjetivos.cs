using BE;
using BLL;

namespace UI;

public partial class FrmObjetivos : Form
{
    private readonly ObjetivoSemanalCajaBLL _bll = new();
    private readonly CajaBLL _cajaBLL = new();
    private List<ObjetivoSemanalCaja> _objetivos = new();
    private ObjetivoSemanalCaja? _seleccionado;
    private bool _cargando;

    public FrmObjetivos()
    {
        InitializeComponent();
    }

    private void FrmObjetivos_Load(object sender, EventArgs e) => Ui.Intentar(() =>
    {
        cboCaja.DisplayMember = nameof(Caja.Nombre);
        cboCaja.ValueMember = nameof(Caja.IdCaja);
        cboCaja.DataSource = _cajaBLL.Listar().Where(c => c.Estado).ToList();

        Recargar();
    });

    private void Recargar()
    {
        _cargando = true;
        _objetivos = _bll.Listar();
        dgv.DataSource = _objetivos
            .Select(o => new
            {
                Id = o.IdObjetivo,
                Caja = o.Caja.Nombre,
                Desde = o.FechaInicio,
                Hasta = o.FechaFin,
                Objetivo = o.MontoObjetivo
            })
            .ToList();
        dgv.Columns["Desde"].DefaultCellStyle.Format = "dd/MM/yyyy";
        dgv.Columns["Hasta"].DefaultCellStyle.Format = "dd/MM/yyyy";
        dgv.Columns["Objetivo"].DefaultCellStyle.Format = "N2";
        _cargando = false;
        Limpiar();
    }

    private void Limpiar()
    {
        _seleccionado = null;
        dgv.ClearSelection();
        Ui.Deseleccionar(cboCaja);
        dtpSemana.Value = DateTime.Today;
        MostrarSemana();
        Ui.Poner(numMonto, numMonto.Minimum);
        btnEliminar.Enabled = false;
        cboCaja.Focus();
    }

    // Las semanas van de lunes a domingo: se elige cualquier día y se
    // muestra a qué semana corresponde.
    private void MostrarSemana()
    {
        DateTime lunes = ObjetivoSemanalCajaBLL.LunesDeLaSemana(dtpSemana.Value);
        lblSemana.Text = $"{lunes:dd/MM/yyyy} al {lunes.AddDays(6):dd/MM/yyyy}";
    }

    private void dtpSemana_ValueChanged(object? sender, EventArgs e) => MostrarSemana();

    private void dgv_SelectionChanged(object sender, EventArgs e)
    {
        int? id = Ui.IdSeleccionado(dgv);
        if (_cargando || id == null)
            return;

        _seleccionado = _objetivos.First(o => o.IdObjetivo == id);
        cboCaja.SelectedValue = _seleccionado.IdCaja;
        dtpSemana.Value = _seleccionado.FechaInicio;
        MostrarSemana();
        Ui.Poner(numMonto, _seleccionado.MontoObjetivo);
        btnEliminar.Enabled = true;
    }

    private void btnNuevo_Click(object sender, EventArgs e) => Limpiar();

    private void btnGuardar_Click(object sender, EventArgs e) => Ui.Intentar(() =>
    {
        _bll.Guardar(
            _seleccionado?.IdObjetivo ?? 0,
            cboCaja.SelectedValue as int? ?? 0,
            ObjetivoSemanalCajaBLL.LunesDeLaSemana(dtpSemana.Value),
            numMonto.Value);
        Recargar();
    });

    private void btnEliminar_Click(object sender, EventArgs e) => Ui.Intentar(() =>
    {
        if (_seleccionado == null)
            return;

        if (!Ui.Confirmar($"¿Eliminar el objetivo de {_seleccionado.Caja.Nombre} de la semana del {_seleccionado.FechaInicio:dd/MM/yyyy}?"))
            return;

        _bll.Eliminar(_seleccionado.IdObjetivo);
        Recargar();
    });
}
