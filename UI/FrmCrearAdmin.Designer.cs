namespace UI;

partial class FrmCrearAdmin
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
        lblAyuda = new Label();
        lblUsuario = new Label();
        txtUsuario = new TextBox();
        lblClave = new Label();
        txtClave = new TextBox();
        lblRepetir = new Label();
        txtRepetir = new TextBox();
        btnCrear = new Button();
        btnCancelar = new Button();
        SuspendLayout();
        //
        // lblAyuda
        //
        lblAyuda.Location = new Point(24, 16);
        lblAyuda.Name = "lblAyuda";
        lblAyuda.Size = new Size(352, 36);
        lblAyuda.TabIndex = 0;
        lblAyuda.Text = "No hay usuarios cargados. Creá el administrador del sistema.";
        //
        // lblUsuario
        //
        lblUsuario.AutoSize = true;
        lblUsuario.Location = new Point(24, 64);
        lblUsuario.Name = "lblUsuario";
        lblUsuario.Size = new Size(47, 15);
        lblUsuario.TabIndex = 1;
        lblUsuario.Text = "Usuario";
        //
        // txtUsuario
        //
        txtUsuario.Location = new Point(24, 82);
        txtUsuario.MaxLength = 50;
        txtUsuario.Name = "txtUsuario";
        txtUsuario.Size = new Size(352, 23);
        txtUsuario.TabIndex = 2;
        //
        // lblClave
        //
        lblClave.AutoSize = true;
        lblClave.Location = new Point(24, 118);
        lblClave.Name = "lblClave";
        lblClave.Size = new Size(36, 15);
        lblClave.TabIndex = 3;
        lblClave.Text = "Clave";
        //
        // txtClave
        //
        txtClave.Location = new Point(24, 136);
        txtClave.Name = "txtClave";
        txtClave.Size = new Size(352, 23);
        txtClave.TabIndex = 4;
        txtClave.UseSystemPasswordChar = true;
        //
        // lblRepetir
        //
        lblRepetir.AutoSize = true;
        lblRepetir.Location = new Point(24, 172);
        lblRepetir.Name = "lblRepetir";
        lblRepetir.Size = new Size(76, 15);
        lblRepetir.TabIndex = 5;
        lblRepetir.Text = "Repetir clave";
        //
        // txtRepetir
        //
        txtRepetir.Location = new Point(24, 190);
        txtRepetir.Name = "txtRepetir";
        txtRepetir.Size = new Size(352, 23);
        txtRepetir.TabIndex = 6;
        txtRepetir.UseSystemPasswordChar = true;
        //
        // btnCrear
        //
        btnCrear.Location = new Point(204, 236);
        btnCrear.Name = "btnCrear";
        btnCrear.Size = new Size(83, 27);
        btnCrear.TabIndex = 7;
        btnCrear.Text = "Crear";
        btnCrear.UseVisualStyleBackColor = true;
        btnCrear.Click += btnCrear_Click;
        //
        // btnCancelar
        //
        btnCancelar.DialogResult = DialogResult.Cancel;
        btnCancelar.Location = new Point(293, 236);
        btnCancelar.Name = "btnCancelar";
        btnCancelar.Size = new Size(83, 27);
        btnCancelar.TabIndex = 8;
        btnCancelar.Text = "Cancelar";
        btnCancelar.UseVisualStyleBackColor = true;
        //
        // FrmCrearAdmin
        //
        AcceptButton = btnCrear;
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        CancelButton = btnCancelar;
        ClientSize = new Size(400, 284);
        Controls.Add(btnCancelar);
        Controls.Add(btnCrear);
        Controls.Add(txtRepetir);
        Controls.Add(lblRepetir);
        Controls.Add(txtClave);
        Controls.Add(lblClave);
        Controls.Add(txtUsuario);
        Controls.Add(lblUsuario);
        Controls.Add(lblAyuda);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "FrmCrearAdmin";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Cine - Primer uso";
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private Label lblAyuda;
    private Label lblUsuario;
    private TextBox txtUsuario;
    private Label lblClave;
    private TextBox txtClave;
    private Label lblRepetir;
    private TextBox txtRepetir;
    private Button btnCrear;
    private Button btnCancelar;
}
