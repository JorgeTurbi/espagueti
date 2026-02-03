using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace campus_sbs_admin
{
    public partial class Roles_Permisos : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Datos fake se cargan por AJAX (WebMethods abajo)
            }
        }

        // ===========================
        // ROLES
        // ===========================
        [WebMethod]
        public static object GetRoles()
        {
            try
            {
                var lista = new List<object>
                {
                    new { id = 1, nombre = "Administrador", descripcion = "Acceso total al sistema", activo = true, usuarios = 3 },
                    new { id = 2, nombre = "Gestor", descripcion = "Gestión de facturas y gastos", activo = true, usuarios = 5 },
                    new { id = 3, nombre = "Consultor", descripcion = "Solo lectura de datos", activo = true, usuarios = 8 },
                    new { id = 4, nombre = "Contable", descripcion = "Acceso a módulos financieros", activo = true, usuarios = 2 },
                    new { id = 5, nombre = "Comercial", descripcion = "Gestión de leads y clientes", activo = false, usuarios = 0 }
                };

                return lista;
            }
            catch (Exception ex)
            {
                return new { ok = false, message = ex.Message };
            }
        }

        [WebMethod]
        public static object SaveRol(int id, string nombre, string descripcion, bool activo)
        {
            try
            {
                // Fake: simula guardado
                return new { ok = true, message = "Rol guardado correctamente.", id = (id > 0 ? id : 6) };
            }
            catch (Exception ex)
            {
                return new { ok = false, message = ex.Message };
            }
        }

        [WebMethod]
        public static object DeleteRol(int id)
        {
            try
            {
                return new { ok = true, message = "Rol eliminado correctamente." };
            }
            catch (Exception ex)
            {
                return new { ok = false, message = ex.Message };
            }
        }

        // ===========================
        // ASIGNACIÓN
        // ===========================
        [WebMethod]
        public static object GetUsuarios()
        {
            try
            {
                var lista = new List<object>
                {
                    new { id = 1, nombre = "Carlos Méndez", email = "carlos@sbs.es", rol = "Administrador", rolId = 1 },
                    new { id = 2, nombre = "Ana García", email = "ana@sbs.es", rol = "Gestor", rolId = 2 },
                    new { id = 3, nombre = "Pedro López", email = "pedro@sbs.es", rol = "Consultor", rolId = 3 },
                    new { id = 4, nombre = "María Torres", email = "maria@sbs.es", rol = "Contable", rolId = 4 },
                    new { id = 5, nombre = "Luis Ramírez", email = "luis@sbs.es", rol = "Gestor", rolId = 2 },
                    new { id = 6, nombre = "Elena Vega", email = "elena@sbs.es", rol = "Consultor", rolId = 3 },
                    new { id = 7, nombre = "Jorge Díaz", email = "jorge@sbs.es", rol = "Sin asignar", rolId = 0 }
                };

                return lista;
            }
            catch (Exception ex)
            {
                return new { ok = false, message = ex.Message };
            }
        }

        [WebMethod]
        public static object AsignarRol(int usuarioId, int rolId)
        {
            try
            {
                return new { ok = true, message = "Rol asignado correctamente." };
            }
            catch (Exception ex)
            {
                return new { ok = false, message = ex.Message };
            }
        }

        // ===========================
        // PERMISOS
        // ===========================
        [WebMethod]
        public static object GetPermisos(int rolId)
        {
            try
            {
                var modulos = new List<object>
                {
                    new { id = 1, modulo = "Facturas", ver = true, crear = true, editar = true, eliminar = (rolId == 1) },
                    new { id = 2, modulo = "Gastos", ver = true, crear = true, editar = true, eliminar = (rolId == 1) },
                    new { id = 3, modulo = "Leads / CRM", ver = true, crear = (rolId <= 2), editar = (rolId <= 2), eliminar = (rolId == 1) },
                    new { id = 4, modulo = "Usuarios", ver = (rolId <= 2), crear = (rolId == 1), editar = (rolId == 1), eliminar = (rolId == 1) },
                    new { id = 5, modulo = "Informes", ver = true, crear = (rolId <= 3), editar = false, eliminar = false },
                    new { id = 6, modulo = "Configuración", ver = (rolId == 1), crear = (rolId == 1), editar = (rolId == 1), eliminar = (rolId == 1) },
                    new { id = 7, modulo = "Ventas TPV", ver = true, crear = (rolId <= 2), editar = (rolId <= 2), eliminar = (rolId == 1) },
                    new { id = 8, modulo = "Documentos", ver = true, crear = true, editar = true, eliminar = (rolId <= 2) }
                };

                return modulos;
            }
            catch (Exception ex)
            {
                return new { ok = false, message = ex.Message };
            }
        }

        [WebMethod]
        public static object SavePermisos(int rolId, List<PermisoDTO> permisos)
        {
            try
            {
                return new { ok = true, message = "Permisos guardados correctamente para el rol." };
            }
            catch (Exception ex)
            {
                return new { ok = false, message = ex.Message };
            }
        }

        // DTO para recibir permisos
        public class PermisoDTO
        {
            public int id { get; set; }
            public bool ver { get; set; }
            public bool crear { get; set; }
            public bool editar { get; set; }
            public bool eliminar { get; set; }
        }
    }
}