using Kanban.Models;
using Kanban.Repositorios;
using Microsoft.AspNetCore.Mvc;
using System.Data.Entity.Core.Metadata.Edm;
using System.Diagnostics;
using TP10.Models;
using TP10.Servicios;
using TP10.ViewModels;

namespace TableroKanban.Controllers
{
    public class TableroController : Controller
    {
        private readonly ITableroRepositorio _servicioTablero;
        private readonly IUsuarioRepositorio _servicioUsuario;
       

        public TableroController(ITableroRepositorio tableroRepositorio,IUsuarioRepositorio usuarioRepositorio)
        {
            _servicioTablero = tableroRepositorio;
            _servicioUsuario = usuarioRepositorio;
            
        }
        public IActionResult Index()
        {
            if (IsUser())
            {

            
                    try
                    {
                        if (IsAdmin())
                        {
                            var tabs = _servicioTablero.ObtenerTablerosConUsuario();  
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
                            string id = HttpContext.Session.GetString("Id");
                            int idUsuarioConectado = int.Parse(id);
                            var tableros = _servicioTablero.ListarTablerosPropiosYConTareas(idUsuarioConectado);
                            
                            var model = tableros.Select(u => new TableroViewModel
                            {
                                IdUsuarioConectado=idUsuarioConectado,  
                                IdUsuarioAsignado = u.IdUsuarioPropietario,
                                Id = u.Id,
                                Nombre = u.Nombre,
                                Descripcion = u.Descripcion,
                                UsuarioNombre = _servicioUsuario.GetById(u.IdUsuarioPropietario)?.Nombre ?? "-",
                                UsuarioRol = _servicioUsuario.GetById(u.IdUsuarioPropietario)?.Rol ?? "-"
                            }).ToList();

                            return View(model);
                        }
                    }
                    catch (Exception e)
                    {
                        var errorViewModel = new ErrorViewModel
                        {
                            ErrorMessage = e.Message,
                            RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
                        };

                        return RedirectToAction("Error", errorViewModel);
                    }

            }
            else
            {
                return RedirectToRoute(new { controller = "Login", action = "Index" });
            }
        }

        [HttpGet]
        public IActionResult Editar(int id)
        {
            try
            {
               var tab = _servicioTablero.GetById(id);
               var usuarios = _servicioUsuario.GetAll(); 
               var tabView = new ModificarTableroViewModel(tab,usuarios);
               return View(tabView);
            }
            catch (Exception e)
            {
                var errorViewModel = new ErrorViewModel
                {
                    ErrorMessage = e.Message,
                    RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
                };

                return RedirectToAction("Error", errorViewModel);
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
                    var errorViewModel = new ErrorViewModel
                    {
                        ErrorMessage = e.Message,
                        RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
                    };

                    return RedirectToAction("Error", errorViewModel);
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
                var errorViewModel = new ErrorViewModel
                {
                    ErrorMessage = e.Message,
                    RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
                };

                return RedirectToAction("Error", errorViewModel);
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
                    var errorViewModel = new ErrorViewModel
                    {
                        ErrorMessage = e.Message,
                        RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
                    };

                    return RedirectToAction("Error", errorViewModel);
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
