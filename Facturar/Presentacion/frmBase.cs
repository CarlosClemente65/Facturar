using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Facturar.Presentacion.Controles;
using Facturar.Presentacion.Paneles;
using Facturar.Servicios;
using Facturar.Entidades;
using Utiles = Facturar.Utilidades.UtilidadesUI;

namespace Facturar.Presentacion
{
    public partial class frmBase : Form
    {
        private bool panelLateralVisible = false;
        private bool panelColapsado = true;

        private int anchoPanelLateralExpandido = 105; // Ancho del panel lateral cuando está visible
        private int anchoPanelLateralColapsado = 45;
        private Timer timerLateral = new Timer();

        private PanelInferior_general panelGeneral;
        private PanelInferior_Edicion panelEdicion;
        private GestorEmpresas gestorEmpresas;

        // Instancia de UserControl para empresas
        private UC_Empresas ucEmpresas = new UC_Empresas();
        public frmBase()
        {
            InitializeComponent();
        }

        private void frmBase_Load(object sender, EventArgs e)
        {
            // Variables para controlar el panel lateral al expandir y contraer
            timerLateral.Interval = 15; // Intervalo de tiempo en milisegundos
            timerLateral.Tick += TimerLateral_Tick;
            panelLateral.BringToFront();  // Traer el panel lateral por encima del central

            // Crea instancias para los paneles inferiores y los carga en el panel inferior
            panelGeneral = new PanelInferior_general();
            panelEdicion = new PanelInferior_Edicion();

            // Suscribir a los eventos de los userControl
            SuscribirEventosPanelInferior(panelGeneral);
            SuscribirEventosPanelInferior(panelEdicion);

            // Carga los dos paneles
            CargarPanelInferior(panelGeneral, dock: DockStyle.Right);
            CargarPanelInferior(panelEdicion, dock: DockStyle.Left);

            panelGeneral.Visible = false;
            panelEdicion.Visible = false;
        }

        private void CargarPanelCentral(UserControl control)
        {
            // Limpia el contenido del panel central
            panelCentral.Controls.Clear();
            panelCentral.Controls.Add(control);
        }

        private void CargarPanelInferior(UserControl panel, DockStyle dock)
        {
            panel.Dock = dock;

            // Se añade el control al contenedor
            panelInferior.Controls.Add(panel);
        }

        private void AlternarPanelInferior(UserControl mostrarPanel)
        {
            foreach(Control control in panelInferior.Controls)
            {
                if(mostrarPanel.Name == "PanelInferior_general" && control.Name == "btnInicio")
                {
                    btnInicio.Visible = true;
                    continue;
                }
                control.Visible = false;
            }

            mostrarPanel.Visible = true;

        }

        private void SuscribirEventosPanelInferior(UserControl panel)
        {
            if(panel is PanelInferior_general general)
            {
                general.AltaClicked += (s, e) => AlternarPanelInferior(panelEdicion); // Cuando se desarrolle el metodo de alta, sustituirlo en esta llamada
                general.BajaClicked += (s, e) => AlternarPanelInferior(panelEdicion); // Cuando se desarrolle el metodo de baja, sustituirlo en esta llamada
                general.EditarClicked += (s, e) =>
                {
                    AlternarPanelInferior(panelEdicion);

                    //Deshabilita el grid de empresas
                    ucEmpresas.dgvEmpresas.Enabled = false;

                    // Habilitar los TextBox y poner el foco en el primer campo
                    Utiles.HabilitarTextBoxes(contenedor: this, habilitar: true);
                };
                general.SeleccionActivos += (s, e) =>
                {
                    string estado = general.EstadoSeleccionado;
                    List<Empresa> lista;
                    bool? activas = null;
                    if(estado == "Activos")
                    {
                        activas = true;
                    }
                    else if(estado == "Inactivos")
                    {
                        activas = false;
                    }
                    lista = gestorEmpresas.ListarTodos(activas).ToList();

                    ucEmpresas?.CargarEmpresas(activas);
                };
                general.EliminarClicked += (s, e) =>
                {
                    AlternarPanelInferior(panelEdicion);
                };


            }
            else if(panel is PanelInferior_Edicion edicion)
            {
                edicion.CancelarClicked += (s, e) =>
                {
                    // Habilita el grid de empresas
                    ucEmpresas.dgvEmpresas.Enabled = true;

                    // Habilitar los TextBox y poner el foco en el primer campo
                    Utiles.HabilitarTextBoxes(contenedor: this, habilitar: false);

                    AlternarPanelInferior(panelGeneral);
                };
                edicion.ValidarClicked += (s, e) =>
                {
                    // Habilita el grid de empresas
                    ucEmpresas.dgvEmpresas.Enabled = true;

                    // Habilitar los TextBox y poner el foco en el primer campo
                    Utiles.HabilitarTextBoxes(contenedor: this, habilitar: false);

                    AlternarPanelInferior(panelGeneral);
                };
            }
        }

        private void imgMinimizar_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void imgCerrar_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }


        private void BotonGeneral_Click(object sender, EventArgs e)
        {
            //CambiarEstadoEdicion(true);
            // Estaba pensado para mostrar / ocultar los paneles inferiores
        }

