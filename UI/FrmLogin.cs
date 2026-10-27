using BLL;

namespace UI;

public partial class FrmLogin : Form
{
    private readonly UsuarioBLL _usuarioBLL = new();

    public FrmLogin()
    {
        InitializeComponent();
    }

    private void btnIngresar_Click(object sender, EventArgs e)
    {
        try
        {
            _usuarioBLL.Login(txtUsuario.Text, txtClave.Text);
            DialogResult = DialogResult.OK;
        }
        catch (NegocioException ex)
        {
            MessageBox.Show(ex.Message, "Inicio de sesión", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            txtClave.Clear();
            txtClave.Focus();
        }
        catch (Exception ex)
        {
            MessageBox.Show("No se pudo iniciar sesión.\n\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
