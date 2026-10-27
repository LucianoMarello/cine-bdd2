namespace UI;

partial class FrmUsuarios
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
        dgv = new DataGridView();
        grpDatos = new GroupBox();
        lbl0 = new Label();
        txtUsuario = new TextBox();
        lbl1 = new Label();
        cboRol = new ComboBox();
        lbl2 = new Label();
        txtClave = new TextBox();
        lblAyudaClave = new Label();
        btnNuevo = new Button();
        btnGuardar = new Button();
        btnEstado = new Button();
        ((System.ComponentModel.ISupportInitialize)dgv).BeginInit();
        grpDatos.SuspendLayout();
        SuspendLayout();
        // 
        // dgv
        // 
        dgv.AllowUserToAddRows = false;
        dgv.AllowUserToDeleteRows = false;
        dgv.AllowUserToResizeRows = false;
        dgv.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        dgv.Location = new Point(12, 12);
        dgv.MultiSelect = false;
        dgv.Name = "dgv";
        dgv.ReadOnly = true;
        dgv.RowHeadersVisible = false;
        dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgv.Size = new Size(460, 396);
        dgv.TabIndex = 0;
        dgv.SelectionChanged += dgv_SelectionChanged;
        // 
        // grpDatos
        // 
        grpDatos.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        grpDatos.Controls.Add(lbl0);
        grpDatos.Controls.Add(txtUsuario);
        grpDatos.Controls.Add(lbl1);
        grpDatos.Controls.Add(cboRol);
        grpDatos.Controls.Add(lbl2);
        grpDatos.Controls.Add(txtClave);
        grpDatos.Controls.Add(lblAyudaClave);
        grpDatos.Location = new Point(484, 12);
        grpDatos.Name = "grpDatos";
        grpDatos.Size = new Size(264, 228);
        grpDatos.TabIndex = 1;
        grpDatos.TabStop = false;
        grpDatos.Text = "Datos";
        // 
        // lbl0
        // 
        lbl0.AutoSize = true;
        lbl0.Location = new Point(12, 22);
        lbl0.Name = "lbl0";
        lbl0.Size = new Size(60, 15);
        lbl0.TabIndex = 0;
        lbl0.Text = "Usuario";
        // 
        // txtUsuario
        // 
        txtUsuario.Location = new Point(12, 40);
        txtUsuario.MaxLength = 50;
        txtUsuario.Name = "txtUsuario";
        txtUsuario.Size = new Size(240, 23);
        txtUsuario.TabIndex = 1;
        // 
        // lbl1
        // 
        lbl1.AutoSize = true;
        lbl1.Location = new Point(12, 70);
        lbl1.Name = "lbl1";
        lbl1.Size = new Size(60, 15);
        lbl1.TabIndex = 2;
        lbl1.Text = "Rol";
        // 
        // cboRol
        // 
        cboRol.DropDownStyle = ComboBoxStyle.DropDownList;
        cboRol.FormattingEnabled = true;
        cboRol.Location = new Point(12, 88);
        cboRol.Name = "cboRol";
        cboRol.Size = new Size(240, 23);
        cboRol.TabIndex = 3;
        // 
        // lbl2
        // 
        lbl2.AutoSize = true;
        lbl2.Location = new Point(12, 118);
        lbl2.Name = "lbl2";
        lbl2.Size = new Size(60, 15);
        lbl2.TabIndex = 4;
        lbl2.Text = "Clave";
        // 
        // txtClave
        // 
        txtClave.Location = new Point(12, 136);
        txtClave.MaxLength = 100;
        txtClave.Name = "txtClave";
        txtClave.Size = new Size(240, 23);
        txtClave.TabIndex = 5;
        txtClave.UseSystemPasswordChar = true;
        // 
        // lblAyudaClave
        // 
        lblAyudaClave.Location = new Point(12, 166);
        lblAyudaClave.Name = "lblAyudaClave";
        lblAyudaClave.Size = new Size(240, 34);
        lblAyudaClave.TabIndex = 6;
        lblAyudaClave.Text = "Al editar, dejá la clave vacía para no cambiarla.";
        // 
        // btnNuevo
        // 
        btnNuevo.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnNuevo.Location = new Point(484, 250);
        btnNuevo.Name = "btnNuevo";
        btnNuevo.Size = new Size(76, 27);
        btnNuevo.TabIndex = 2;
        btnNuevo.Text = "Nuevo";
        btnNuevo.UseVisualStyleBackColor = true;
        btnNuevo.Click += btnNuevo_Click;
        // 
        // btnGuardar
        // 
        btnGuardar.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnGuardar.Location = new Point(566, 250);
        btnGuardar.Name = "btnGuardar";
        btnGuardar.Size = new Size(76, 27);
        btnGuardar.TabIndex = 3;
        btnGuardar.Text = "Guardar";
        btnGuardar.UseVisualStyleBackColor = true;
        btnGuardar.Click += btnGuardar_Click;
        // 
        // btnEstado
        // 
        btnEstado.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnEstado.Location = new Point(648, 250);
        btnEstado.Name = "btnEstado";
        btnEstado.Size = new Size(100, 27);
        btnEstado.TabIndex = 4;
        btnEstado.Text = "Dar de baja";
        btnEstado.UseVisualStyleBackColor = true;
        btnEstado.Click += btnEstado_Click;
        // 
        // FrmUsuarios
        // 
        AcceptButton = btnGuardar;
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(760, 420);
        Controls.Add(btnEstado);
        Controls.Add(btnGuardar);
        Controls.Add(btnNuevo);
        Controls.Add(grpDatos);
        Controls.Add(dgv);
        MinimizeBox = false;
        MinimumSize = new Size(776, 459);
        Name = "FrmUsuarios";
        ShowInTaskbar = false;
        StartPosition = FormStartPosition.CenterParent;
        Text = "Usuarios";
        Load += FrmUsuarios_Load;
        ((System.ComponentModel.ISupportInitialize)dgv).EndInit();
        grpDatos.ResumeLayout(false);
        grpDatos.PerformLayout();
        ResumeLayout(false);
    }

    #endregion

    private DataGridView dgv;
    private GroupBox grpDatos;
    private Label lbl0;
    private TextBox txtUsuario;
    private Label lbl1;
    private ComboBox cboRol;
    private Label lbl2;
    private TextBox txtClave;
    private Label lblAyudaClave;
    private Button btnNuevo;
    private Button btnGuardar;
    private Button btnEstado;
}
