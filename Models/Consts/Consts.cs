using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ferpuser.Models.Consts
{
    public static class Consts
    {
        public const string CODIGO_EMPRESA = "01";
        public const string USUARIO_WEB_NET = "WEB_NET";
        public const string LOCAL_RELATIVE_PATH_UPLOADS = "uploads"; //Ruta donde se guardan las imagenes en el servidor local
        public const string CODIGO_TIPO_CENTRO_COSTE_E9 = "Z";        

        public const string PRESERVE_PERSONAL_URL = "PRESERVE_PERSONAL_URL";
        public const string PRESERVE_COMPRAS_FACTURAS_URL = "PRESERVE_PERSONAL_URL";
        public const string PRESERVE_PARAMETRO_URL = "PRESERVE_PARAMETRO_URL";
        public const string PRESERVE_PARTE_PERSONAL_URL = "PRESERVE_PARTE_PERSONAL_URL";

        public const string SESSION_EJERCICIO = "SESSION_EJERCICIO";
        public const string SESSION_ACOMMODATION_LIST_STATE = "SESSION_ACOMMODATION_LIST_STATE";
        public const string SESSION_REGISTRATION_LIST_STATE = "SESSION_REGISTRATION_LIST_STATE";
        

        //El siguiente se debería quitar en algún momento
        public const int NUMBER_EVENTO_HARDCODEADO = 22022;

        //Series para numeración de documentos de ventas
        public const int SERIES_CODIGO_TIPO_DOCUMENTO_VENTA_PEDIDO = 1;
        public const int SERIES_CODIGO_TIPO_DOCUMENTO_VENTA_ALBARAN = 2;
        public const int SERIES_CODIGO_TIPO_DOCUMENTO_VENTA_FACTURA = 7;

        //Claims
        public const string CLAIM_SMTP_SERVER = "SMPT_SERVER";
        public const string CLAIM_SMTP_PORT = "SMPT_PORT";
        public const string CLAIM_MAIL_USER = "MAIL_USER";
        public const string CLAIM_MAIL_PASSWORD = "MAIL_PASSWORD";
        public const string CLAIM_SEND_COPY = "SEND_COPY";
        public const string CLAIM_CODIGO_OPERARIO = "CLAIM_CODIGO_OPERARIO";
        public const string CLAIM_CODIGO_VENDEDOR = "CLAIM_CODIGO_VENDEDOR";
        public const string CLAIM_ACCOUNT_ID = "CLAIM_ACCOUNT_ID";

        //Claims Permisos
        public const string CLAIM_PERMISO_ADMINISTRACION = "PermisoAdministracion";
        public const string CLAIM_PERMISO_EVENTOS = "PermisoEventos";
        public const string CLAIM_PERMISO_FACTURACION = "PermisoFacturacion";
        public const string CLAIM_PERMISO_VENTAS = "PermisoVentas";
        public const string CLAIM_PERMISO_COMPRAS = "PermisoCompras";
        public const string CLAIM_PERMISO_CONTROL_PRESUPUESTARIO = "PermisoControlPresupuestario";
        public const string CLAIM_PERMISO_CONTROL_ALMACEN = "PermisoControlAlmacen";
        public const string CLAIM_PERFIL_USUARIO_EMPLEADO = "PerfilUsuarioEmpleado";
        public const string CLAIM_PERFIL_USUARIO_CLIENTE = "PerfilUsuarioCliente";

        //Parámetros
        public const string PARAMETRO_CODIGO_FECHA_LIMITE_INFERIOR_FACTURAS_COMPRA_VENTA = "FECHA_LIMITE_INFERIOR_FACTURAS_COMPRA_VENTA";

        public const string PARAMETRO_CODIGO_EMPRESA_NOMBRE = "EMPRESA_NOMBRE";
        public const string PARAMETRO_CODIGO_EMPRESA_DIRECCION = "EMPRESA_DIRECCION";
        public const string PARAMETRO_CODIGO_EMPRESA_CP = "EMPRESA_CP";
        public const string PARAMETRO_CODIGO_EMPRESA_POBLACION = "EMPRESA_POBLACION";
        public const string PARAMETRO_CODIGO_EMPRESA_PROVINCIA = "EMPRESA_PROVINCIA";
        public const string PARAMETRO_CODIGO_EMPRESA_NIF_CIF = "EMPRESA_NIF_CIF";
        public const string PARAMETRO_CODIGO_EMPRESA_LOGO = "EMPRESA_LOGO";
        public const string PARAMETRO_CODIGO_EMPRESA_FIRMA = "EMPRESA_FIRMA";
        public const string PARAMETRO_CODIGO_EMPRESA_NOMBRE_REPRESENTANTE = "EMPRESA_REPRESENTANTE";
        public const string PARAMETRO_CODIGO_EMPRESA_NIF_CIF_REPRESENTANTE = "EMPRESA_NIF_CIF_REPRESENTANTE";


        //Tipos de evento
        public const string CODIGO_TIPO_EVENTO_CONGRESO = "C";
        public const string CODIGO_TIPO_EVENTO_OBRA = "O";
        public const string CODIGO_TIPO_EVENTO_CENTRO_COSTE = "Z";

    }
}
