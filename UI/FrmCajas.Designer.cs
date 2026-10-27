namespace UI;

partial class FrmCajas
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
        txtNombre = new TextBox();
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
        grpDatos.Controls.Add(txtNombre);
        grpDatos.Location = new Point(484, 12);
        grpDatos.Name = "grpDatos";
        grpDatos.Size = new Size(264, 84);
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
        lbl0.Text = "Nombre";
        // 
        // txtNombre
        // 
        txtNombre.Location = new Point(12, 40);
        txtNombre.MaxLength = 30;
        txtNombre.Name = "txtNombre";
        txtNombre.Size = new Size(240, 23);
        txtNombre.TabIndex = 1;
        // 
        // btnNuevo
        // 
        btnNuevo.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnNuevo.Location = new Point(484, 106);
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
        btnGuardar.Location = new Point(566, 106);
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
        btnEstado.Location = new Point(648, 106);
        btnEstado.Name = "btnEstado";
        btnEstado.Size = new Size(100, 27);
        btnEstado.TabIndex = 4;
        btnEstado.Text = "Dar de baja";
        btnEstado.UseVisualStyleBackColor = true;
        btnEstado.Click += btnEstado_Click;
        // 
        // FrmCajas
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
        Name = "FrmCajas";
        ShowInTaskbar = false;
        StartPosition = FormStartPosition.CenterParent;
        Text = "Cajas";
        Load += FrmCajas_Load;
        ((System.ComponentModel.ISupportInitialize)dgv).EndInit();
        grpDatos.ResumeLayout(false);
        grpDatos.PerformLayout();
        ResumeLayout(false);
    }

    #endregion

    private DataGridView dgv;
    private GroupBox grpDatos;
    private Label lbl0;
    private TextBox txtNombre;
    private Button btnNuevo;
    private Button btnGuardar;
    private Button btnEstado;
}
