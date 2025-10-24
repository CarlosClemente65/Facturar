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
        // Variables para el control del panel lateral
        private bool panelLateralVisible = false;
        private bool panelColapsado = true;
        private int anchoPanelLateralExpandido = 105; // Ancho del panel lateral cuando está visible
        private int anchoPanelLateralColapsado = 45;
        private Timer timerLateral = new Timer();


        // Variables de clase para gestionar paneles y entidades
        private PanelInferior_general panelGeneral;
        private PanelInferior_Edicion panelEdicion;
        private GestorEmpresas gestorEmpresas;
        private GestorLocales gestorLocales;
        private GestorClientes gestorClientes;
        private GestorContratos gestorContratos;
        private GestorConfiguracion gestorConfiguracion;


        // Instancias de UserControl para empresas
        private UC_Empresas ucEmpresas = new UC_Empresas();
        private UC_Locales ucLocales = new UC_Locales();
        private UC_Clientes ucClientes = new UC_Clientes();
        private UC_Contratos ucContratos = new UC_Contratos();
        private UC_Configuracion ucConfiguracion = new UC_Configuracion();

        // Control de entidad cargada en el panel central
        private UserControl panelCentralActivo; // Permite despues acceder acceder al panel para habilitar controles o refrescar el grid
        private TipoEntidad entidadActiva = TipoEntidad.Ninguno; // Al inicio no se ha cargado ninguna
        private object gestorActual; // Almacena el gestor que debe gestionarse en el formulario (se cambia al acceder a las opciones de cada tipo de entidad)

        public enum TipoEntidad
        {
            Ninguno,
            Empresa,
            Cliente,
            Local,
            Contrato,
            Configurar
        }

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
            gestorEmpresas = new GestorEmpresas(); // Instancia para acceder a los metodos de empresas

            // Suscribir a los eventos de los userControl
            SuscribirEventosPanelInferior(panelGeneral);
            SuscribirEventosPanelInferior(panelEdicion);

            // Carga los dos paneles
            CargarPanelInferior(panelGeneral, dock: DockStyle.Right);
            CargarPanelInferior(panelEdicion, dock: DockStyle.Left);

            panelGeneral.Visible = false;
            panelEdicion.Visible = false;
        }

        private void CargarPanelCentral(UserControl panel, TipoEntidad tipo)
        {
            // Limpia el contenido del panel central
            panelCentral.Controls.Clear();
            panelCentral.Controls.Add(panel);

            // Control de tipo de entidad cargada en el panel
            panelCentralActivo = panel;
            entidadActiva = tipo;

            switch(entidadActiva)
            {
                case TipoEntidad.Empresa:
                    gestorActual = gestorEmpresas;
                    break;

                case TipoEntidad.Local:
                    gestorActual = gestorLocales;
                    break;

                case TipoEntidad.Cliente:
                    gestorActual = gestorClientes;
                    break;

                case TipoEntidad.Contrato:
                    gestorActual = gestorContratos;
                    break;

                case TipoEntidad.Configurar:
                    gestorActual = gestorConfiguracion;
                    break;
            }
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

        /// <summary>
        /// Suscribe los eventos de los paneles inferiores (general y edicion)
        /// y define el comportamiento segun la entidad activa y el gestor actual
        /// </summary>
        /// <param name="panel"></param>
        private void SuscribirEventosPanelInferior(UserControl panel)
        {
            if(panel is PanelInferior_general general)
            {
                // Procesos de alta
                general.AltaClicked += (s, e) =>
                {
                    AlternarPanelInferior(panelEdicion); // Cuando se desarrolle el metodo de alta, sustituirlo en esta llamada
                    Utiles.HabilitarTextBoxes(contenedor: this, habilitar: true); // Activa los campos para la entrada de datos 

                    switch(entidadActiva)
                    {
                        case TipoEntidad.Empresa:
                            //gestorEmpresas.Agregar(); // Pendiente de desarrollo
                            break;

                        case TipoEntidad.Local:
                            //gestorLocales.Agregar(); // Pendiente de desarrollo
                            break;

                        case TipoEntidad.Cliente:
                            //gestorClientes.Agregar(); // Pendiente de desarrollo
                            break;

                        case TipoEntidad.Contrato:
                            //gestorContratos.Agregar(); // Pendiente de desarrollo
                            break;
                    }
                };

                // Procesos de baja
                general.BajaClicked += (s, e) =>
                {
                    AlternarPanelInferior(panelEdicion); // Cuando se desarrolle el metodo de baja, sustituirlo en esta llamada

                    switch(entidadActiva)
                    {
                        case TipoEntidad.Empresa:
                            //gestorEmpresas.Baja(); // Pendiente de desarrollo
                            break;

                        case TipoEntidad.Local:
                            //gestorLocales.Baja(); // Pendiente de desarrollo
                            break;

                        case TipoEntidad.Cliente:
                            //gestorClientes.Baja(); // Pendiente de desarrollo
                            break;

                        case TipoEntidad.Contrato:
                            //gestorContratos.Baja(); // Pendiente de desarrollo
                            break;

                    }
                };

                // Proceso de edicion
                general.EditarClicked += (s, e) =>
                {
                    AlternarPanelInferior(panelEdicion);

                    // Habilitar los TextBox y poner el foco en el primer campo
                    Utiles.HabilitarTextBoxes(contenedor: this, habilitar: true);

                    //Deshabilita el grid de empresas
                    ucEmpresas.dgvEmpresas.Enabled = false;

                    switch(entidadActiva)
                    {
                        case TipoEntidad.Empresa:
                            //gestorEmpresas.Actualizar(); // Pendiente de desarrollo
                            break;

                        case TipoEntidad.Local:
                            //gestorLocales.Actualizar(); // Pendiente de desarrollo
                            break;

                        case TipoEntidad.Cliente:
                            //gestorClientes.Actualizar(); // Pendiente de desarrollo
                            break;

                        case TipoEntidad.Contrato:
                            //gestorContratos.Actualizar(); // Pendiente de desarrollo
                            break;
                    }
                };

                // Proceso al seleccionar estado
                general.SeleccionActivos += (s, e) =>
                {
                    string seleccionEstado = general.EstadoSeleccionado;
                    bool? estado = null;
                    if(seleccionEstado == "Activos")
                    {
                        estado = true;
                    }
                    else if(seleccionEstado == "Inactivos")
                    {
                        estado = false;
                    }

                    switch(entidadActiva)
                    {
                        case TipoEntidad.Empresa:
                            ucEmpresas?.CargarEmpresas(activas: estado);
                            break;

                        case TipoEntidad.Local:
                            ucLocales?.CargarLocales(activos: estado);
                            break;

                        case TipoEntidad.Cliente:
                            ucClientes?.CargarClientes(activos: estado);
                            break;

                        case TipoEntidad.Contrato:
                            ucContratos?.CargarContratos(activos: estado);
                            //gestorContratos.Actualizar(); // Pendiente de desarrollo
                            break;
                    }
                };

                // Procesos para eliminar
                general.EliminarClicked += (s, e) =>
                {
                    AlternarPanelInferior(panelEdicion);

                    switch(entidadActiva)
                    {
                        case TipoEntidad.Empresa:
                            //gestorEmpresas.Eliminar(); // Pendiente de desarrollo
                            break;

                        case TipoEntidad.Local:
                            //gestorLocales.Eliminar(); // Pendiente de desarrollo
                            break;

                        case TipoEntidad.Cliente:
                            //gestorClientes.Eliminar(); // Pendiente de desarrollo
                            break;

                        case TipoEntidad.Contrato:
                            //gestorContratos.Eliminar(); // Pendiente de desarrollo
                            break;
                    }
                };


            }
            else if(panel is PanelInferior_Edicion edicion)
            {
                // Procesos al cancelar la edicion
                edicion.CancelarClicked += (s, e) =>
                {
                    AlternarPanelInferior(panelGeneral);

                    // Habilitar los TextBox y poner el foco en el primer campo
                    Utiles.HabilitarTextBoxes(contenedor: this, habilitar: false);

                    switch(entidadActiva)
                    {
                        case TipoEntidad.Empresa:
                            // Habilita el grid de empresas
                            ucEmpresas.dgvEmpresas.Enabled = true;
                            break;

                        case TipoEntidad.Local:
                            // Habilita el grid de locales
                            //ucLocales.dgvLocales.Enabled = true; // Pendiente desarrollo
                            break;

                        case TipoEntidad.Cliente:
                            // Habilita el grid de clientes
                            //ucClientes.dgvClientes.Enabled = true; // Pendiente desarrollo
                            break;

                        case TipoEntidad.Contrato:
                            // Habilita el grid de contratos
                            //ucContratos.dgvContratos.Enabled = true; // Pendiente desarrollo
                            break;
                    }
                };

                // Procesos al validar la edicion
                edicion.ValidarClicked += (s, e) =>
                {
                    AlternarPanelInferior(panelGeneral);

                    // Habilitar los TextBox y poner el foco en el primer campo
                    Utiles.HabilitarTextBoxes(contenedor: this, habilitar: false);

                    switch(entidadActiva)
                    {
                        case TipoEntidad.Empresa:
                            // Habilita el grid de empresas
                            ucEmpresas.dgvEmpresas.Enabled = true;

                            //Actualiza la base de datos
                            //gestorEmpresas.Agregar(); // Pendiente de desarrollo
                            break;

                        case TipoEntidad.Local:
                            // Habilita el grid de locales
                            //ucLocales.dgvLocales.Enabled = true; // Pendiente desarrollo

                            //Actualiza la base de datos
                            //gestorLocales.Agregar(); // Pendiente de desarrollo
                            break;

                        case TipoEntidad.Cliente:
                            // Habilita el grid de clientes
                            //ucClientes.dgvClientes.Enabled = true; // Pendiente desarrollo

                            //Actualiza la base de datos
                            //gestorClientes.Agregar(); // Pendiente de desarrollo
                            break;

                        case TipoEntidad.Contrato:
                            // Habilita el grid de contratos
                            //ucContratos.dgvContratos.Enabled = true; // Pendiente desarrollo

                            //Actualiza la base de datos
                            //gestorContratos.Agregar(); // Pendiente de desarrollo
                            break;
                    }



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
            CargarPanelCentral(ucEmpresas, TipoEntidad.Empresa);
        }

        private void btnClientes_Click(object sender, EventArgs e)
        {
            var ucClientes = new UC_Clientes();
            ucClientes.Dock = DockStyle.Fill;
            btnAbrirPanel_Click(btnAbrirPanel, EventArgs.Empty);
            CargarPanelCentral(ucClientes, TipoEntidad.Cliente);
        }

        private void btnContratos_Click(object sender, EventArgs e)
        {
            var ucContratos = new UC_Contratos();
            ucContratos.Dock = DockStyle.Fill;
            btnAbrirPanel_Click(btnAbrirPanel, EventArgs.Empty);
            CargarPanelCentral(ucContratos, TipoEntidad.Contrato);
        }

        private void btnLocales_Click(object sender, EventArgs e)
        {
            var ucLocales = new UC_Locales();
            ucLocales.Dock = DockStyle.Fill;
            btnAbrirPanel_Click(btnAbrirPanel, EventArgs.Empty);
            CargarPanelCentral(ucLocales, TipoEntidad.Local);
        }

        private void btnConfigurar_Click(object sender, EventArgs e)
        {
            var ucConfiguracion = new UC_Configuracion();
            ucConfiguracion.Dock = DockStyle.Fill;
            btnAbrirPanel_Click(btnAbrirPanel, EventArgs.Empty);
            CargarPanelCentral(ucConfiguracion, TipoEntidad.Configurar);
        }

        private void btnInicio_Click(object sender, EventArgs e)
        {
            panelCentral.Controls.Clear();
            panelGeneral.Visible = false;
        }
    }

}
