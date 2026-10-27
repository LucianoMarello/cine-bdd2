namespace UI;

partial class FrmFunciones
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
        cboPelicula = new ComboBox();
        lbl1 = new Label();
        cboSala = new ComboBox();
        lbl2 = new Label();
        dtpFechaHora = new DateTimePicker();
        lbl3 = new Label();
        numPrecio = new NumericUpDown();
        lbl4 = new Label();
        cboFormato = new ComboBox();
        btnNuevo = new Button();
        btnGuardar = new Button();
        btnEliminar = new Button();
        ((System.ComponentModel.ISupportInitialize)dgv).BeginInit();
        grpDatos.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)numPrecio).BeginInit();
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
        dgv.Size = new Size(740, 396);
        dgv.TabIndex = 0;
        dgv.SelectionChanged += dgv_SelectionChanged;
        // 
        // grpDatos
        // 
        grpDatos.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        grpDatos.Controls.Add(lbl0);
        grpDatos.Controls.Add(cboPelicula);
        grpDatos.Controls.Add(lbl1);
        grpDatos.Controls.Add(cboSala);
        grpDatos.Controls.Add(lbl2);
        grpDatos.Controls.Add(dtpFechaHora);
        grpDatos.Controls.Add(lbl3);
        grpDatos.Controls.Add(numPrecio);
        grpDatos.Controls.Add(lbl4);
        grpDatos.Controls.Add(cboFormato);
        grpDatos.Location = new Point(764, 12);
        grpDatos.Name = "grpDatos";
        grpDatos.Size = new Size(264, 276);
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
        lbl0.Text = "Película";
        // 
        // cboPelicula
        // 
        cboPelicula.DropDownStyle = ComboBoxStyle.DropDownList;
        cboPelicula.FormattingEnabled = true;
        cboPelicula.Location = new Point(12, 40);
        cboPelicula.Name = "cboPelicula";
        cboPelicula.Size = new Size(240, 23);
        cboPelicula.TabIndex = 1;
        // 
        // lbl1
        // 
        lbl1.AutoSize = true;
        lbl1.Location = new Point(12, 70);
        lbl1.Name = "lbl1";
        lbl1.Size = new Size(60, 15);
        lbl1.TabIndex = 2;
        lbl1.Text = "Sala";
        // 
        // cboSala
        // 
        cboSala.DropDownStyle = ComboBoxStyle.DropDownList;
        cboSala.FormattingEnabled = true;
        cboSala.Location = new Point(12, 88);
        cboSala.Name = "cboSala";
        cboSala.Size = new Size(240, 23);
        cboSala.TabIndex = 3;
        // 
        // lbl2
        // 
        lbl2.AutoSize = true;
        lbl2.Location = new Point(12, 118);
        lbl2.Name = "lbl2";
        lbl2.Size = new Size(60, 15);
        lbl2.TabIndex = 4;
        lbl2.Text = "Fecha y hora";
        // 
        // dtpFechaHora
        // 
        dtpFechaHora.CustomFormat = "dd/MM/yyyy HH:mm";
        dtpFechaHora.Format = DateTimePickerFormat.Custom;
        dtpFechaHora.Location = new Point(12, 136);
        dtpFechaHora.Name = "dtpFechaHora";
        dtpFechaHora.Size = new Size(240, 23);
        dtpFechaHora.TabIndex = 5;
        // 
        // lbl3
        // 
        lbl3.AutoSize = true;
        lbl3.Location = new Point(12, 166);
        lbl3.Name = "lbl3";
        lbl3.Size = new Size(60, 15);
        lbl3.TabIndex = 6;
        lbl3.Text = "Precio de la entrada";
        // 
        // numPrecio
        // 
        numPrecio.DecimalPlaces = 2;
        numPrecio.Location = new Point(12, 184);
        numPrecio.Maximum = new decimal(new int[] { 10000000, 0, 0, 0 });
        numPrecio.Name = "numPrecio";
        numPrecio.Size = new Size(240, 23);
        numPrecio.TabIndex = 7;
        numPrecio.ThousandsSeparator = true;
        // 
        // lbl4
        // 
        lbl4.AutoSize = true;
        lbl4.Location = new Point(12, 214);
        lbl4.Name = "lbl4";
        lbl4.Size = new Size(60, 15);
        lbl4.TabIndex = 8;
        lbl4.Text = "Formato";
        // 
        // cboFormato
        // 
        cboFormato.DropDownStyle = ComboBoxStyle.DropDownList;
        cboFormato.FormattingEnabled = true;
        cboFormato.Location = new Point(12, 232);
        cboFormato.Name = "cboFormato";
        cboFormato.Size = new Size(240, 23);
        cboFormato.TabIndex = 9;
        // 
        // btnNuevo
        // 
        btnNuevo.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnNuevo.Location = new Point(764, 298);
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
        btnGuardar.Location = new Point(846, 298);
        btnGuardar.Name = "btnGuardar";
        btnGuardar.Size = new Size(76, 27);
        btnGuardar.TabIndex = 3;
        btnGuardar.Text = "Guardar";
        btnGuardar.UseVisualStyleBackColor = true;
        btnGuardar.Click += btnGuardar_Click;
        // 
        // btnEliminar
        // 
        btnEliminar.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnEliminar.Location = new Point(928, 298);
        btnEliminar.Name = "btnEliminar";
        btnEliminar.Size = new Size(100, 27);
        btnEliminar.TabIndex = 4;
        btnEliminar.Text = "Eliminar";
        btnEliminar.UseVisualStyleBackColor = true;
        btnEliminar.Click += btnEliminar_Click;
        // 
        // FrmFunciones
        // 
        AcceptButton = btnGuardar;
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(1040, 420);
        Controls.Add(btnEliminar);
        Controls.Add(btnGuardar);
        Controls.Add(btnNuevo);
        Controls.Add(grpDatos);
        Controls.Add(dgv);
        MinimizeBox = false;
        MinimumSize = new Size(1056, 459);
        Name = "FrmFunciones";
        ShowInTaskbar = false;
        StartPosition = FormStartPosition.CenterParent;
        Text = "Funciones";
        Load += FrmFunciones_Load;
        ((System.ComponentModel.ISupportInitialize)dgv).EndInit();
        grpDatos.ResumeLayout(false);
        grpDatos.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)numPrecio).EndInit();
        ResumeLayout(false);
    }

    #endregion

    private DataGridView dgv;
    private GroupBox grpDatos;
    private Label lbl0;
    private ComboBox cboPelicula;
    private Label lbl1;
    private ComboBox cboSala;
    private Label lbl2;
    private DateTimePicker dtpFechaHora;
    private Label lbl3;
    private NumericUpDown numPrecio;
    private Label lbl4;
    private ComboBox cboFormato;
    private Button btnNuevo;
    private Button btnGuardar;
    private Button btnEliminar;
}
