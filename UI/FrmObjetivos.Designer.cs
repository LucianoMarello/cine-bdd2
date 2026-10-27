namespace UI;

partial class FrmObjetivos
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
        cboCaja = new ComboBox();
        lbl1 = new Label();
        dtpSemana = new DateTimePicker();
        lbl2 = new Label();
        lblSemana = new Label();
        lbl3 = new Label();
        numMonto = new NumericUpDown();
        btnNuevo = new Button();
        btnGuardar = new Button();
        btnEliminar = new Button();
        ((System.ComponentModel.ISupportInitialize)dgv).BeginInit();
        grpDatos.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)numMonto).BeginInit();
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
        dgv.Size = new Size(580, 396);
        dgv.TabIndex = 0;
        dgv.SelectionChanged += dgv_SelectionChanged;
        // 
        // grpDatos
        // 
        grpDatos.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        grpDatos.Controls.Add(lbl0);
        grpDatos.Controls.Add(cboCaja);
        grpDatos.Controls.Add(lbl1);
        grpDatos.Controls.Add(dtpSemana);
        grpDatos.Controls.Add(lbl2);
        grpDatos.Controls.Add(lblSemana);
        grpDatos.Controls.Add(lbl3);
        grpDatos.Controls.Add(numMonto);
        grpDatos.Location = new Point(604, 12);
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
        lbl0.Text = "Caja";
        // 
        // cboCaja
        // 
        cboCaja.DropDownStyle = ComboBoxStyle.DropDownList;
        cboCaja.FormattingEnabled = true;
        cboCaja.Location = new Point(12, 40);
        cboCaja.Name = "cboCaja";
        cboCaja.Size = new Size(240, 23);
        cboCaja.TabIndex = 1;
        // 
        // lbl1
        // 
        lbl1.AutoSize = true;
        lbl1.Location = new Point(12, 70);
        lbl1.Name = "lbl1";
        lbl1.Size = new Size(60, 15);
        lbl1.TabIndex = 2;
        lbl1.Text = "Semana (elegí cualquier día)";
        // 
        // dtpSemana
        // 
        dtpSemana.Format = DateTimePickerFormat.Short;
        dtpSemana.Location = new Point(12, 88);
        dtpSemana.Name = "dtpSemana";
        dtpSemana.Size = new Size(240, 23);
        dtpSemana.TabIndex = 3;
        dtpSemana.ValueChanged += dtpSemana_ValueChanged;
        // 
        // lbl2
        // 
        lbl2.AutoSize = true;
        lbl2.Location = new Point(12, 118);
        lbl2.Name = "lbl2";
        lbl2.Size = new Size(60, 15);
        lbl2.TabIndex = 4;
        lbl2.Text = "Período (lunes a domingo)";
        // 
        // lblSemana
        // 
        lblSemana.Location = new Point(12, 136);
        lblSemana.Name = "lblSemana";
        lblSemana.Size = new Size(240, 23);
        lblSemana.TabIndex = 5;
        lblSemana.Text = "-";
        // 
        // lbl3
        // 
        lbl3.AutoSize = true;
        lbl3.Location = new Point(12, 166);
        lbl3.Name = "lbl3";
        lbl3.Size = new Size(60, 15);
        lbl3.TabIndex = 6;
        lbl3.Text = "Monto objetivo";
        // 
        // numMonto
        // 
        numMonto.DecimalPlaces = 2;
        numMonto.Location = new Point(12, 184);
        numMonto.Maximum = new decimal(new int[] { 1000000000, 0, 0, 0 });
        numMonto.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
        numMonto.Name = "numMonto";
        numMonto.Size = new Size(240, 23);
        numMonto.TabIndex = 7;
        numMonto.ThousandsSeparator = true;
        numMonto.Value = new decimal(new int[] { 1, 0, 0, 0 });
        // 
        // btnNuevo
        // 
        btnNuevo.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnNuevo.Location = new Point(604, 250);
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
        btnGuardar.Location = new Point(686, 250);
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
        btnEliminar.Location = new Point(768, 250);
        btnEliminar.Name = "btnEliminar";
        btnEliminar.Size = new Size(100, 27);
        btnEliminar.TabIndex = 4;
        btnEliminar.Text = "Eliminar";
        btnEliminar.UseVisualStyleBackColor = true;
        btnEliminar.Click += btnEliminar_Click;
        // 
        // FrmObjetivos
        // 
        AcceptButton = btnGuardar;
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(880, 420);
        Controls.Add(btnEliminar);
        Controls.Add(btnGuardar);
        Controls.Add(btnNuevo);
        Controls.Add(grpDatos);
        Controls.Add(dgv);
        MinimizeBox = false;
        MinimumSize = new Size(896, 459);
        Name = "FrmObjetivos";
        ShowInTaskbar = false;
        StartPosition = FormStartPosition.CenterParent;
        Text = "Objetivos semanales por caja";
        Load += FrmObjetivos_Load;
        ((System.ComponentModel.ISupportInitialize)dgv).EndInit();
        grpDatos.ResumeLayout(false);
        grpDatos.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)numMonto).EndInit();
        ResumeLayout(false);
    }

    #endregion

    private DataGridView dgv;
    private GroupBox grpDatos;
    private Label lbl0;
    private ComboBox cboCaja;
    private Label lbl1;
    private DateTimePicker dtpSemana;
    private Label lbl2;
    private Label lblSemana;
    private Label lbl3;
    private NumericUpDown numMonto;
    private Button btnNuevo;
    private Button btnGuardar;
    private Button btnEliminar;
}
