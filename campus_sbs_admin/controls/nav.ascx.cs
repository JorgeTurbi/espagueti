using campus_sbs_admin.Models;
using sbs_DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;

namespace campus_sbs_admin.controls
{
    public partial class nav : UserControl
    {
        private readonly DataAccess da = new DataAccess();

        protected void Page_Load(object sender, EventArgs e)
        {
            var auth = Session["auth"] as AuthContext;
            if (auth == null || auth.User == null)
            {
                Response.Redirect("login.aspx");
                return;
            }

            if (!IsPostBack)
            {
                var menu = GetMenuForUser(auth.User.id_cliente);
                paint_menu(menu, Request.Path);
            }
        }

        private void paint_menu(List<MenuGroupDto> menu, string currentPage)
        {
            menu = menu ?? new List<MenuGroupDto>();
            currentPage = NormalizePage(currentPage);

            var sb = new StringBuilder();

            foreach (var g in menu.OrderBy(x => x.SortOrder))
            {
                var groupCode = HttpUtility.HtmlAttributeEncode(g.Code); // MENU_COMPANY para id único

                var groupIcon = (g.IconCss ?? "").Trim();
                if (!string.IsNullOrWhiteSpace(groupIcon) && !groupIcon.Contains("fa-2x"))
                    groupIcon += " fa-2x";

                sb.Append("<li class='nav-item'>");
                sb.Append($"<a href='javascript:void(0)' aria-controls='menu_{groupCode}' aria-expanded='false'>");

                if (!string.IsNullOrWhiteSpace(groupIcon))
                    sb.Append($"<i class='{HttpUtility.HtmlAttributeEncode(groupIcon)}' aria-hidden='true'></i>");

                sb.Append($"<span> {HttpUtility.HtmlEncode(g.Title)}</span>");
                sb.Append("<i class='fas fa-angle-down fa-2x'></i>");
                sb.Append("</a>");

                sb.Append($"<div class='nav-level'><ul id='menu_{groupCode}'>");

                foreach (var p in g.Children)
                {
                    if (string.IsNullOrWhiteSpace(p.Url)) continue;

                    var url = (p.Url ?? "").Trim();
                    var urlCompare = NormalizePage(url);

                    var isActive = string.Equals(currentPage, urlCompare, StringComparison.OrdinalIgnoreCase);
                    var activeClass = isActive ? " class='active'" : "";

                    var pageIcon = (p.IconCss ?? "").Trim();
                    if (!string.IsNullOrWhiteSpace(pageIcon) && !pageIcon.Contains("fa-2x"))
                        pageIcon += " fa-2x";

                    sb.Append($"<li><a href='{HttpUtility.HtmlAttributeEncode(url)}'{activeClass}>");

                    if (!string.IsNullOrWhiteSpace(pageIcon))
                        sb.Append($"<i class='{HttpUtility.HtmlAttributeEncode(pageIcon)}' aria-hidden='true'></i>");

                    sb.Append($"<span> {HttpUtility.HtmlEncode(p.Title)}</span>");
                    sb.Append("</a></li>");
                }

                sb.Append("</ul></div></li>");
            }

            menu_nav.InnerHtml = sb.ToString();
        }

        private static string NormalizePage(string path)
        {
            if (string.IsNullOrWhiteSpace(path)) return string.Empty;

            path = path.Trim();

            if (path.StartsWith("/")) path = path.TrimStart('/');

            var q = path.IndexOf("?", StringComparison.Ordinal);
            if (q >= 0) path = path.Substring(0, q);

            return path;
        }

        //private void paint_menu(List<MenuPageDto> pages, string currentPage)
        //{
        //    pages = pages ?? new List<MenuPageDto>();
        //    currentPage = (currentPage ?? string.Empty).Trim();

        //    // Normaliza currentPage a "algo.aspx"
        //    // (si te llega con / o con querystring)
        //    if (currentPage.StartsWith("/")) currentPage = currentPage.TrimStart('/');
        //    var qIndex = currentPage.IndexOf("?", StringComparison.Ordinal);
        //    if (qIndex >= 0) currentPage = currentPage.Substring(0, qIndex);

