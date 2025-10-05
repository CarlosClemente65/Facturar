using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Facturar.Presentacion
{
    public partial class frmBase : Form
    {
        private bool panelLateralVisible = false;
        private bool panelColapsado = true;

        private int anchoPanelLateralExpandido = 120; // Ancho del panel lateral cuando está visible
        private int anchoPanelLateralColapsado = 49;
        private Timer timerLateral = new Timer();
        public frmBase()
        {
            InitializeComponent();
        }

        private void CambiarEstadoPanelLateral()
        {
            if(panelLateralVisible)
            {
                // Ocultar el panel lateral
                panelLateral.Width = 40;
                panelInferiorGeneral.Location = new Point(40, panelInferiorGeneral.Location.Y);
                panelInferiorGeneral.Width = panelInferiorGeneral.Width + 160;
                panelLateralVisible = false;
            }
            else
            {
                // Mostrar el panel lateral
                panelLateral.Width = 200;
                panelInferiorGeneral.Location = new Point(200, panelInferiorGeneral.Location.Y);
                panelInferiorGeneral.Width = panelInferiorGeneral.Width - 160;
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
            // Si estamos en edición, ocultamos el panel general y mostramos el de Validar- Cancelar
            panelInferiorGeneral.Visible = !enEdicion;
            panelInferiorGeneralEditar.Visible = enEdicion;
        }

        private void BotonGeneral_Click(object sender, EventArgs e)
        {
            CambiarEstadoEdicion(true);
        }

        private void BotonEditar_Click(object sender, EventArgs e)
        {
            CambiarEstadoEdicion(false);
        }

        private void btnAbrirPanel_Click(object sender, EventArgs e)
        {
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

        private void frmBase_Load(object sender, EventArgs e)
        {
            timerLateral.Interval = 15; // Intervalo de tiempo en milisegundos
            timerLateral.Tick += TimerLateral_Tick;
            panelLateral.BringToFront();  // Traer el panel lateral por encima del central
        }

        private void TimerLateral_Tick(object sender, EventArgs e)
        {
            int velocidad = 5; // pixeles por tick
            int posicionX = 0;

            if(panelLateralVisible)
            {
                // Reducir ancho (colapsar)
                panelLateral.Width -= velocidad;
                panelInferiorGeneral.Location = new Point(
                    panelLateral.Width, 
                    panelInferiorGeneral.Location.Y);
                
                panelInferiorGeneral.Width += velocidad;
                
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
                panelInferiorGeneral.Location = new Point(
                    panelLateral.Width, 
                    panelInferiorGeneral.Location.Y);

                panelInferiorGeneral.Width -= velocidad;
                
                if(panelLateral.Width >= anchoPanelLateralExpandido)
                {
                    panelLateral.Width = anchoPanelLateralExpandido;
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
    }

}
