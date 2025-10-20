using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Facturar.Entidades;
using Facturar.Servicios;
using Utiles = Facturar.Utilidades.UtilesGenerales;


namespace Facturar.Utilidades
{
    public static class Pruebas
    {
        public static void LanzaPruebas(Entidades entidad, Procesos proceso)
        {
            switch(entidad)
            {
                case Entidades.Empresas:
                    ProbarEmpresas(proceso);
                    break;

                case Entidades.Locales:
                    ProbarLocales(proceso);
                    break;

                case Entidades.Clientes:
                    ProbarClientes(proceso);
                    break;

                case Entidades.Contratos:
                    ProbarContratos(proceso);
                    break;

                case Entidades.BaseDatos:
                    break;

            }
        }

        private static void ProbarEmpresas(Procesos proceso)
        {
            //var repo = GestorDatos.AbrirConexion();
            var empresa = new Empresa();
            var gestor = new GestorEmpresas();
            var gestorLocales = new GestorLocales();
            bool resultado;
            switch(proceso)
            {
                case Procesos.Alta:
                    empresa.Nombre = "EMPRESA PRUEBAS DOS";
                    empresa.NIF = "05100001G";
                    empresa.Direccion = "Calle Mayor, 1";
                    empresa.CodigoPostal = "02002";
                    empresa.Poblacion = "Albacete";
                    empresa.Provincia = "Albacete";
                    empresa.Telefono = "666333222";
                    empresa.Email = "correo@correo.com";
                    empresa.PersonaContacto = "Persona contacto";
                    empresa.SerieFactura = "A";

                    resultado = gestor.Agregar(empresa);

                    break;

                case Procesos.Baja:
                    resultado = gestor.Baja("05100001G");
                    break;

                case Procesos.Modificacion:
                    empresa.Nombre = "Empresa de pruebas modificada";
                    empresa.NIF = "05100001G";
                    empresa.Direccion = "Calle Mayor, 1";
                    empresa.CodigoPostal = "02002";
                    empresa.Poblacion = "Albacete";
                    empresa.Provincia = "Albacete";
                    empresa.Telefono = "666333222";
                    empresa.Email = "correo@correo.com";
                    empresa.PersonaContacto = "Persona contacto";
                    empresa.SerieFactura = "A";

                    resultado = gestor.Actualizar(empresa);
                    break;


                case Procesos.Consulta:
                    List<Empresa> ConsultaEmpresa = gestor.ListarTodos(false).ToList();
                    break;

                case Procesos.ConsultaActivos:
                    List<Empresa> ConsultaActivas = gestor.ListarTodos(true).ToList();
                    break;

                case Procesos.Eliminacion:
                    resultado = gestor.Eliminar("05100001G");
                    break;
            }
        }

        private static void ProbarClientes(Procesos proceso)
        {
            //var repo = GestorDatos.AbrirConexion();
            var cliente = new Cliente();
            var gestor = new GestorClientes();
            bool resultado;
            switch(proceso)
            {
                case Procesos.Alta:
                    cliente.Nombre = "PRIMER CLIENTE NOMBRE";
                    cliente.NIF = "05100001G";
                    cliente.Direccion = "Calle Mayor, 1";
                    cliente.CodigoPostal = "02002";
                    cliente.Poblacion = "Albacete";
                    cliente.Provincia = "Albacete";
                    cliente.Telefono = "666333222";
                    cliente.Email = "correo@correo.com";
                    cliente.PersonaContacto = "Persona contacto";
                    cliente.FormaPago = GestorClientes.FormasPago.Transferencia.ToString();
                    cliente.Observaciones = "Observaciones del cliente 2";

                    resultado = gestor.Agregar(cliente);

                    break;

                case Procesos.Baja:
                    resultado = gestor.Baja("05100001G");
                    break;

                case Procesos.Modificacion:
                    cliente.Nombre = "CLIENTE DE PRUEBAS MODIFICADO";
                    cliente.NIF = "05100001G";
                    cliente.Direccion = "Calle Mayor, 1";
                    cliente.CodigoPostal = "02002";
                    cliente.Poblacion = "Albacete";
                    cliente.Provincia = "Albacete";
                    cliente.Telefono = "666333222";
                    cliente.Email = "correo@correo.com";
                    cliente.PersonaContacto = "Persona contacto";
                    cliente.FormaPago = GestorClientes.FormasPago.Transferencia.ToString();
                    cliente.IBAN = "ES6601822032002200231234";
                    cliente.Observaciones = "Cliente de pruebas modificado";

                    resultado = gestor.Actualizar(cliente);
                    break;


                case Procesos.Consulta:
                    List<Cliente> ConsultaCliente = gestor.ListarTodos(false).ToList();
                    break;

                case Procesos.ConsultaActivos:
                    List<Cliente> ConsultaActivas = gestor.ListarTodos(true).ToList();
                    break;

                case Procesos.Eliminacion:
                    resultado = gestor.Eliminar("05100001G");
                    break;
            }
        }


