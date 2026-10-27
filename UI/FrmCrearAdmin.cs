using BLL;

namespace UI;

/// <summary>
/// Se muestra solo en el primer uso, cuando todavía no hay un administrador.
/// </summary>
public partial class FrmCrearAdmin : Form
{
    private readonly UsuarioBLL _usuarioBLL = new();

    public FrmCrearAdmin()
    {
        InitializeComponent();
        lblAyuda.Text = $"Todavía no hay un administrador. Creá el administrador del sistema.\nLa clave debe tener al menos {UsuarioBLL.LargoMinimoClave} caracteres.";
    }

    private void btnCrear_Click(object sender, EventArgs e)
    {
        if (txtClave.Text != txtRepetir.Text)
        {
            MessageBox.Show("Las claves no coinciden.", "Crear administrador", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            txtRepetir.Clear();
            txtRepetir.Focus();
            return;
        }

        try
        {
            _usuarioBLL.CrearAdminInicial(txtUsuario.Text, txtClave.Text);
            MessageBox.Show("Administrador creado. Ya podés iniciar sesión.", "Crear administrador", MessageBoxButtons.OK, MessageBoxIcon.Information);
            DialogResult = DialogResult.OK;
        }
        catch (NegocioException ex)
        {
            MessageBox.Show(ex.Message, "Crear administrador", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        catch (Exception ex)
        {
            MessageBox.Show("No se pudo crear el administrador.\n\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
