namespace UI;

partial class FrmVentasListado
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
        lblDesde = new Label();
        dtpDesde = new DateTimePicker();
        lblHasta = new Label();
        dtpHasta = new DateTimePicker();
        lblCaja = new Label();
        cboCaja = new ComboBox();
        lblPelicula = new Label();
        cboPelicula = new ComboBox();
        btnBuscar = new Button();
        dgv = new DataGridView();
        lblTotales = new Label();
        ((System.ComponentModel.ISupportInitialize)dgv).BeginInit();
        SuspendLayout();
        //
        // lblDesde
        //
        lblDesde.AutoSize = true;
        lblDesde.Location = new Point(12, 16);
        lblDesde.Name = "lblDesde";
        lblDesde.Size = new Size(39, 15);
        lblDesde.TabIndex = 0;
        lblDesde.Text = "Desde";
        //
        // dtpDesde
        //
        dtpDesde.Format = DateTimePickerFormat.Short;
        dtpDesde.Location = new Point(60, 12);
        dtpDesde.Name = "dtpDesde";
        dtpDesde.Size = new Size(110, 23);
        dtpDesde.TabIndex = 1;
        //
        // lblHasta
        //
        lblHasta.AutoSize = true;
        lblHasta.Location = new Point(186, 16);
        lblHasta.Name = "lblHasta";
        lblHasta.Size = new Size(37, 15);
        lblHasta.TabIndex = 2;
        lblHasta.Text = "Hasta";
        //
        // dtpHasta
        //
        dtpHasta.Format = DateTimePickerFormat.Short;
        dtpHasta.Location = new Point(232, 12);
        dtpHasta.Name = "dtpHasta";
        dtpHasta.Size = new Size(110, 23);
        dtpHasta.TabIndex = 3;
        //
        // lblCaja
        //
        lblCaja.AutoSize = true;
        lblCaja.Location = new Point(360, 16);
        lblCaja.Name = "lblCaja";
        lblCaja.Size = new Size(30, 15);
        lblCaja.TabIndex = 4;
        lblCaja.Text = "Caja";
        //
        // cboCaja
        //
        cboCaja.DropDownStyle = ComboBoxStyle.DropDownList;
        cboCaja.FormattingEnabled = true;
        cboCaja.Location = new Point(398, 12);
        cboCaja.Name = "cboCaja";
        cboCaja.Size = new Size(150, 23);
        cboCaja.TabIndex = 5;
        //
        // lblPelicula
        //
        lblPelicula.AutoSize = true;
        lblPelicula.Location = new Point(566, 16);
        lblPelicula.Name = "lblPelicula";
        lblPelicula.Size = new Size(48, 15);
        lblPelicula.TabIndex = 6;
        lblPelicula.Text = "Película";
        //
        // cboPelicula
        //
        cboPelicula.DropDownStyle = ComboBoxStyle.DropDownList;
        cboPelicula.FormattingEnabled = true;
        cboPelicula.Location = new Point(624, 12);
        cboPelicula.Name = "cboPelicula";
        cboPelicula.Size = new Size(240, 23);
        cboPelicula.TabIndex = 7;
        //
        // btnBuscar
        //
        btnBuscar.Location = new Point(880, 10);
        btnBuscar.Name = "btnBuscar";
        btnBuscar.Size = new Size(90, 27);
        btnBuscar.TabIndex = 8;
        btnBuscar.Text = "Buscar";
        btnBuscar.UseVisualStyleBackColor = true;
        btnBuscar.Click += btnBuscar_Click;
        //
        // dgv
        //
        dgv.AllowUserToAddRows = false;
        dgv.AllowUserToDeleteRows = false;
        dgv.AllowUserToResizeRows = false;
        dgv.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        dgv.Location = new Point(12, 48);
        dgv.MultiSelect = false;
        dgv.Name = "dgv";
        dgv.ReadOnly = true;
        dgv.RowHeadersVisible = false;
        dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgv.Size = new Size(1076, 430);
        dgv.TabIndex = 9;
        //
        // lblTotales
        //
        lblTotales.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        lblTotales.AutoSize = true;
        lblTotales.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblTotales.Location = new Point(12, 490);
        lblTotales.Name = "lblTotales";
        lblTotales.Size = new Size(53, 15);
        lblTotales.TabIndex = 10;
        lblTotales.Text = "Ventas: 0";
        //
        // FrmVentasListado
        //
        AcceptButton = btnBuscar;
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(1100, 516);
        Controls.Add(lblTotales);
        Controls.Add(dgv);
        Controls.Add(btnBuscar);
        Controls.Add(cboPelicula);
        Controls.Add(lblPelicula);
        Controls.Add(cboCaja);
        Controls.Add(lblCaja);
        Controls.Add(dtpHasta);
        Controls.Add(lblHasta);
        Controls.Add(dtpDesde);
        Controls.Add(lblDesde);
        MinimizeBox = false;
        MinimumSize = new Size(1016, 400);
        Name = "FrmVentasListado";
        ShowInTaskbar = false;
        StartPosition = FormStartPosition.CenterParent;
        Text = "Listado de ventas";
        Load += FrmVentasListado_Load;
        ((System.ComponentModel.ISupportInitialize)dgv).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private Label lblDesde;
    private DateTimePicker dtpDesde;
    private Label lblHasta;
    private DateTimePicker dtpHasta;
    private Label lblCaja;
    private ComboBox cboCaja;
    private Label lblPelicula;
    private ComboBox cboPelicula;
    private Button btnBuscar;
    private DataGridView dgv;
    private Label lblTotales;
}