        //    var groups = pages
        //        .Where(p => p != null && !string.IsNullOrWhiteSpace(p.GroupCode))
        //        .GroupBy(p => p.GroupCode.Trim())
        //        .Select(g =>
        //        {
        //            // Tomamos "metadata" del grupo desde el primer item
        //            var first = g.First();

        //            return new
        //            {
        //                GroupCode = g.Key,
        //                GroupTitle = string.IsNullOrWhiteSpace(first.GroupTitle) ? g.Key : first.GroupTitle,
        //                GroupIconCss = (first.GroupIconCss ?? "").Trim(),
        //                GroupSortOrder = first.GroupSortOrder,
        //                Pages = g.Where(x => x != null)
        //                         .OrderBy(x => x.SortOrder)
        //                         .ThenBy(x => x.Title)
        //                         .ToList()
        //            };
        //        })
        //        .OrderBy(g => g.GroupSortOrder)
        //        .ThenBy(g => g.GroupTitle);

        //    var sb = new StringBuilder();

        //    foreach (var g in groups)
        //    {
        //        // Icono del grupo desde BD (OBLIGATORIO)
        //        // Si no viene, puedes poner uno "neutro" (pero tú dijiste que DEBE venir de BD).
        //        // Aun así, para no romper el HTML, si viene vacío lo dejamos sin <i>.
        //        string groupIconCss = g.GroupIconCss;
        //        bool hasGroupIcon = !string.IsNullOrWhiteSpace(groupIconCss);

        //        // Garantiza tamaño (si en BD guardas "fas fa-x", le añade fa-2x)
        //        if (hasGroupIcon && !groupIconCss.Contains("fa-2x"))
        //            groupIconCss += " fa-2x";

        //        sb.Append($"<li class='nav-item'>");
        //        sb.Append($"<a href='javascript:void(0)' aria-controls='menu_{HttpUtility.HtmlAttributeEncode(g.GroupCode)}' aria-expanded='false'>");

        //        if (hasGroupIcon)
        //            sb.Append($"<i class='{HttpUtility.HtmlAttributeEncode(groupIconCss)}' aria-hidden='true'></i>");

        //        sb.Append($"<span> {HttpUtility.HtmlEncode(g.GroupTitle)}</span>");
        //        sb.Append($"<i class='fas fa-angle-down fa-2x'></i>");
        //        sb.Append("</a>");

        //        sb.Append($"<div class='nav-level'><ul id='menu_{HttpUtility.HtmlAttributeEncode(g.GroupCode)}'>");

        //        foreach (var p in g.Pages)
        //        {
        //            if (string.IsNullOrWhiteSpace(p.Url)) continue;

        //            // Normaliza URL de la página (por si viene /empresas.aspx o con query)
        //            var url = p.Url.Trim();
        //            if (url.StartsWith("/")) url = url.TrimStart('/');
        //            var q2 = url.IndexOf("?", StringComparison.Ordinal);
        //            var urlToCompare = q2 >= 0 ? url.Substring(0, q2) : url;

        //            var isActive = string.Equals(currentPage, urlToCompare, StringComparison.OrdinalIgnoreCase);
        //            var activeClass = isActive ? " class='active'" : "";

        //            var pageIconCss = (p.IconCss ?? "").Trim();
        //            if (!string.IsNullOrWhiteSpace(pageIconCss) && !pageIconCss.Contains("fa-2x"))
        //                pageIconCss += " fa-2x";

        //            sb.Append($"<li><a href='{HttpUtility.HtmlAttributeEncode(url)}'{activeClass}>");

        //            if (!string.IsNullOrWhiteSpace(pageIconCss))
        //                sb.Append($"<i class='{HttpUtility.HtmlAttributeEncode(pageIconCss)}' aria-hidden='true'></i>");

