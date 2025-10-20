using System;

namespace Facturar.Infraestructura
{
    // Excepciones personalizadas para lanzar los errores y poder mostrar en los formularios los avisos con sus propias caracteristicas e incluso generar un registro en el log.
    public class ExcepcionBaseAplicacion : Exception
    {
        // Excepcion base de todas las personalizadas
        public ExcepcionBaseAplicacion(string mensaje) : base(mensaje) { }
        public ExcepcionBaseAplicacion(string mensaje, Exception inner) : base(mensaje, inner) { }
    }

    // Excepciones de la capa de datos (SQLite, consultas, etc)
    public class ExcepcionAccesoDatos : ExcepcionBaseAplicacion
    {
        public ExcepcionAccesoDatos(string mensaje) : base(mensaje) { }
        public ExcepcionAccesoDatos(string mensaje, Exception inner) : base(mensaje, inner) { }
    }

    // Excepciones de reglas de negocio o validaciones
    public class ExcepcionNegocio : ExcepcionBaseAplicacion
    {
        public ExcepcionNegocio(string mensaje) : base(mensaje) { }
        public ExcepcionNegocio(string mensaje, Exception inner) : base(mensaje, inner) { }
    }

    // Excepciones especificas para validacion de entidades
    public class ExcepcionValidacionEntidad : ExcepcionNegocio
    {
        public ExcepcionValidacionEntidad(string mensaje) : base(mensaje) { }
        public ExcepcionValidacionEntidad(string mensaje, Exception inner) : base(mensaje, inner) { }
    }

    public class ExcepcionGeneral : ExcepcionBaseAplicacion
    {
        public ExcepcionGeneral(string mensaje) : base(mensaje) { }
        public ExcepcionGeneral(string mensaje, Exception inner) : base(mensaje, inner) { }

    }
}
