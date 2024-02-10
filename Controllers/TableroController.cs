using Kanban.Models;
using Kanban.Repositorios;
using Microsoft.AspNetCore.Mvc;
using System.Data.Entity.Core.Metadata.Edm;
using TP10.ViewModels;

namespace TableroKanban.Controllers
{
    public class TableroController : Controller
    {
        private readonly ITableroRepositorio _servicioTablero;
        private readonly IUsuarioRepository _servicioUsuario;

        public TableroController(ITableroRepositorio tableroRepositorio,IUsuarioRepository usuarioRepositorio)
        {
            _servicioTablero = tableroRepositorio;
            _servicioUsuario = usuarioRepositorio;
        }
        public IActionResult Index()
        {
            if (IsAdmin())
            {
                var tabs = _servicioTablero.ObtenerTablerosConUsuario(); // llamar a un inner join y adaptarlo al view model 
                var model = tabs.Select(u => new TableroViewModel
                {
                    Id = u.IdTablero,
                    Nombre = u.Nombre,
                    Descripcion = u.Descripcion,
                    UsuarioNombre = u.UsuarioNombre,
                    UsuarioRol = u.UsuarioRol
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
            try
            {
               var tab = _servicioTablero.GetById(id);
               var tabView = new ModificarTableroViewModel(tab);
               return View(tabView);
            }
            catch (Exception e)
            {
                TempData["ErrorMessage"] = e.Message;
                return View();
            }
        }

        [HttpPost]
        public IActionResult Editar(ModificarTableroViewModel tablero)
        {
            if (ModelState.IsValid) 
            {
                try
                {
                    var idUSuario = _servicioUsuario.GetById(tablero.IdUsuarioPropietario).Id;
                    var tableroEditado = new Tablero(tablero);
                    _servicioTablero.Update(tableroEditado);
                    return RedirectToAction("Index");
                }
                catch (Exception e)
                {
                    TempData["ErrorMessage"] = e.Message;
                }
            }

             return View(tablero);
        }

        public IActionResult Borrar(int id)
        {
            try
            {
                _servicioTablero.Remove(id);
            }
            catch (Exception e)
            {
                TempData["ErrorMessage"] = e.Message;
            }
            
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

                try
                {
                    var idUSuario = _servicioUsuario.GetById(tab.IdUsuarioPropietario).Id;
                    _servicioTablero.Create(_tab);
                    return RedirectToAction("Index");
                }
                catch (Exception e)
                {
                    TempData["ErrorMessage"] = e.Message;
                }
                
            }
                return View(tab);
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