        //            sb.Append($"<span> {HttpUtility.HtmlEncode(p.Title)}</span>");
        //            sb.Append("</a></li>");
        //        }

        //        sb.Append("</ul></div></li>");
        //    }

        //    menu_nav.InnerHtml = sb.ToString();
        //}

        //private void paint_menu(List<MenuPageDto> pages, string currentPage)
        //{
        //    if (pages == null) pages = new List<MenuPageDto>();

        //    var groups = pages
        //        .Where(p => p != null && !string.IsNullOrWhiteSpace(p.GroupCode))
        //        .GroupBy(p => p.GroupCode)
        //        .OrderBy(g => g.Key);

        //    var sb = new StringBuilder();

        //    foreach (var g in groups)
        //    {
        //        sb.Append($"<li class='nav-item'><a href='javascript:void(0)' aria-controls='menu_{g.Key}' aria-expanded='false'>");
        //        sb.Append($"<i class='fas fa-folder fa-2x' aria-hidden='true'></i>");
        //        sb.Append($"<span> {g.Key}</span><i class='fas fa-angle-down fa-2x'></i></a>");
        //        sb.Append($"<div class='nav-level'><ul id='menu_{g.Key}'>");

        //        foreach (var p in g.OrderBy(x => x.SortOrder))
        //        {
        //            var isActive = string.Equals(currentPage, p.Url, StringComparison.OrdinalIgnoreCase);
        //            var activeClass = isActive ? " class='active'" : "";

        //            sb.Append($"<li><a href='{p.Url}'{activeClass}>");
        //            sb.Append($"<i class='{p.IconCss}' aria-hidden='true'></i><span> {p.Title}</span>");
        //            sb.Append("</a></li>");
        //        }

        //        sb.Append("</ul></div></li>");
        //    }

        //    menu_nav.InnerHtml = sb.ToString();
        //}
        private List<MenuGroupDto> GetMenuForUser(long idCliente)
        {
            using (var db = new SpainBS_Connection())
            {
                var roleIds = db.ClienteRoles
                    .Where(x => x.id_cliente == idCliente)
                    .Select(x => x.RoleId)
                    .Distinct()
                    .ToList();

                if (roleIds.Count == 0)
                    return new List<MenuGroupDto>();

                // 1) Padres ROOT (menús)
                var roots = db.Pages
                    .Where(p => p.IsActive && p.IsMenu && p.GroupCode == "ROOT")
                    .Select(p => new MenuGroupDto
                    {
                        PageId = p.PageId,
                        Code = p.Code,          // MENU_COMPANY
                        Title = p.Title,        // Company
                        IconCss = p.IconCss,
                        SortOrder = p.SortOrder
                    })
                    .OrderBy(p => p.SortOrder)
                    .ToList();

                // 2) Hijos permitidos por roles (CanView = true)
                var rawChildren = (from rp in db.RolePages
                                   join p in db.Pages on rp.PageId equals p.PageId
                                   where roleIds.Contains(rp.RoleId)
                                         && rp.CanView
                                         && p.IsActive
                                         && p.IsMenu
                                         && p.GroupCode != "ROOT" // hijos
                                   select new
                                   {
                                       p.PageId,
                                       p.Code,
                                       p.Title,
                                       p.Url,
                                       p.IconCss,
                                       p.GroupCode,
                                       p.SortOrder,
                                       rp.CanView,
                                       rp.CanEdit,
                                       rp.CanDelete
                                   })
                                   .ToList();

                // dedupe por PageId (si varios roles lo traen)
                var children = rawChildren
                    .GroupBy(x => x.PageId)
                    .Select(g => new MenuPageDto
                    {
                        PageId = g.Key,
                        Code = g.First().Code,
                        Title = g.First().Title,
                        Url = g.First().Url,
                        IconCss = g.First().IconCss,
                        GroupCode = g.First().GroupCode,
                        SortOrder = g.Min(x => x.SortOrder),
                        CanView = true,
                        CanEdit = g.Any(x => x.CanEdit),
                        CanDelete = g.Any(x => x.CanDelete)
                    })
                    .ToList();

                // 3) Relación padre -> hijos
                // Padre.Code = MENU_COMPANY => sufijo = COMPANY
                var rootBySuffix = roots.ToDictionary(
                    r => GetSuffixFromMenuCode(r.Code),   // COMPANY
                    r => r,
                    StringComparer.OrdinalIgnoreCase
                );

                foreach (var c in children)
                {
                    if (string.IsNullOrWhiteSpace(c.GroupCode)) continue;

                    if (rootBySuffix.TryGetValue(c.GroupCode.Trim(), out var parent))
                    {
                        parent.Children.Add(c);
                    }
                }

                // 4) Ordenar hijos dentro de cada padre
                foreach (var r in roots)
                {
                    r.Children = r.Children
                        .OrderBy(x => x.SortOrder)
                        .ThenBy(x => x.Title)
                        .ToList();
                }

                // 5) Opcional: solo devolver padres que tengan hijos
                roots = roots.Where(r => r.Children.Count > 0).ToList();

                return roots;
            }
        }

