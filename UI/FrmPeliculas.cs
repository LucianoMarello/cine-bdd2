using BE;
using BLL;

namespace UI;

public partial class FrmPeliculas : Form
{
    private readonly PeliculaBLL _bll = new();
    private List<Pelicula> _peliculas = new();
    private Pelicula? _seleccionada;
    private bool _cargando;

    public FrmPeliculas()
    {
        InitializeComponent();
        cboGenero.Items.AddRange(Pelicula.Generos);
    }

    private void FrmPeliculas_Load(object sender, EventArgs e) => Ui.Intentar(Recargar);

    private void Recargar()
    {
        _cargando = true;
        _peliculas = _bll.Listar();
        dgv.DataSource = _peliculas
            .Select(p => new
            {
                Id = p.IdPelicula,
                Título = p.Titulo,
                Género = p.Genero,
                Minutos = p.Duracion,
                Estado = p.Estado ? "Activa" : "Inactiva"
            })
            .ToList();
        _cargando = false;
        Limpiar();
    }

    private void Limpiar()
    {
        _seleccionada = null;
        dgv.ClearSelection();
        txtTitulo.Clear();
        cboGenero.SelectedIndex = -1;
        Ui.Poner(numDuracion, 90);
        btnEstado.Text = "Dar de baja";
        btnEstado.Enabled = false;
        txtTitulo.Focus();
    }

    private void dgv_SelectionChanged(object sender, EventArgs e)
    {
        int? id = Ui.IdSeleccionado(dgv);
        if (_cargando || id == null)
            return;

        _seleccionada = _peliculas.First(p => p.IdPelicula == id);
        txtTitulo.Text = _seleccionada.Titulo;
        cboGenero.SelectedItem = _seleccionada.Genero;
        Ui.Poner(numDuracion, _seleccionada.Duracion);
        btnEstado.Text = _seleccionada.Estado ? "Dar de baja" : "Reactivar";
        btnEstado.Enabled = true;
    }

    private void btnNuevo_Click(object sender, EventArgs e) => Limpiar();

    private void btnGuardar_Click(object sender, EventArgs e) => Ui.Intentar(() =>
    {
        _bll.Guardar(_seleccionada?.IdPelicula ?? 0, txtTitulo.Text,
            cboGenero.SelectedItem as string ?? string.Empty, (int)numDuracion.Value);
        Recargar();
    });

    private void btnEstado_Click(object sender, EventArgs e) => Ui.Intentar(() =>
    {
        if (_seleccionada == null)
            return;

        bool activar = !_seleccionada.Estado;
        if (!activar && !Ui.Confirmar($"¿Dar de baja la película \"{_seleccionada.Titulo}\"?"))
            return;

        _bll.CambiarEstado(_seleccionada.IdPelicula, activar);
        Recargar();
    });
}
