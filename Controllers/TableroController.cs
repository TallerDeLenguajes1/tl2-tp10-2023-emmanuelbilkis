using Kanban.Models;
using Kanban.Repositorios;
using Microsoft.AspNetCore.Mvc;
using System.Data.Entity.Core.Metadata.Edm;
using TP10.ViewModels;

namespace TableroKanban.Controllers
{
    public class TableroController : Controller
    {
        private ITableroRepositorio _servicioTablero;
        private IUsuarioRepository _servicioUsuario;

        public TableroController()
        {
            _servicioTablero = new TableroRepositorio();
            _servicioUsuario = new UsuarioRepository();
        }
        public IActionResult Index()
        {
            if (IsAdmin())
            {
                var tabs = _servicioTablero.GetAll();
                var model = tabs.Select(u => new TableroViewModel
                {
                    Id = u.Id,
                    Nombre = u.Nombre,
                    Descripcion = u.Descripcion,
                    UsuarioNombre = _servicioUsuario.GetById(u.IdUsuarioPropietario)?.Nombre ?? "-",
                    UsuarioRol = _servicioUsuario.GetById(u.IdUsuarioPropietario)?.Rol ?? "-"
                }).ToList();

                return View(model);
            }
            else
            {
                if (!IsAdmin() && IsUser())
                {
                    string id = HttpContext.Session.GetString("Id");
                    var tabs = _servicioTablero.ListarPorUsuario(int.Parse(id));
                    var model = tabs.Select(u => new TableroViewModel
                    {
                        Id = u.Id,
                        Nombre = u.Nombre,
                        Descripcion = u.Descripcion,
                        UsuarioNombre = _servicioUsuario.GetById(u.IdUsuarioPropietario)?.Nombre ?? "-",
                        UsuarioRol = _servicioUsuario.GetById(u.IdUsuarioPropietario)?.Rol ?? "-"

                    }).ToList();
                    return View(model);
                }
                else
                {
                    return RedirectToRoute(new { controller = "Login", action = "Index" });
                }
            }
        }

        [HttpGet]
        public IActionResult Editar(int id)
        {
            var tab = _servicioTablero.GetById(id);
            // hacer control de null
            var tabView = new ModificarTableroViewModel(tab);
            return View(tabView);
        }

        [HttpPost]
        public IActionResult Editar(ModificarTableroViewModel tablero)
        {
            if (ModelState.IsValid) 
            {
                var tableroEditado = new Tablero(tablero);
                _servicioTablero.Update(tableroEditado);
                return RedirectToAction("Index");
            }
            else
            {
                return View(tablero);
            }
        }

        public IActionResult Borrar(int id)
        {
            _servicioTablero.Remove(id);
            return RedirectToAction("Index");
        }

        public IActionResult Alta()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Alta(CrearTableroViewModel tab)
        {
            if (ModelState.IsValid)
            {
                var _tab = new Tablero(tab);
                _servicioTablero.Create(_tab);
                return RedirectToAction("Index");
            }
            else
            {
                return View(tab);
            }
        }

        private bool IsAdmin()
        {
            if (HttpContext.Session != null && HttpContext.Session.GetString("Rol") == "Admin")
                return true;

            return false;
        }

        private bool IsUser()
        {
            if ((HttpContext.Session != null) && (HttpContext.Session.GetString("Rol") == "Admin" || HttpContext.Session.GetString("Rol") == "Operador"))
                return true;

            return false;
        }
    }
}
