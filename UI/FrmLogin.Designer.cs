namespace UI;

partial class FrmLogin
{
    /// <summary>
    ///  Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    ///  Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    ///  Required method for Designer support - do not modify
    ///  the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        lblUsuario = new Label();
        txtUsuario = new TextBox();
        lblClave = new Label();
        txtClave = new TextBox();
        btnIngresar = new Button();
        btnSalir = new Button();
        SuspendLayout();
        //
        // lblUsuario
        //
        lblUsuario.AutoSize = true;
        lblUsuario.Location = new Point(24, 24);
        lblUsuario.Name = "lblUsuario";
        lblUsuario.Size = new Size(47, 15);
        lblUsuario.TabIndex = 0;
        lblUsuario.Text = "Usuario";
        //
        // txtUsuario
        //
        txtUsuario.Location = new Point(24, 42);
        txtUsuario.MaxLength = 50;
        txtUsuario.Name = "txtUsuario";
        txtUsuario.Size = new Size(272, 23);
        txtUsuario.TabIndex = 1;
        //
        // lblClave
        //
        lblClave.AutoSize = true;
        lblClave.Location = new Point(24, 80);
        lblClave.Name = "lblClave";
        lblClave.Size = new Size(36, 15);
        lblClave.TabIndex = 2;
        lblClave.Text = "Clave";
        //
        // txtClave
        //
        txtClave.Location = new Point(24, 98);
        txtClave.Name = "txtClave";
        txtClave.Size = new Size(272, 23);
        txtClave.TabIndex = 3;
        txtClave.UseSystemPasswordChar = true;
        //
        // btnIngresar
        //
        btnIngresar.Location = new Point(124, 144);
        btnIngresar.Name = "btnIngresar";
        btnIngresar.Size = new Size(83, 27);
        btnIngresar.TabIndex = 4;
        btnIngresar.Text = "Ingresar";
        btnIngresar.UseVisualStyleBackColor = true;
        btnIngresar.Click += btnIngresar_Click;
        //
        // btnSalir
        //
        btnSalir.DialogResult = DialogResult.Cancel;
        btnSalir.Location = new Point(213, 144);
        btnSalir.Name = "btnSalir";
        btnSalir.Size = new Size(83, 27);
        btnSalir.TabIndex = 5;
        btnSalir.Text = "Salir";
        btnSalir.UseVisualStyleBackColor = true;
        //
        // FrmLogin
        //
        AcceptButton = btnIngresar;
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        CancelButton = btnSalir;
        ClientSize = new Size(320, 192);
        Controls.Add(btnSalir);
        Controls.Add(btnIngresar);
        Controls.Add(txtClave);
        Controls.Add(lblClave);
        Controls.Add(txtUsuario);
        Controls.Add(lblUsuario);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "FrmLogin";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Cine - Iniciar sesión";
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private Label lblUsuario;
    private TextBox txtUsuario;
    private Label lblClave;
    private TextBox txtClave;
    private Button btnIngresar;
    private Button btnSalir;
}
