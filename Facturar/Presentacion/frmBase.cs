using System;
using System.Drawing;
using System.Windows.Forms;
using Facturar.Presentacion.Controles;
using Facturar.Presentacion.Paneles;

namespace Facturar.Presentacion
{
    public partial class frmBase : Form
    {
        private bool panelLateralVisible = false;
        private bool panelColapsado = true;

        private int anchoPanelLateralExpandido = 100; // Ancho del panel lateral cuando está visible
        private int anchoPanelLateralColapsado = 45;
        private Timer timerLateral = new Timer();

        private PanelInferiorGeneral panelGeneral;
        private PanelInferiorEdicion panelEdicion;
        public frmBase()
        {
            InitializeComponent();
        }

        private void frmBase_Load(object sender, EventArgs e)
        {

            timerLateral.Interval = 15; // Intervalo de tiempo en milisegundos
            timerLateral.Tick += TimerLateral_Tick;
            panelLateral.BringToFront();  // Traer el panel lateral por encima del central

            // Crear instancia para el panel inferior general y lo carga en el panel inferior
            panelGeneral = new PanelInferiorGeneral();
            panelEdicion = new PanelInferiorEdicion();

            // Suscribir a los eventos de los userControl
            SuscribirEventosPanelInferior(panelGeneral);
            SuscribirEventosPanelInferior(panelEdicion);

            // Carga los dos paneles
            CargarPanelInferior(panelGeneral, dock: DockStyle.Right);
            CargarPanelInferior(panelEdicion, dock: DockStyle.Left);

            panelGeneral.Visible = true;
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
            //panel.Location = new Point(0, 0);
            //control.Size = panelInferior.ClientSize;
            //control.Anchor = AnchorStyles.Top | AnchorStyles.Right;

            // Se añade el control al contenedor
            panelInferior.Controls.Add(panel);
        }

        private void AlternarPanelInferior(UserControl mostrarPanel)
        {
            foreach(Control control in panelInferior.Controls)
            {
                control.Visible = false;
            }

            mostrarPanel.Visible = true;
                
        }

        private void SuscribirEventosPanelInferior(UserControl panel)
        {
            if (panel is PanelInferiorGeneral general)
            {
                general.AltaClicked += (s, e) => AlternarPanelInferior(panelEdicion); // Cuando se desarrolle el metodo de alta, sustituirlo en esta llamada
                general.BajaClicked += (s, e) => AlternarPanelInferior(panelEdicion); // Cuando se desarrolle el metodo de baja, sustituirlo en esta llamada
                general.EditarClicked += (s, e) => AlternarPanelInferior(panelEdicion);
            }
            else if (panel is PanelInferiorEdicion edicion)
            {
                edicion.CancelarClicked += (s,e) => AlternarPanelInferior(panelGeneral);
                edicion.ValidarClicked+= (s, e) => AlternarPanelInferior(panelGeneral);
            }
        }

        private void CambiarEstadoPanelLateral()
        {
            if(panelLateralVisible)
            {
                // Ocultar el panel lateral
                panelLateral.Width = 40;
                panelInferior.Location = new Point(40, panelInferior.Location.Y);
                panelInferior.Width = panelInferior.Width + 160;
                panelLateralVisible = false;
            }
            else
            {
                // Mostrar el panel lateral
                panelLateral.Width = 200;
                panelInferior.Location = new Point(200, panelInferior.Location.Y);
                panelInferior.Width = panelInferior.Width - 160;
                panelLateralVisible = true;
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

        private void CambiarEstadoEdicion(bool enEdicion)
        {
            // Este metodo estaba pensado cuando habia dos paneles, uno general y otro para edicion, pero supongo que habra que mostrar el userControl del panel inferior que corresponda.
            
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
            }
        }

        

        private void TimerLateral_Tick(object sender, EventArgs e)
        {
            int velocidad = 5; // pixeles por tick
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
            btnAbrirPanel.Location = new Point(posicionX - (btnAbrirPanel.Width / 2),btnAbrirPanel.Location.Y);
            btnEmpresas.Location = new Point(posicionX - (btnEmpresas.Width / 2),btnEmpresas.Location.Y);
            btnClientes.Location = new Point(posicionX - (btnClientes.Width / 2),btnClientes.Location.Y);
            btnLocales.Location = new Point(posicionX - (btnLocales.Width / 2),btnLocales.Location.Y);
        }

        private void AjustarPanelCentral()
        {
            int left = panelLateralVisible ? panelLateral.Width : panelLateral.Width;
            int top = panelSuperior.Height;
            int width = this.ClientSize.Width - left;
            int height = this.ClientSize.Height - top - panelInferior.Height;

            panelCentral.Location = new Point(left, top);
            panelCentral.Size = new Size(width, height);
        }


        private void btnEmpresas_Click(object sender, EventArgs e)
        {
            var ucEmpresas = new UC_Empresas();
            btnAbrirPanel_Click(btnAbrirPanel, EventArgs .Empty);
            CargarPanelCentral(ucEmpresas);
        }
    }

}
