using BE;
using BLL;

namespace UI;

public partial class FrmVentasListado : Form
{
    private readonly VentaBLL _bll = new();
    private readonly CajaBLL _cajaBLL = new();
    private readonly PeliculaBLL _peliculaBLL = new();

    public FrmVentasListado()
    {
        InitializeComponent();
        dtpDesde.Value = DateTime.Today.AddDays(-7);
        dtpHasta.Value = DateTime.Today;
    }

    private void FrmVentasListado_Load(object sender, EventArgs e) => Ui.Intentar(() =>
    {
        // En los filtros se incluyen también las cajas y películas dadas de
        // baja: pueden tener ventas viejas.
        var cajas = new List<Opcion> { new(0, "Todas") };
        cajas.AddRange(_cajaBLL.Listar().Select(c => new Opcion(c.IdCaja, c.Nombre)));
        cboCaja.DisplayMember = nameof(Opcion.Nombre);
        cboCaja.ValueMember = nameof(Opcion.Id);
        cboCaja.DataSource = cajas;

        var peliculas = new List<Opcion> { new(0, "Todas") };
        peliculas.AddRange(_peliculaBLL.Listar().Select(p => new Opcion(p.IdPelicula, p.Titulo)));
        cboPelicula.DisplayMember = nameof(Opcion.Nombre);
        cboPelicula.ValueMember = nameof(Opcion.Id);
        cboPelicula.DataSource = peliculas;

        if (!Sesion.EsAdmin)
            Text += " (solo tus ventas)";

        Buscar();
    });

    private void Buscar()
    {
        List<Venta> ventas = _bll.Listar(
            dtpDesde.Value,
            dtpHasta.Value,
            cboCaja.SelectedValue as int? ?? 0,
            cboPelicula.SelectedValue as int? ?? 0);

        dgv.DataSource = ventas
            .Select(v => new
            {
                Id = v.IdVenta,
                Fecha = v.FechaVenta,
                Película = v.Funcion.Pelicula.Titulo,
                Función = v.Funcion.FechaHora,
                Sala = v.Funcion.Sala.Nombre,
                Caja = v.Caja.Nombre,
                Vendedor = v.Usuario.NombreUsuario,
                Entradas = v.CantidadEntradas,
                Precio = v.PrecioUnitario,
                v.Total,
                Pago = v.FormaPago
            })
            .ToList();
        dgv.Columns["Fecha"].DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";
        dgv.Columns["Función"].DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";
        dgv.Columns["Precio"].DefaultCellStyle.Format = "N2";
        dgv.Columns["Total"].DefaultCellStyle.Format = "N2";
        dgv.Columns["Id"].FillWeight = 45;
        dgv.Columns["Entradas"].FillWeight = 80;
        dgv.Columns["Fecha"].FillWeight = 135;
        dgv.Columns["Función"].FillWeight = 135;
        dgv.Columns["Película"].FillWeight = 150;
        dgv.ClearSelection();

        lblTotales.Text =
            $"Ventas: {ventas.Count}   |   Entradas: {ventas.Sum(v => v.CantidadEntradas)}   |   Recaudado: {ventas.Sum(v => v.Total):C2}";
    }

    private void btnBuscar_Click(object sender, EventArgs e) => Ui.Intentar(Buscar);
}
