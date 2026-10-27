namespace UI;

partial class FrmPrincipal
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
        menuPrincipal = new MenuStrip();
        mnuArchivo = new ToolStripMenuItem();
        mnuCerrarSesion = new ToolStripMenuItem();
        mnuSalir = new ToolStripMenuItem();
        mnuVentas = new ToolStripMenuItem();
        mnuNuevaVenta = new ToolStripMenuItem();
        mnuListadoVentas = new ToolStripMenuItem();
        mnuAdministracion = new ToolStripMenuItem();
        mnuPeliculas = new ToolStripMenuItem();
        mnuSalas = new ToolStripMenuItem();
        mnuFunciones = new ToolStripMenuItem();
        mnuCajas = new ToolStripMenuItem();
        mnuObjetivos = new ToolStripMenuItem();
        mnuUsuarios = new ToolStripMenuItem();
        barraEstado = new StatusStrip();
        lblUsuario = new ToolStripStatusLabel();
        menuPrincipal.SuspendLayout();
        barraEstado.SuspendLayout();
        SuspendLayout();
        //
        // menuPrincipal
        //
        menuPrincipal.Items.AddRange(new ToolStripItem[] { mnuArchivo, mnuVentas, mnuAdministracion });
        menuPrincipal.Location = new Point(0, 0);
        menuPrincipal.Name = "menuPrincipal";
        menuPrincipal.Size = new Size(800, 24);
        menuPrincipal.TabIndex = 0;
        //
        // mnuArchivo
        //
        mnuArchivo.DropDownItems.AddRange(new ToolStripItem[] { mnuCerrarSesion, mnuSalir });
        mnuArchivo.Name = "mnuArchivo";
        mnuArchivo.Size = new Size(60, 20);
        mnuArchivo.Text = "Archivo";
        //
        // mnuCerrarSesion
        //
        mnuCerrarSesion.Name = "mnuCerrarSesion";
        mnuCerrarSesion.Size = new Size(180, 22);
        mnuCerrarSesion.Text = "Cerrar sesión";
        mnuCerrarSesion.Click += mnuCerrarSesion_Click;
        //
        // mnuSalir
        //
        mnuSalir.Name = "mnuSalir";
        mnuSalir.Size = new Size(180, 22);
        mnuSalir.Text = "Salir";
        mnuSalir.Click += mnuSalir_Click;
        //
        // mnuVentas
        //
        mnuVentas.DropDownItems.AddRange(new ToolStripItem[] { mnuNuevaVenta, mnuListadoVentas });
        mnuVentas.Name = "mnuVentas";
        mnuVentas.Size = new Size(53, 20);
        mnuVentas.Text = "Ventas";
        //
        // mnuNuevaVenta
        //
        mnuNuevaVenta.Name = "mnuNuevaVenta";
        mnuNuevaVenta.Size = new Size(180, 22);
        mnuNuevaVenta.Text = "Nueva venta";
        mnuNuevaVenta.Click += AbrirVentas_Click;
        //
        // mnuListadoVentas
        //
        mnuListadoVentas.Name = "mnuListadoVentas";
        mnuListadoVentas.Size = new Size(180, 22);
        mnuListadoVentas.Text = "Listado de ventas";
        mnuListadoVentas.Click += AbrirVentas_Click;
        //
        // mnuAdministracion
        //
        mnuAdministracion.DropDownItems.AddRange(new ToolStripItem[] { mnuPeliculas, mnuSalas, mnuFunciones, mnuCajas, mnuObjetivos, mnuUsuarios });
        mnuAdministracion.Name = "mnuAdministracion";
        mnuAdministracion.Size = new Size(100, 20);
        mnuAdministracion.Text = "Administración";
        //
        // mnuPeliculas
        //
        mnuPeliculas.Name = "mnuPeliculas";
        mnuPeliculas.Size = new Size(180, 22);
        mnuPeliculas.Text = "Películas";
        mnuPeliculas.Click += AbrirAbm_Click;
        //
        // mnuSalas
        //
        mnuSalas.Name = "mnuSalas";
        mnuSalas.Size = new Size(180, 22);
        mnuSalas.Text = "Salas";
        mnuSalas.Click += AbrirAbm_Click;
        //
        // mnuFunciones
        //
        mnuFunciones.Name = "mnuFunciones";
        mnuFunciones.Size = new Size(180, 22);
        mnuFunciones.Text = "Funciones";
        mnuFunciones.Click += AbrirAbm_Click;
        //
        // mnuCajas
        //
        mnuCajas.Name = "mnuCajas";
        mnuCajas.Size = new Size(180, 22);
        mnuCajas.Text = "Cajas";
        mnuCajas.Click += AbrirAbm_Click;
        //
        // mnuObjetivos
        //
        mnuObjetivos.Name = "mnuObjetivos";
        mnuObjetivos.Size = new Size(180, 22);
        mnuObjetivos.Text = "Objetivos semanales";
        mnuObjetivos.Click += AbrirAbm_Click;
        //
        // mnuUsuarios
        //
        mnuUsuarios.Name = "mnuUsuarios";
        mnuUsuarios.Size = new Size(180, 22);
        mnuUsuarios.Text = "Usuarios";
        mnuUsuarios.Click += AbrirAbm_Click;
        //
        // barraEstado
        //
        barraEstado.Items.AddRange(new ToolStripItem[] { lblUsuario });
        barraEstado.Location = new Point(0, 428);
        barraEstado.Name = "barraEstado";
        barraEstado.Size = new Size(800, 22);
        barraEstado.TabIndex = 1;
        //
        // lblUsuario
        //
        lblUsuario.Name = "lblUsuario";
        lblUsuario.Size = new Size(60, 17);
        lblUsuario.Text = "Sin sesión";
        //
        // FrmPrincipal
        //
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(800, 450);
        Controls.Add(barraEstado);
        Controls.Add(menuPrincipal);
        MainMenuStrip = menuPrincipal;
        Name = "FrmPrincipal";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Cine - Sistema de gestión";
        Load += FrmPrincipal_Load;
        menuPrincipal.ResumeLayout(false);
        menuPrincipal.PerformLayout();
        barraEstado.ResumeLayout(false);
        barraEstado.PerformLayout();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private MenuStrip menuPrincipal;
    private ToolStripMenuItem mnuArchivo;
    private ToolStripMenuItem mnuCerrarSesion;
    private ToolStripMenuItem mnuSalir;
    private ToolStripMenuItem mnuVentas;
    private ToolStripMenuItem mnuNuevaVenta;
    private ToolStripMenuItem mnuListadoVentas;
    private ToolStripMenuItem mnuAdministracion;
    private ToolStripMenuItem mnuPeliculas;
    private ToolStripMenuItem mnuSalas;
    private ToolStripMenuItem mnuFunciones;
    private ToolStripMenuItem mnuCajas;
    private ToolStripMenuItem mnuObjetivos;
    private ToolStripMenuItem mnuUsuarios;
    private StatusStrip barraEstado;
    private ToolStripStatusLabel lblUsuario;
}
