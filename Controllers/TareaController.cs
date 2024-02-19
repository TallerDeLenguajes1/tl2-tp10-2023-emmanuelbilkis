using Kanban.Models;
using Kanban.Repositorios;
using Microsoft.AspNetCore.Mvc;
using MVC.Controllers;
using System.Diagnostics;
using TP10.Models;
using TP10.ViewModels;
using Microsoft.Extensions.Logging;

namespace TableroKanban.Controllers
{
    public class TareaController : Controller
    {
        private readonly ITareaRepositorio _servicioTarea;
        private readonly IUsuarioRepositorio _servicioUsuario;
        private readonly ITableroRepositorio _servicioTablero;
        private readonly ILogger<LoginController> _logger;
        public TareaController(ITableroRepositorio tableroRepositorio, IUsuarioRepositorio usuarioRepositorio,ITareaRepositorio tareaRepositorio, ILogger<LoginController> logger)
        {
            _servicioTarea = tareaRepositorio;
            _servicioUsuario = usuarioRepositorio;
            _servicioTablero = tableroRepositorio;
            _logger = logger;   
        }
        public IActionResult Index()
        {
            if (IsUser())
            {
                    try
                    {
                        if (IsAdmin())
                        {
                            var tareas = _servicioTarea.GetAll();
                            
                            if (tareas.Count == 0) 
                            {
                                TempData["SinTareas"] = "No hay tareas para mostrar, aquí puede crear una";
                                return RedirectToAction("Alta");  
                            }

                            var model = tareas.Select(u => new TareaViewModel
                            {
                                Id = u.Id,
                                Nombre = u.Nombre,
                                Descripcion = u.Descripcion,
                                Color = u.Color,
                                Estado = u.Estado,  
                                UsuarioAsignado = u.IdUsuarioAsignado == 0 ? "Sin usuario asignado" : _servicioUsuario.GetById(u.IdUsuarioAsignado).Nombre,
                                TableroAsignado = u.IdTablero == 0 ? "Sin tablero asignado" : _servicioTablero.GetById(u.IdTablero).Nombre
                            }).ToList();

                             return View(model);

                        }
                        else
                        {
                           return RedirectToAction("IndexOperador");
                        }
                    }
                    catch (Exception e)
                    {
                       _logger.LogError(e.Message);
                       return RedirectToRoute(new { controller = "Home", action = "Index" });
                    }
            }
            else
            {
                return RedirectToRoute(new { controller = "Login", action = "Index" });
            }
        }

        public IActionResult IndexOperador()
        {
            string id = HttpContext.Session.GetString("Id");
            string rolConectado = HttpContext.Session.GetString("Rol");
            var tareas = _servicioTarea.ListarPorUsuario(int.Parse(id));
            var model = tareas.Select(u => new TareaViewModel
            {
                //RolUsuarioConectado = rolConectado,
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
                var tarea = new Tarea(tareaEditada);
                _servicioTarea.Update(tarea);
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return RedirectToAction("Index");
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
                _logger.LogError(e.Message);
                return RedirectToAction("Index");
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
                var tarea = new Tarea(tar);
                _servicioTarea.Create(tarea);
                _logger.LogInformation("Se creó con éxito la tarea | Fecha: "+ DateTime.Now.ToString());
                
            }
            catch (Exception e)
            {
                
                _logger.LogError(e.Message);
                
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult TareasDeTablero(int Id) 
        {
            try
            {
                var tareas = _servicioTarea.ListarPorTablero(Id);

                if (tareas.Count == 0 || tareas is null)
                {
                    TempData["SinTareas"] = "Esta tablero no tiene tareas - Aquí puede crear tareas";
                    return RedirectToAction("Alta");
                }

                var model = tareas.Select(u => new TareaViewModel
                { 
                    Id = u.Id,
                    Nombre = u.Nombre,
                    Estado = u.Estado,
                    Color = u.Color,
                    Descripcion = u.Descripcion,
                    UsuarioAsignado = u.IdUsuarioAsignado == 0 ? "Sin usuario asignado" : _servicioUsuario.GetById(u.IdUsuarioAsignado).Nombre,
                    TableroAsignado = u.IdTablero == 0 ? "Sin tablero asignado" : _servicioTablero.GetById(u.IdTablero).Nombre

                }).ToList();

                return View(model);
            }
            catch (Exception e )
            {
              
                _logger.LogError(e.Message);
                return RedirectToAction("Index");
            }
            
        }

        public IActionResult AsignarUsuario(int idTarea, string usuAsignado)
        {
            
            var tarea = _servicioTarea.GetById(idTarea);
            var usuarios = _servicioUsuario.GetAll().Where(a => a.Id != tarea.IdUsuarioAsignado);
            var model = new TareaUsuarioViewModel(idTarea,tarea.Nombre,tarea.Descripcion,usuarios, usuAsignado);
            return View(model);
        }

        
        public IActionResult Asignar(int idUsuario, int idTarea)
        {
            try
            {
                _servicioTarea.AsignarUsuario(idUsuario,idTarea);
                return RedirectToAction("Index");

            }
            catch (Exception e)
            {
                _logger.LogWarning(e.Message);
                return RedirectToAction("Index");
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
    }
}
