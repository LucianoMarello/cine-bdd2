using BE;
using BLL;

namespace UI;

public partial class FrmFunciones : Form
{
    private readonly FuncionBLL _bll = new();
    private readonly PeliculaBLL _peliculaBLL = new();
    private readonly SalaBLL _salaBLL = new();
    private List<Funcion> _funciones = new();
    private Funcion? _seleccionada;
    private bool _cargando;

    public FrmFunciones()
    {
        InitializeComponent();
        cboFormato.Items.AddRange(Funcion.Formatos);
    }

    private void FrmFunciones_Load(object sender, EventArgs e) => Ui.Intentar(() =>
    {
        // En los combos solo se ofrecen películas y salas activas.
        cboPelicula.DisplayMember = nameof(Pelicula.Titulo);
        cboPelicula.ValueMember = nameof(Pelicula.IdPelicula);
        cboPelicula.DataSource = _peliculaBLL.Listar().Where(p => p.Estado).ToList();

        cboSala.DisplayMember = nameof(Sala.Nombre);
        cboSala.ValueMember = nameof(Sala.IdSala);
        cboSala.DataSource = _salaBLL.Listar().Where(s => s.Estado).ToList();

        Recargar();
    });

    private void Recargar()
    {
        _cargando = true;
        _funciones = _bll.Listar();
        Dictionary<int, int> vendidas = _bll.EntradasVendidasPorFuncion();

        dgv.DataSource = _funciones
            .Select(f => new
            {
                Id = f.IdFuncion,
                Película = f.Pelicula.Titulo,
                Sala = f.Sala.Nombre,
                Fecha = f.FechaHora,
                Precio = f.PrecioEntrada,
                f.Formato,
                Vendidas = vendidas.GetValueOrDefault(f.IdFuncion),
                Capacidad = f.Sala.Capacidad
            })
            .ToList();
        dgv.Columns["Fecha"].DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";
        dgv.Columns["Precio"].DefaultCellStyle.Format = "N2";
        // Reparto del ancho: más lugar para la fecha y la película, menos para el Id.
        dgv.Columns["Id"].FillWeight = 40;
        dgv.Columns["Película"].FillWeight = 160;
        dgv.Columns["Fecha"].FillWeight = 170;
        _cargando = false;
        Limpiar();
    }

    private void Limpiar()
    {
        _seleccionada = null;
        dgv.ClearSelection();
        Ui.Deseleccionar(cboPelicula);
        Ui.Deseleccionar(cboSala);
        dtpFechaHora.Value = DateTime.Today.AddDays(1).AddHours(20);
        Ui.Poner(numPrecio, 0);
        cboFormato.SelectedItem = Funcion.Formato2D;
        btnEliminar.Enabled = false;
        cboPelicula.Focus();
    }

    private void dgv_SelectionChanged(object sender, EventArgs e)
    {
        int? id = Ui.IdSeleccionado(dgv);
        if (_cargando || id == null)
            return;

        _seleccionada = _funciones.First(f => f.IdFuncion == id);
        // Si la película o la sala están dadas de baja no figuran en el combo
        // y quedan sin selección.
        cboPelicula.SelectedValue = _seleccionada.IdPelicula;
        cboSala.SelectedValue = _seleccionada.IdSala;
        dtpFechaHora.Value = _seleccionada.FechaHora;
        Ui.Poner(numPrecio, _seleccionada.PrecioEntrada);
        cboFormato.SelectedItem = _seleccionada.Formato;
        btnEliminar.Enabled = true;
    }

    private void btnNuevo_Click(object sender, EventArgs e) => Limpiar();

    private void btnGuardar_Click(object sender, EventArgs e) => Ui.Intentar(() =>
    {
        _bll.Guardar(
            _seleccionada?.IdFuncion ?? 0,
            cboPelicula.SelectedValue as int? ?? 0,
            cboSala.SelectedValue as int? ?? 0,
            dtpFechaHora.Value,
            numPrecio.Value,
            cboFormato.SelectedItem as string ?? string.Empty);
        Recargar();
    });

    private void btnEliminar_Click(object sender, EventArgs e) => Ui.Intentar(() =>
    {
        if (_seleccionada == null)
            return;

        if (!Ui.Confirmar($"¿Eliminar la función de \"{_seleccionada.Pelicula.Titulo}\" del {_seleccionada.FechaHora:dd/MM/yyyy HH:mm}?"))
            return;

        _bll.Eliminar(_seleccionada.IdFuncion);
        Recargar();
    });
}
