using System;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Windows.Forms;
using Facturar.Presentacion.Controles;
using Facturar.Presentacion.Paneles;
using Facturar.Servicios;
using Enumerador = Facturar.Utilidades.Enumeradores;
using Proceso = Facturar.Presentacion.Procesos;
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

        // Propiedad publica para gestionar si se produce un error en la gestion de procesos al validar y grabar en el base de datos
        public bool errorProceso { get; set; }

        // Variables de clase para gestionar paneles y entidades
        private PanelInferior_general panelGeneral;
        private PanelInferior_Edicion panelEdicion;
        private PanelInferiorFacturas panelFacturas;
        private GestorEmpresas gestorEmpresas;
        private GestorLocales gestorLocales;
        private GestorClientes gestorClientes;
        private GestorContratos gestorContratos;
        private GestorConfiguracion gestorConfiguracion;
        private GestorFacturas gestorFacturas;

        // Propiedades publicas de los gestores para acceso desde fuera de la clase
        public GestorEmpresas GestorEmpresas => gestorEmpresas;
        public GestorLocales GestorLocales => gestorLocales;
        public GestorClientes GestorClientes => gestorClientes;
        public GestorContratos GestorContratos => gestorContratos;


        // Instancias de UserControl 
        private UC_Empresas ucEmpresas = new UC_Empresas();
        private UC_Locales ucLocales = new UC_Locales();
        private UC_Clientes ucClientes = new UC_Clientes();
        private UC_Contratos ucContratos = new UC_Contratos();
        private UC_Configuracion ucConfiguracion = new UC_Configuracion();
        private UC_Facturas ucFacturas = new UC_Facturas();


        // Control de entidad cargada en el panel central
        private UserControl panelCentralActivo; // Permite despues acceder acceder al panel para habilitar controles o refrescar el grid
        private Enumerador.TipoEntidad entidadActiva = Enumerador.TipoEntidad.Ninguno; // Al inicio no se ha cargado ninguna
        private object gestorActual; // Almacena el gestor que debe gestionarse en el formulario (se cambia al acceder a las opciones de cada tipo de entidad)
        private Enumerador.TipoProceso tipoProceso = Enumerador.TipoProceso.Ninguno; // Controla el tipo de proceso que se está realizando (alta, baja, edicion, eliminacion) 

        public Enumerador.TipoProceso TipoProceso => tipoProceso;

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
            panelFacturas = new PanelInferiorFacturas();
            gestorEmpresas = new GestorEmpresas(); // Instancia para acceder a los metodos de empresas

            // Suscribir a los eventos de los userControl
            SuscribirEventosPanelInferior(panelGeneral);
            SuscribirEventosPanelInferior(panelEdicion);
            SuscribirEventosPanelInferior(panelFacturas);

            // Carga los dos paneles
            CargarPanelInferior(panelGeneral, dock: DockStyle.Right);
            CargarPanelInferior(panelEdicion, dock: DockStyle.Left);
            CargarPanelInferior(panelFacturas, dock: DockStyle.Right);

            panelGeneral.Visible = false;
            panelEdicion.Visible = false;
            panelFacturas.Visible = false;
        }

        private void CargarPanelCentral(UserControl panel, Enumerador.TipoEntidad tipo)
        {
            // Limpia el contenido del panel central
            panelCentral.Controls.Clear();
            panelCentral.Controls.Add(panel);

            // Control de tipo de entidad cargada en el panel
            panelCentralActivo = panel;
            entidadActiva = tipo;

            switch(entidadActiva)
            {
                case Enumerador.TipoEntidad.Empresa:
                    gestorActual = gestorEmpresas;
                    break;

                case Enumerador.TipoEntidad.Local:
                    gestorActual = gestorLocales;
                    break;

                case Enumerador.TipoEntidad.Cliente:
                    gestorActual = gestorClientes;
                    break;

                case Enumerador.TipoEntidad.Contrato:
                    gestorActual = gestorContratos;
                    break;

                case Enumerador.TipoEntidad.Configurar:
                    gestorActual = gestorConfiguracion;
                    break;

                case Enumerador.TipoEntidad.Factura:
                    gestorActual = gestorFacturas;
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
                // Boton Alta
                general.AltaClicked += (s, e) =>
                {
                    // Habilita el panel de edicion
                    AlternarPanelInferior(panelEdicion);

                    // Habilitar los TextBox y poner el foco en el primer campo
                    Utiles.HabilitarTextBoxes(contenedor: this, habilitar: true);

                    // Limpiar los TextBoxes para poder introducir datos del alta
                    Utiles.LimpiarTextBoxes(contenedor: this);

                    tipoProceso = Enumerador.TipoProceso.Alta;

                    // Crea una instancia del boton para pasar las instancias de las entidades y ejecutar el proceso correspondiente
                    var botonAlta = new Proceso.BotonAlta(ucEmpresas, ucLocales, ucClientes, ucContratos, ucFacturas);
                    botonAlta.Ejecutar(entidadActiva);

                    // Deja el estado de los registros como activo
                    panelGeneral.MostrarActivos(visible: true);
                };

                // Boton Baja
                general.BajaClicked += (s, e) =>
                {
                    // Selecciona el tipo de proceso
                    tipoProceso = Enumerador.TipoProceso.Baja;

                    // Crea una instancia del boton para pasar las instancias de las entidades y ejecutar el proceso correspondiente
                    var botonBaja = new Proceso.BotonBaja(ucEmpresas, ucLocales, ucClientes, ucContratos, ucFacturas, this);
                    botonBaja.Ejecutar(entidadActiva: entidadActiva);

                    // Deja el proceso libre para siguientes procesos
                    tipoProceso = Enumerador.TipoProceso.Ninguno;

                    // Deja el estado de los registros como activo
                    panelGeneral.MostrarActivos(visible: true);
                };

                // Boton edicion
                general.EditarClicked += (s, e) =>
                {
                    // Habilita el panel de edicion
                    AlternarPanelInferior(panelEdicion);

                    // Habilitar los TextBox y poner el foco en el primer campo
                    Utiles.HabilitarTextBoxes(contenedor: this, habilitar: true);

                    // Selecciona el tipo de proceso
                    tipoProceso = Enumerador.TipoProceso.Edicion;

                    // Crea una instancia del boton para pasar las instancias de las entidades y ejecutar el proceso correspondiente
                    var botonEditar = new Proceso.BotonEditar(ucEmpresas, ucLocales, ucClientes, ucContratos, ucFacturas);
                    botonEditar.Ejecutar(entidadActiva);
                };

                // Proceso al seleccionar estado
                general.SeleccionActivos += (s, e) =>
                {
                    // Carga el estado que tiene el ComboBox de estados
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

                    // Crea una instancia del boton para pasar las instancias de las entidades y ejecutar el proceso correspondiente
                    var botonSeleccionActivos = new Proceso.SeleccionActivos(ucEmpresas, ucLocales, ucClientes, ucContratos, ucFacturas);
                    botonSeleccionActivos.Ejecutar(entidadActiva, estado);
                };

                // Procesos para eliminar
                general.EliminarClicked += (s, e) =>
                {
                    tipoProceso = Enumerador.TipoProceso.Eliminacion;

                    // Crea una instancia del boton para pasar las instancias de las entidades y ejecutar el proceso correspondiente
                    var botonEliminar = new Proceso.BotonEliminar(ucEmpresas, ucLocales, ucClientes, ucContratos, ucFacturas, this);
                    botonEliminar.Ejecutar(entidadActiva: entidadActiva);

                    // Deja el proceso libre para siguientes procesos
                    tipoProceso = Enumerador.TipoProceso.Ninguno;

                    // Deja el estado de los registros como activo
                    panelGeneral.MostrarActivos(visible: true);
                };
            }
            else if(panel is PanelInferior_Edicion edicion)
            {
                // Procesos al cancelar la edicion
                edicion.CancelarClicked += (s, e) =>
                {
                    // Crea una instancia del boton para pasar las instancias de las entidades y ejecutar el proceso correspondiente
                    var botonCancelar = new Proceso.BotonCancelar(ucEmpresas, ucLocales, ucClientes, ucContratos, ucFacturas);
                    botonCancelar.Ejecutar(entidadActiva);

                    // Muestra el panel de botones estandard
                    AlternarPanelInferior(panelGeneral);

                    // Deshabilitar los TextBox
                    Utiles.HabilitarTextBoxes(contenedor: this, habilitar: false);
                };

                // Procesos al validar la edicion
                edicion.ValidarClicked += (s, e) =>
                {
                    // Crea una instancia del boton para pasar las instancias de las entidades y ejecutar el proceso correspondiente
                    var botonValidar = new Proceso.BotonValidar(ucEmpresas, ucLocales, ucClientes, ucContratos, ucFacturas, formulario: this);

                    // Ejecuta las acciones establecidas en el boton
                    botonValidar.Ejecutar(entidadActiva);

                    // Controla si se ha producido algun error en el try-catch interno
                    if(!errorProceso)
                    {
                        // Muestra el panel de botones estandard
                        AlternarPanelInferior(panelGeneral);

                        // Deshabilitar los TextBox
                        Utiles.HabilitarTextBoxes(contenedor: this, habilitar: false);

                        // Inicializa el tipo de proceso para siguientes acciones.
                        tipoProceso = Enumerador.TipoProceso.Ninguno;
                    }
                };

            }

            else if(panel is PanelInferiorFacturas facturas)
            {
                // Boton Alta
                facturas.AltaClicked += (s, e) =>
                {
                    // Habilita el panel de edicion
                    AlternarPanelInferior(panelEdicion);

                    // Habilitar los TextBox y poner el foco en el primer campo
                    Utiles.HabilitarTextBoxes(contenedor: this, habilitar: true);

                    // Limpiar los TextBoxes para poder introducir datos del alta
                    Utiles.LimpiarTextBoxes(contenedor: this);

                    tipoProceso = Enumerador.TipoProceso.Alta;

                    // Crea una instancia del boton para pasar las instancias de las entidades y ejecutar el proceso correspondiente
                    var botonAlta = new Proceso.BotonAlta(ucEmpresas, ucLocales, ucClientes, ucContratos, ucFacturas);
                    botonAlta.Ejecutar(entidadActiva);

                    //// Deja el estado de los registros como activo
                    //panelGeneral.MostrarActivos(visible: true);
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
                btnFacturas.Visible = true;
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
                    btnFacturas.Visible = false;
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
            btnFacturas.Location = new Point(posicionX - (btnFacturas.Width / 2), btnFacturas.Location.Y);
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
            //panelGeneral.Visible = false; // Se oculta para evitar suponerlo a otro que pueda haberse abierto
            ucEmpresas.Dock = DockStyle.Fill;
            btnAbrirPanel_Click(btnAbrirPanel, EventArgs.Empty);

            // Evita abrir varias veces el panelGeneral
            if(panelGeneral.Visible == false)
            {
                panelGeneral.Visible = true;
            }
            panelGeneral.MostrarActivos(visible: true);
            CargarPanelCentral(ucEmpresas, Enumerador.TipoEntidad.Empresa);
        }

        private void btnClientes_Click(object sender, EventArgs e)
        {
            ucClientes.Dock = DockStyle.Fill;
            btnAbrirPanel_Click(btnAbrirPanel, EventArgs.Empty);
            
            // Evita abrir varias veces el panelGeneral
            if(panelGeneral.Visible == false)
            {
                panelGeneral.Visible = true;
            }
            panelGeneral.MostrarActivos(visible: true);
            CargarPanelCentral(ucClientes, Enumerador.TipoEntidad.Cliente);
        }

        private void btnContratos_Click(object sender, EventArgs e)
        {
            ucContratos.Dock = DockStyle.Fill;
            btnAbrirPanel_Click(btnAbrirPanel, EventArgs.Empty);

            // Evita abrir varias veces el panelGeneral
            if(panelGeneral.Visible == false)
            {
                panelGeneral.Visible = true;
            }
            panelGeneral.MostrarActivos(visible: true);
            CargarPanelCentral(ucContratos, Enumerador.TipoEntidad.Contrato);
        }

        private void btnLocales_Click(object sender, EventArgs e)
        {
            ucLocales.Dock = DockStyle.Fill;
            btnAbrirPanel_Click(btnAbrirPanel, EventArgs.Empty);

            // Evita abrir varias veces el panelGeneral
            if(panelGeneral.Visible == false)
            {
                panelGeneral.Visible = true;
            }
            panelGeneral.MostrarActivos(visible: true);
            CargarPanelCentral(ucLocales, Enumerador.TipoEntidad.Local);
        }

        private void btnFacturas_Click(object sender, EventArgs e)
        {
            //panelGeneral.Visible = false; // Se oculta para evitar suponerlo a otro que pueda haberse abierto
            ucFacturas.Dock = DockStyle.Fill;
            btnAbrirPanel_Click(btnAbrirPanel, EventArgs.Empty);
            if(panelGeneral.Visible == false)
            {
                panelGeneral.Visible = true;
            }
            CargarPanelCentral(ucFacturas, Enumerador.TipoEntidad.Factura);

        }

        private void btnConfigurar_Click(object sender, EventArgs e)
        {
            panelGeneral.Visible = false; // Se oculta para evitar suponerlo a otro que pueda haberse abierto
            var ucConfiguracion = new UC_Configuracion();
            ucConfiguracion.Dock = DockStyle.Fill;
            btnAbrirPanel_Click(btnAbrirPanel, EventArgs.Empty);
            panelGeneral.Visible = true;
            CargarPanelCentral(ucConfiguracion, Enumerador.TipoEntidad.Configurar);
        }


        private void btnInicio_Click(object sender, EventArgs e)
        {
            panelCentral.Controls.Clear();
            panelGeneral.Visible = false;
        }


    }

}