        private static string GetSuffixFromMenuCode(string code)
        {
            // MENU_COMPANY -> COMPANY
            if (string.IsNullOrWhiteSpace(code)) return code;
            code = code.Trim();

            const string prefix = "MENU_";
            if (code.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                return code.Substring(prefix.Length);

            return code;
        }
        //private List<MenuPageDto> GetMenuPagesForUser(long idCliente)
        //{
        //    using (var db = new SpainBS_Connection())
        //    {
        //        var roleIds = db.ClienteRoles
        //            .Where(x => x.id_cliente == idCliente)
        //            .Select(x => x.RoleId)
        //            .Distinct()
        //            .ToList();

        //        if (roleIds == null || roleIds.Count == 0)
        //            return new List<MenuPageDto>();

        //        var raw = (from rp in db.RolePages
        //                   join p in db.Pages on rp.PageId equals p.PageId
        //                   where roleIds.Contains(rp.RoleId)
        //                         && rp.CanView
        //                         && p.IsActive
        //                         && p.IsMenu
        //                   select new
        //                   {
        //                       p.Title,
        //                       p.Url,
        //                       p.IconCss,
        //                       p.GroupCode,
        //                       p.SortOrder,
        //                       rp.CanView,
        //                       rp.CanEdit,
        //                       rp.CanDelete
        //                   })
        //                   .ToList();

        //        var pages = raw
        //            .Where(x => !string.IsNullOrWhiteSpace(x.Url))
        //            .GroupBy(x => x.Url)
        //            .Select(g => new MenuPageDto
        //            {
        //                Title = g.First().Title,
        //                Url = g.Key,
        //                IconCss = g.First().IconCss,
        //                GroupCode = g.First().GroupCode,
        //                SortOrder = g.Min(x => x.SortOrder),

        //                CanView = true,
        //                CanEdit = g.Any(x => x.CanEdit),
        //                CanDelete = g.Any(x => x.CanDelete)
        //            })
        //            .OrderBy(x => x.GroupCode)
        //            .ThenBy(x => x.SortOrder)
        //            .ToList();

        //        return pages;
        //    }
        //}
    }

    public class MenuPageDto
    {
        public int PageId { get; set; }
        public string Code { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public string IconCss { get; set; }
        public string GroupCode { get; set; }
        public int SortOrder { get; set; }

        public bool CanView { get; set; }
        public bool CanEdit { get; set; }
        public bool CanDelete { get; set; }
    }

    public class MenuGroupDto
    {
        public int PageId { get; set; }
        public string Code { get; set; }          // MENU_COMPANY
        public string Title { get; set; }         // Company
        public string IconCss { get; set; }       // fa-solid fa-industry
        public int SortOrder { get; set; }        // 10

        public List<MenuPageDto> Children { get; set; } = new List<MenuPageDto>();
    }
}
