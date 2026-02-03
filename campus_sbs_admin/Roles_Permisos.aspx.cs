using campus_sbs_admin.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
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
                
            }
        }

        // ===========================
        // ROLES
        // ===========================
        [WebMethod]
        public static List<RolesUsuarioDto> GetRoles()
        {
            try
            {
                return ObtenerRoles();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        [WebMethod]
        public static object SaveRol(int id, string nombre, string descripcion, bool activo = false)
        {
            try
            {
                if (!validarRol(nombre))
                {
                    using (var db = new SpainBS_Connection())
                    {
                        var mapping = new Role
                        {
                            Name = nombre,
                            Description = descripcion,
                            IsActive = activo

                        };
                        db.Roles.Add(mapping);
                        db.SaveChanges();

                    }
                }
                // Fake: simula guardado
                return new { ok = true, message = "Rol guardado correctamente." };
            }
            catch (Exception ex)
            {
                return new { ok = false, message = ex.Message };
            }
        }

        [WebMethod]
        public static object DeleteRol(int Id)
        {
            try
            {
                using (var db = new SpainBS_Connection())
                {
                    var findToDelete = db.Roles.Find(Id);
                    if (findToDelete is null)
                    {
                        return new { ok = false, message = "Rol no se encontró." };
                    }

                    db.Roles.Remove(findToDelete);
                    if (db.SaveChanges() > 0)
                        return new { ok = true, message = "Rol eliminado correctamente." };

                }


                return new { ok = false, message = "no se pudo eliminar el role." };
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
                var lista = new List<object>();
                using (var db = new SpainBS_Connection())
                {
            


                    var listaClientes =(from c in db.CLIENTES
                                        join cr in db.ClienteRoles on c.id_cliente equals cr.id_cliente
                                        join r in db.Roles on cr.RoleId equals r.RoleId
                                        where c.activo == "1" && c.Comercial == true || c.Administrador=="1"
                                        select new
                                        {
                                            id = c.id_cliente,
                                            nombre = c.Nombre_Completo,
                                            email=c.email,
                                            rol = r.Name,
                                            rolId = cr.RoleId
                                        }).AsEnumerable()   
                                          .Cast<object>()
                                        .ToList();

                    lista = listaClientes;

                }


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
              
             
                    using (var db = new SpainBS_Connection())
                    {
                       
                        var actual = db.ClienteRoles.FirstOrDefault(x => x.id_cliente == usuarioId);

                       
                        if (actual != null && actual.RoleId == rolId)
                            return new { ok = true, message = "El usuario ya tiene ese rol." };

                   
                        if (actual != null)
                            db.ClienteRoles.Remove(actual);

                        bool existe = db.ClienteRoles.Any(x => x.id_cliente == usuarioId && x.RoleId == rolId);
                        if (!existe)
                        {
                            db.ClienteRoles.Add(new ClienteRole
                            {
                                id_cliente = usuarioId,
                                RoleId = rolId,
                                CreatedAt = DateTime.Now 
                            });
                        }

                        db.SaveChanges();
                        return new { ok = true, message = "Rol asignado correctamente." };
                    }


                }
            catch (Exception ex)
            {
                var msg = ex.InnerException?.InnerException?.Message
                ?? ex.InnerException?.Message
                ?? ex.Message;

                return new { ok = false, message = msg };
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
                using (var db = new SpainBS_Connection())
                {
                    var permisos = (
                        from p in db.Pages
                        join rp in db.RolePages
                            on new { PageId = p.PageId, RoleId = rolId }
                            equals new { rp.PageId, rp.RoleId }
                        where p.IsActive && p.IsMenu
                        orderby p.SortOrder
                        select new
                        {
                            id = p.PageId,
                            modulo = p.Title,
                            ver = rp.CanView,
                            crear = true,
                            editar = rp.CanEdit,
                            eliminar = rp.CanDelete
                        }
                    )
                    .AsEnumerable()     
                    .Cast<object>()     
                    .ToList();

                    return permisos;
                }
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
                if (permisos == null || permisos.Count == 0)
                    return new { ok = false, message = "No se recibieron permisos." };

                using (var db = new SpainBS_Connection())
                {
                    // Traer permisos actuales del rol (solo los pageId que vienen del cliente)
                    var pageIds = permisos.Select(p => p.id).Distinct().ToList();

                    var existentes = db.RolePages
                        .Where(rp => rp.RoleId == rolId && pageIds.Contains(rp.PageId))
                        .ToList();

                    if (existentes.Count == 0)
                        return new { ok = false, message = "No se encontró registros para ese rol." };

                    // Diccionario para acceso rápido
                    var map = existentes.ToDictionary(x => x.PageId);

                    int updated = 0;
              

                    foreach (var dto in permisos)
                    {
                        if (map.TryGetValue(dto.id, out var entity))
                        {
                            bool changed =
                                entity.CanView != dto.ver ||

                                entity.CanEdit != dto.editar ||
                                entity.CanDelete != dto.eliminar;

                            if (changed)
                            {
                                entity.CanView = dto.ver;

                                entity.CanEdit = dto.editar;
                                entity.CanDelete = dto.eliminar;
                                updated++;
                            }
                        }
                    }
                    if (updated > 0) db.SaveChanges();
                    return new
                    {
                        ok = true,
                        message = "Permisos guardados correctamente para el rol.",
                        updated,
                       
                    };
                }
            }
            catch (Exception ex)
            {
                var msg = ex.InnerException?.InnerException?.Message
                          ?? ex.InnerException?.Message
                          ?? ex.Message;

                return new { ok = false, message = msg };
            }
        }

        private static List<RolesUsuarioDto> ObtenerRoles()
        {
            try
            {
                var lista = new List<RolesUsuarioDto>();
                using (var db = new SpainBS_Connection())
                {
                    var roles = db.Roles.Where(a => a.IsActive == true).OrderByDescending(a => a.RoleId).Select(a => new RolesUsuarioDto
                    {
                        Id = a.RoleId,
                        Nombre = a.Name,
                        Descripcion = a.Description,
                        Activo = a.IsActive,
                        CantidadUsuario = db.ClienteRoles.Where(x => x.RoleId == a.RoleId).ToList().Count(),
                    }).ToList();

                    lista = roles;

                }
                return lista;
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
                return null;
            }
        }
        private static bool validarRol(string nombre)
        {
            try
            {
                using (var db = new SpainBS_Connection())
                {
                    var found = db.Roles.ToList();

                    if (found.Count() > 0)
                    {
                        return found.Any(a => a.Name.ToLower().Trim() == nombre.ToLower().Trim());
                    }
                    return false;
                }



            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
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

        public class RolesUsuarioDto
        {

            public int Id { get; set; }
            public string Nombre { get; set; }
            public string Descripcion { get; set; }
            public bool Activo { get; set; }
            public int CantidadUsuario { get; set; }

            public static implicit operator RolesUsuarioDto(List<RolesUsuarioDto> v)
            {
                throw new NotImplementedException();
            }
        }

    }
}