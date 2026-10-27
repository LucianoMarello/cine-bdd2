namespace UI;

partial class FrmVenta
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
        grpVenta = new GroupBox();
        lblCajaTitulo = new Label();
        cboCaja = new ComboBox();
        lblCantidadTitulo = new Label();
        numCantidad = new NumericUpDown();
        lblFormaPagoTitulo = new Label();
        cboFormaPago = new ComboBox();
        lblPrecioTitulo = new Label();
        lblPrecio = new Label();
        lblTotalTitulo = new Label();
        lblTotal = new Label();
        btnVender = new Button();
        btnActualizar = new Button();
        ((System.ComponentModel.ISupportInitialize)dgv).BeginInit();
        grpVenta.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)numCantidad).BeginInit();
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
        // grpVenta
        //
        grpVenta.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        grpVenta.Controls.Add(lblCajaTitulo);
        grpVenta.Controls.Add(cboCaja);
        grpVenta.Controls.Add(lblCantidadTitulo);
        grpVenta.Controls.Add(numCantidad);
        grpVenta.Controls.Add(lblFormaPagoTitulo);
        grpVenta.Controls.Add(cboFormaPago);
        grpVenta.Controls.Add(lblPrecioTitulo);
        grpVenta.Controls.Add(lblPrecio);
        grpVenta.Controls.Add(lblTotalTitulo);
        grpVenta.Controls.Add(lblTotal);
        grpVenta.Location = new Point(764, 12);
        grpVenta.Name = "grpVenta";
        grpVenta.Size = new Size(264, 284);
        grpVenta.TabIndex = 1;
        grpVenta.TabStop = false;
        grpVenta.Text = "Venta";
        //
        // lblCajaTitulo
        //
        lblCajaTitulo.AutoSize = true;
        lblCajaTitulo.Location = new Point(12, 22);
        lblCajaTitulo.Name = "lblCajaTitulo";
        lblCajaTitulo.Size = new Size(30, 15);
        lblCajaTitulo.TabIndex = 0;
        lblCajaTitulo.Text = "Caja";
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
        // lblCantidadTitulo
        //
        lblCantidadTitulo.AutoSize = true;
        lblCantidadTitulo.Location = new Point(12, 70);
        lblCantidadTitulo.Name = "lblCantidadTitulo";
        lblCantidadTitulo.Size = new Size(120, 15);
        lblCantidadTitulo.TabIndex = 2;
        lblCantidadTitulo.Text = "Cantidad de entradas";
        //
        // numCantidad
        //
        numCantidad.Location = new Point(12, 88);
        numCantidad.Maximum = new decimal(new int[] { 5000, 0, 0, 0 });
        numCantidad.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
        numCantidad.Name = "numCantidad";
        numCantidad.Size = new Size(240, 23);
        numCantidad.TabIndex = 3;
        numCantidad.Value = new decimal(new int[] { 1, 0, 0, 0 });
        numCantidad.ValueChanged += numCantidad_ValueChanged;
        //
        // lblFormaPagoTitulo
        //
        lblFormaPagoTitulo.AutoSize = true;
        lblFormaPagoTitulo.Location = new Point(12, 118);
        lblFormaPagoTitulo.Name = "lblFormaPagoTitulo";
        lblFormaPagoTitulo.Size = new Size(86, 15);
        lblFormaPagoTitulo.TabIndex = 4;
        lblFormaPagoTitulo.Text = "Forma de pago";
        //
        // cboFormaPago
        //
        cboFormaPago.DropDownStyle = ComboBoxStyle.DropDownList;
        cboFormaPago.FormattingEnabled = true;
        cboFormaPago.Location = new Point(12, 136);
        cboFormaPago.Name = "cboFormaPago";
        cboFormaPago.Size = new Size(240, 23);
        cboFormaPago.TabIndex = 5;
        //
        // lblPrecioTitulo
        //
        lblPrecioTitulo.AutoSize = true;
        lblPrecioTitulo.Location = new Point(12, 170);
        lblPrecioTitulo.Name = "lblPrecioTitulo";
        lblPrecioTitulo.Size = new Size(87, 15);
        lblPrecioTitulo.TabIndex = 6;
        lblPrecioTitulo.Text = "Precio unitario";
        //
        // lblPrecio
        //
        lblPrecio.Location = new Point(12, 188);
        lblPrecio.Name = "lblPrecio";
        lblPrecio.Size = new Size(240, 20);
        lblPrecio.TabIndex = 7;
        lblPrecio.Text = "-";
        //
        // lblTotalTitulo
        //
        lblTotalTitulo.AutoSize = true;
        lblTotalTitulo.Location = new Point(12, 216);
        lblTotalTitulo.Name = "lblTotalTitulo";
        lblTotalTitulo.Size = new Size(33, 15);
        lblTotalTitulo.TabIndex = 8;
        lblTotalTitulo.Text = "Total";
        //
        // lblTotal
        //
        lblTotal.Font = new Font("Segoe UI", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
        lblTotal.Location = new Point(12, 234);
        lblTotal.Name = "lblTotal";
        lblTotal.Size = new Size(240, 36);
        lblTotal.TabIndex = 9;
        lblTotal.Text = "-";
        //
        // btnVender
        //
        btnVender.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnVender.Location = new Point(764, 306);
        btnVender.Name = "btnVender";
        btnVender.Size = new Size(264, 36);
        btnVender.TabIndex = 2;
        btnVender.Text = "Registrar venta";
        btnVender.UseVisualStyleBackColor = true;
        btnVender.Click += btnVender_Click;
        //
        // btnActualizar
        //
        btnActualizar.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnActualizar.Location = new Point(764, 350);
        btnActualizar.Name = "btnActualizar";
        btnActualizar.Size = new Size(264, 27);
        btnActualizar.TabIndex = 3;
        btnActualizar.Text = "Actualizar disponibilidad";
        btnActualizar.UseVisualStyleBackColor = true;
        btnActualizar.Click += btnActualizar_Click;
        //
        // FrmVenta
        //
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(1040, 420);
        Controls.Add(btnActualizar);
        Controls.Add(btnVender);
        Controls.Add(grpVenta);
        Controls.Add(dgv);
        MinimizeBox = false;
        MinimumSize = new Size(1056, 459);
        Name = "FrmVenta";
        ShowInTaskbar = false;
        StartPosition = FormStartPosition.CenterParent;
        Text = "Nueva venta";
        Load += FrmVenta_Load;
        ((System.ComponentModel.ISupportInitialize)dgv).EndInit();
        grpVenta.ResumeLayout(false);
        grpVenta.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)numCantidad).EndInit();
        ResumeLayout(false);
    }

    #endregion

    private DataGridView dgv;
    private GroupBox grpVenta;
    private Label lblCajaTitulo;
    private ComboBox cboCaja;
    private Label lblCantidadTitulo;
    private NumericUpDown numCantidad;
    private Label lblFormaPagoTitulo;
    private ComboBox cboFormaPago;
    private Label lblPrecioTitulo;
    private Label lblPrecio;
    private Label lblTotalTitulo;
    private Label lblTotal;
    private Button btnVender;
    private Button btnActualizar;
}
