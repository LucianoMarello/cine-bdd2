using BE;
using BLL;

namespace UI;

public partial class FrmVenta : Form
{
    private readonly VentaBLL _bll = new();
    private readonly CajaBLL _cajaBLL = new();
    private List<Funcion> _funciones = new();
    private Dictionary<int, int> _vendidas = new();
    private Funcion? _seleccionada;
    private bool _cargando;

    public FrmVenta()
    {
        InitializeComponent();
        cboFormaPago.Items.AddRange(Venta.FormasPago);
        cboFormaPago.SelectedIndex = 0;
    }

    private void FrmVenta_Load(object sender, EventArgs e) => Ui.Intentar(() =>
    {
        cboCaja.DisplayMember = nameof(Caja.Nombre);
        cboCaja.ValueMember = nameof(Caja.IdCaja);
        cboCaja.DataSource = _cajaBLL.Listar().Where(c => c.Estado).ToList();
        Ui.Deseleccionar(cboCaja);

        Recargar(null);
    });

    /// <summary>Recarga las funciones y vuelve a elegir la indicada (si sigue en venta).</summary>
    private void Recargar(int? idFuncion)
    {
        _cargando = true;
        _funciones = _bll.ListarFuncionesEnVenta();
        _vendidas = _bll.EntradasVendidasPorFuncion();

        dgv.DataSource = _funciones
            .Select(f => new
            {
                Id = f.IdFuncion,
                Película = f.Pelicula.Titulo,
                Fecha = f.FechaHora,
                Sala = f.Sala.Nombre,
                f.Formato,
                Precio = f.PrecioEntrada,
                Disponibles = Disponibles(f)
            })
            .ToList();
        dgv.Columns["Fecha"].DefaultCellStyle.Format = "ddd dd/MM HH:mm";
        dgv.Columns["Precio"].DefaultCellStyle.Format = "N2";
        dgv.Columns["Id"].FillWeight = 40;
        dgv.Columns["Película"].FillWeight = 180;
        dgv.Columns["Fecha"].FillWeight = 130;
        dgv.ClearSelection();
        _seleccionada = null;
        _cargando = false;

        if (idFuncion != null)
        {
            foreach (DataGridViewRow fila in dgv.Rows)
            {
                if ((int)fila.Cells["Id"].Value == idFuncion)
                {
                    fila.Selected = true;
                    break;
                }
            }
        }

        MostrarImportes();
    }

    private int Disponibles(Funcion f) => f.Sala.Capacidad - _vendidas.GetValueOrDefault(f.IdFuncion);

    private void MostrarImportes()
    {
        if (_seleccionada == null)
        {
            lblPrecio.Text = "-";
            lblTotal.Text = "-";
            btnVender.Enabled = false;
            return;
        }

        lblPrecio.Text = _seleccionada.PrecioEntrada.ToString("C2");
        lblTotal.Text = (numCantidad.Value * _seleccionada.PrecioEntrada).ToString("C2");
        btnVender.Enabled = Disponibles(_seleccionada) > 0;
    }

    private void dgv_SelectionChanged(object sender, EventArgs e)
    {
        if (_cargando)
            return;

        int? id = Ui.IdSeleccionado(dgv);
        _seleccionada = id == null ? null : _funciones.First(f => f.IdFuncion == id);
        MostrarImportes();
    }

    private void numCantidad_ValueChanged(object sender, EventArgs e) => MostrarImportes();

    private void btnActualizar_Click(object sender, EventArgs e) => Ui.Intentar(() => Recargar(_seleccionada?.IdFuncion));

    private void btnVender_Click(object sender, EventArgs e)
    {
        int? idFuncion = _seleccionada?.IdFuncion;

        Ui.Intentar(() =>
        {
            Venta venta = _bll.Registrar(
                idFuncion ?? 0,
                cboCaja.SelectedValue as int? ?? 0,
                (int)numCantidad.Value,
                cboFormaPago.SelectedItem as string ?? string.Empty);

            MessageBox.Show(
                $"Venta N° {venta.IdVenta} registrada.\n\n{venta.CantidadEntradas} entrada(s) - Total {venta.Total:C2}",
                "Venta registrada", MessageBoxButtons.OK, MessageBoxIcon.Information);

            numCantidad.Value = 1;
        });

        // Se recarga siempre: si la venta falló por falta de lugar, la grilla
        // tiene que mostrar el cupo real.
        Ui.Intentar(() => Recargar(idFuncion));
    }
}
