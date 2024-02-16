using Kanban.Models;
using Kanban.Repositorios;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TP10.Models;
using TP10.ViewModels;

namespace TableroKanban.Controllers
{
    public class TareaController : Controller
    {
        private readonly ITareaRepositorio _servicioTarea;
        private readonly IUsuarioRepositorio _servicioUsuario;
        private readonly ITableroRepositorio _servicioTablero;

        public TareaController(ITableroRepositorio tableroRepositorio, IUsuarioRepositorio usuarioRepositorio,ITareaRepositorio tareaRepositorio)
        {
            _servicioTarea = tareaRepositorio;
            _servicioUsuario = usuarioRepositorio;
            _servicioTablero = tableroRepositorio;
        }
        public IActionResult Index()
        {
            if (IsUser())
            {

            
                    try
                    {
                        if (IsAdmin())
                        {
                            var tareas = _servicioTarea.ObtenerTareasConUsuTablero();
                            return View(tareas);
                        }
                        else
                        {

                            string id = HttpContext.Session.GetString("Id");
                            string rolConectado = HttpContext.Session.GetString("Rol");
                            var tareas = _servicioTarea.ListarPorUsuario(int.Parse(id));
                            var model = tareas.Select(u => new TareaViewModel
                            {
                                RolUsuarioConectado = rolConectado,
                                Id = u.Id,
                                Nombre = u.Nombre,
                                Estado = u.Estado,
                                Color = u.Color,
                                Descripcion = u.Descripcion,
                                UsuarioAsignado = _servicioUsuario.GetById(u.IdUsuarioAsignado)?.Nombre ?? "-",
                                TableroAsignado = _servicioTablero.GetById(u.IdTablero)?.Nombre ?? "-"
                                
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

                        return View("~/Views/Shared/Error.cshtml", errorViewModel);
                    }
            }
            else
            {
                return RedirectToRoute(new { controller = "Login", action = "Index" });
            }
        }
        public IActionResult Editar(int id)
        {
            var tarea = _servicioTarea.GetById(id);
            var usuarios = _servicioUsuario.GetAll();
            var tableros = _servicioTablero.GetAll();
            var model = new ModificarTareaViewModel(tarea,usuarios,tableros);
            return View(model);
        }

        [HttpPost]
        public IActionResult Editar(ModificarTareaViewModel tareaEditada)
        {
            if (!ModelState.IsValid) 
            {
                return View(tareaEditada);
            }

            try
            {
                valdiadIdTablero(tareaEditada.IdTablero);
                valdiadIdUsuario(tareaEditada.IdUsuarioAsignado);
                /*var TablerosConUsus = _servicioTablero.ObtenerTablerosConUsuario(tareaEditada.IdUsuarioAsignado, tareaEditada.IdTablero);
                if (TablerosConUsus is null || TablerosConUsus.Count == 0)
                {
                    throw new InvalidOperationException("El tablero no es administrado por el usuario seleccionado");
                }*/
                var tarea = new Tarea(tareaEditada);
                _servicioTarea.Update(tarea);
                return RedirectToAction("Index");

            }
            catch (Exception e)
            {
                var errorViewModel = new ErrorViewModel
                {
                    ErrorMessage = e.Message,
                    RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
                };

                return View("~/Views/Shared/Error.cshtml", errorViewModel);
            }
        }

        public IActionResult Borrar(int id)
        {
            try
            {
                _servicioTarea.Remove(id);
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {

                var errorViewModel = new ErrorViewModel
                {
                    ErrorMessage = e.Message,
                    RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
                };

                return View("~/Views/Shared/Error.cshtml", errorViewModel);
            }
          
        }

        public IActionResult Alta()
        {
            var tableros = _servicioTablero.GetAll();
            var usuarios = _servicioUsuario.GetAll();
            var model = new CrearTareaViewModel(tableros,usuarios);
            return View(model);
        }

        [HttpPost]
        public IActionResult Alta(CrearTareaViewModel tar)
        {
            if (!ModelState.IsValid)
            {
                return View(tar);
            }

            try
            {
                valdiadIdTablero(tar.IdTablero);
                valdiadIdUsuario(tar.IdUsuarioAsignado);
                var TablerosConUsus = _servicioTablero.ObtenerTablerosConUsuario(tar.IdUsuarioAsignado,tar.IdTablero);
                if (TablerosConUsus is null || TablerosConUsus.Count==0 )
                {
                    throw new InvalidOperationException("El tablero no es administrado por el usuario seleccionado");
                }

                var tarea = new Tarea(tar);
                _servicioTarea.Create(tarea);
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                var errorViewModel = new ErrorViewModel
                {
                    ErrorMessage = e.Message,
                    RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
                };

                return View("~/Views/Shared/Error.cshtml", errorViewModel);
            }
            
        }

        [HttpGet]
        public IActionResult TareasDeTablero(int Id) 
        {
            try
            {
                /*if (!IsAdmin())
                {
                    int idUsu = Convert.ToInt32 (HttpContext.Session.GetString("Id"));
                    var TablerosConUsus = _servicioTablero.ObtenerTablerosConUsuario(idUsu, Id);
                    if (TablerosConUsus is null || TablerosConUsus.Count == 0)
                    {
                        HttpContext.Response.StatusCode = 404;
                        return NotFound(); 
                    }
                }*/
                string id = HttpContext.Session.GetString("Id");
                int idUsuarioConectado = int.Parse(id);
                var tareas = _servicioTarea.ListarPorTablero(Id);
                var model = tareas.Select(u => new TareaViewModel
                {
                    UsuarioTableroAsignadoId = _servicioTablero.GetById(u.IdTablero).IdUsuarioPropietario,
                    IdUsuarioConectado=idUsuarioConectado,  
                    Id = u.Id,
                    Nombre = u.Nombre,
                    Estado = u.Estado,
                    Color = u.Color,
                    Descripcion = u.Descripcion,
                    UsuarioAsignado = _servicioUsuario.GetById(u.IdUsuarioAsignado)?.Nombre ?? "-",
                    TableroAsignado = _servicioTablero.GetById(u.IdTablero)?.Nombre ?? "-"

                }).ToList();

                return View(model);
            }
            catch (Exception e )
            {
                var errorViewModel = new ErrorViewModel
                {
                    ErrorMessage = e.Message,
                    RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
                };

                return View("~/Views/Shared/Error.cshtml", errorViewModel);
            }
            
        }

        public IActionResult AsignarUsuarios(int idTarea,string usuarioAsignado)
        {
            
            var tarea = _servicioTarea.GetById(idTarea);
            var usuarios = _servicioUsuario.GetAll().Where(a => a.Id != tarea.IdUsuarioAsignado);
            var model = new TareaUsuarioViewModel(idTarea,tarea.Nombre,"descripcion",usuarios,usuarioAsignado);
            return View(model);
        }

        
        public IActionResult Asignar(int idUsuario, int idTarea)
        {
            try
            {
                valdiadIdUsuario(idUsuario);

                _servicioTarea.Asignar(idUsuario,idTarea);
                return RedirectToAction("Index");

            }
            catch (Exception e)
            {
                var errorViewModel = new ErrorViewModel
                {
                    ErrorMessage = e.Message,
                    RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
                };

                return View("~/Views/Shared/Error.cshtml", errorViewModel);
            }

        }

        public IActionResult CambiarEstado(int idTarea) 
        {
            var tarea = _servicioTarea.GetById(idTarea);
            var model = new ModificarTareaViewModel(tarea);
             
            return View(model);
        }

        private bool IsAdmin()
        {
            if (HttpContext.Session != null && HttpContext.Session.GetString("Rol") == "Admin")
                return true;

            return false;
        }

        private bool IsUser()
        {
            if (HttpContext.Session != null && (HttpContext.Session.GetString("Rol") == "Admin" || HttpContext.Session.GetString("Rol") == "Operador"))
                return true;

            return false;
        }

        private void valdiadIdUsuario(int idUsu) 
        {
            _servicioUsuario.GetById(idUsu);
        }

        private void valdiadIdTablero(int idTablero)
        
        {
            _servicioTablero.GetById(idTablero);
        }

    }
}