        private static void ProbarLocales(Procesos proceso)
        {
            //var repo = GestorDatos.AbrirConexion();
            var local = new Local();
            var gestor = new GestorLocales();
            bool resultado;
            switch(proceso)
            {
                case Procesos.Alta:
                    local.EmpresaId = 1;
                    local.Descripcion = "Local en poligono campollano";
                    local.Direccion = "POLIGONO CAMPOLLANO C/B, 1";
                    local.CodigoPostal = "02007";
                    local.Poblacion = "ALBAETE";
                    local.Provincia = "ALBAETE";
                    local.ImporteAlquiler = 725.45m;

                    resultado = gestor.Agregar(local);

                    break;

                case Procesos.Baja:
                    resultado = gestor.BajaLocal(2);
                    break;

                case Procesos.Modificacion:
                    var localNuevo = gestor.ObtenerPorId(1);
                    localNuevo.EmpresaId = 2;

                    resultado = gestor.Actualizar(localNuevo);
                    break;


                case Procesos.Consulta:
                    List<Local> ConsultaLocal = gestor.ListarTodos().ToList();
                    break;

                case Procesos.ConsultaActivos:
                    List<Local> ConsultaLocalesActivos = gestor.ListarLocalesPorEmpresa(1, false).ToList();
                    break;

                case Procesos.Eliminacion:
                    resultado = gestor.EliminarLocal(1);
                    break;
            }
        }


        private static void ProbarContratos(Procesos proceso)
        {
            var contrato = new Contrato();
            var gestor = new GestorContratos();
            bool resultado;
            switch(proceso)
            {
                case Procesos.Alta:
                    contrato.EmpresaId = 1;
                    contrato.ClienteId = 4;
                    contrato.LocalId = 2;
                    contrato.PrecioMensual = 755.22m;
                    contrato.FechaInicio = Utiles.ConvertirFecha("15/05/2025").Value;
                    contrato.Observaciones = "Observaciones contrato 1";

                    resultado = gestor.Agregar(contrato);

                    contrato.EmpresaId = 2;
                    contrato.ClienteId = 4;
                    contrato.LocalId = 3;
                    contrato.PrecioMensual = 755.22m;
                    contrato.FechaInicio = Utiles.ConvertirFecha("15/05/2025").Value;
                    contrato.Observaciones = "Observaciones contrato 2";

                    resultado = gestor.Agregar(contrato);

                    break;

                case Procesos.Baja:
                    resultado = gestor.Baja(2);
                    break;

                case Procesos.Modificacion:
                    var contratoNuevo = gestor.ObtenerPorId(1);
                    contratoNuevo.EmpresaId = 2;
                    contratoNuevo.Observaciones = "Observaciones modificadas";

                    resultado = gestor.Actualizar(contratoNuevo);
                    break;


                case Procesos.Consulta:
                    List<Contrato> ConsultaContratos = gestor.ListarTodos().ToList();
                    break;

                case Procesos.ConsultaActivos:
                    // Consulta todos los contratos activos
                    List<Contrato> ConsultaContratosActivos = gestor.ListarTodos(true).ToList();

                    // Consulta contratos de un cliente
                    List<Contrato> ConsultaContratosCliente = gestor.ListarContratosPorCliente(clienteNif:"05100001G").ToList();

                    //Consulta los contratos de una empresa
                    List<Contrato> ConsultaContratosEmpresa = gestor.ListarContratosPorEmpresa(empresaNif:"05196375P").ToList();

                    // Consulta los contratos de un local
                    List<Contrato> ConsultaContratosLocal = gestor.ListarContratosPorLocal(2).ToList();

                    //Consulta contratos por fecha
                    List<Contrato> ConsultaContratosFecha = gestor.ListarContratosPorFecha(Utiles.ConvertirFecha("01/05/2025").Value, Utiles.ConvertirFecha("01/05/2025").Value).ToList();

                    break;

                case Procesos.Eliminacion:
                    resultado = gestor.Eliminar(1);
                    break;
            }
        }

        public enum Entidades
        {
            Empresas = 0,
            Clientes = 1,
            Contratos = 2,
            Locales = 3,
            BaseDatos = 4
        }

        public enum Procesos
        {
            Alta = 0,
            Baja = 1,
            Modificacion = 2,
            Consulta = 3,
            ConsultaActivos = 4,
            Eliminacion = 5
        }
    }
}