        private void BotonEditar_Click(object sender, EventArgs e)
        {
            //CambiarEstadoEdicion(false);
            // Estaba pensado para mostrar / ocultar los paneles inferiores
        }

        private void btnAbrirPanel_Click(object sender, EventArgs e)
        {
            // Animacion para expandir / ocultar panel lateral
            //CambiarEstadoPanelLateral();
            timerLateral.Start();
            panelLateral.BringToFront();
            if(!panelLateralVisible)
            {
                btnEmpresas.Visible = true;
                btnClientes.Visible = true;
                btnLocales.Visible = true;
                btnContratos.Visible = true;
                btnConfigurar.Visible = true;
            }
        }


        private void TimerLateral_Tick(object sender, EventArgs e)
        {
            int velocidad = 15; // pixeles por tick
            int posicionX = 0;

            if(panelLateralVisible)
            {
                // Reducir ancho (colapsar)
                panelLateral.Width -= velocidad;
                panelInferior.Location = new Point(
                    panelLateral.Width,
                    panelInferior.Location.Y);

                panelInferior.Width += velocidad;

                AjustarPanelCentral();

                if(panelLateral.Width <= anchoPanelLateralColapsado)
                {
                    panelLateral.Width = anchoPanelLateralColapsado;
                    panelLateralVisible = false;
                    panelColapsado = true;
                    timerLateral.Stop();

                    // Al final de colapsar, ocultamos los botones
                    btnEmpresas.Visible = false;
                    btnClientes.Visible = false;
                    btnLocales.Visible = false;
                    btnContratos.Visible = false;
                    btnConfigurar.Visible = false;
                }
            }
            else
            {
                // Aumentar ancho (expandir)
                panelLateral.Width += velocidad;
                panelInferior.Location = new Point(
                    panelLateral.Width,
                    panelInferior.Location.Y);

                panelInferior.Width -= velocidad;

                AjustarPanelCentral();

                if(panelLateral.Width >= anchoPanelLateralExpandido)
                {
                    //panelLateral.Width = anchoPanelLateralExpandido;
                    panelLateralVisible = true;
                    panelColapsado = false;
                    timerLateral.Stop();
                }
            }


            posicionX = panelLateral.Width / 2;
            btnAbrirPanel.Location = new Point(posicionX - (btnAbrirPanel.Width / 2), btnAbrirPanel.Location.Y);
            btnEmpresas.Location = new Point(posicionX - (btnEmpresas.Width / 2), btnEmpresas.Location.Y);
            btnClientes.Location = new Point(posicionX - (btnClientes.Width / 2), btnClientes.Location.Y);
            btnLocales.Location = new Point(posicionX - (btnLocales.Width / 2), btnLocales.Location.Y);
            btnContratos.Location = new Point(posicionX - (btnContratos.Width / 2), btnContratos.Location.Y);
            btnConfigurar.Location = new Point(posicionX - (btnConfigurar.Width / 2), btnConfigurar.Location.Y);
        }

        private void AjustarPanelCentral()
        {
            int left = panelLateralVisible ? panelLateral.Width : panelLateral.Width;
            int top = panelSuperior.Height + 4;
            int width = this.ClientSize.Width - left - 4;
            int height = this.ClientSize.Height - top - panelInferior.Height;

            panelCentral.Location = new Point(left, top);
            panelCentral.Size = new Size(width, height);
        }


        private void btnEmpresas_Click(object sender, EventArgs e)
        {
            ucEmpresas.Dock = DockStyle.Fill;
            btnAbrirPanel_Click(btnAbrirPanel, EventArgs.Empty);
            panelGeneral.Visible = true;
            panelGeneral.MostrarActivos(visible: true);
            CargarPanelCentral(ucEmpresas);
        }

        private void btnClientes_Click(object sender, EventArgs e)
        {
            var ucClientes = new UC_Clientes();
            ucClientes.Dock = DockStyle.Fill;
            btnAbrirPanel_Click(btnAbrirPanel, EventArgs.Empty);
            CargarPanelCentral(ucClientes);
        }

        private void btnContratos_Click(object sender, EventArgs e)
        {
            var ucContratos = new UC_Contratos();
            ucContratos.Dock = DockStyle.Fill;
            btnAbrirPanel_Click(btnAbrirPanel, EventArgs.Empty);
            CargarPanelCentral(ucContratos);
        }

        private void btnLocales_Click(object sender, EventArgs e)
        {
            var ucLocales = new UC_Locales();
            ucLocales.Dock = DockStyle.Fill;
            btnAbrirPanel_Click(btnAbrirPanel, EventArgs.Empty);
            CargarPanelCentral(ucLocales);
        }

        private void btnConfigurar_Click(object sender, EventArgs e)
        {
            var ucConfiguracion = new UC_Configuracion();
            ucConfiguracion.Dock = DockStyle.Fill;
            btnAbrirPanel_Click(btnAbrirPanel, EventArgs.Empty);
            CargarPanelCentral(ucConfiguracion);
        }

        private void btnInicio_Click(object sender, EventArgs e)
        {
            panelCentral.Controls.Clear();
            panelGeneral.Visible = false;
        }
    }

}
