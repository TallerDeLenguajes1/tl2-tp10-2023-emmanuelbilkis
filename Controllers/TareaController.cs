using Kanban.Models;
using Kanban.Repositorios;
using Microsoft.AspNetCore.Mvc;
using TP10.ViewModels;

namespace TableroKanban.Controllers
{
    public class TareaController : Controller
    {
        private readonly ITareaRepositorio _servicioTarea;
        private readonly IUsuarioRepository _servicioUsuario;
        private readonly ITableroRepositorio _servicioTablero;

        public TareaController(ITableroRepositorio tableroRepositorio, IUsuarioRepository usuarioRepositorio,ITareaRepositorio tareaRepositorio)
        {
            _servicioTarea = tareaRepositorio;
            _servicioUsuario = usuarioRepositorio;
            _servicioTablero = tableroRepositorio;
        }
        public IActionResult Index()
        {
            if (IsAdmin())
            {
                var tareas = _servicioTarea.GetAll();
                var model = tareas.Select(u => new TareaViewModel
                {
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
            else
            {
                if (!IsAdmin() && IsUser())
                {
                    string id = HttpContext.Session.GetString("Id");
                    var tareas = _servicioTarea.ListarPorUsuario(int.Parse(id));
                    var model = tareas.Select(u => new TareaViewModel
                    {
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
                else
                {
                    return RedirectToRoute(new { controller = "Usuario", action = "Index" });
                }
            }
        }
        public IActionResult Editar(int id)
        {
            var tarea = _servicioTarea.GetById(id);
            var model = new ModificarTareaViewModel(tarea);
            return View(model);
        }

        [HttpPost]
        public IActionResult Editar(ModificarTareaViewModel tareaEditada)
        {
            if (!ModelState.IsValid) 
            {
                return View(tareaEditada);
            }
            var tarea = new Tarea(tareaEditada);
            _servicioTarea.Update(tarea);
            return RedirectToAction("Index");
        }

        public IActionResult Borrar(int id)
        {
            _servicioTarea.Remove(id);
            return RedirectToAction("Index");
        }

        public IActionResult Alta()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Alta(CrearTareaViewModel tar)
        {
            if (!ModelState.IsValid)
            {
                return View(tar);
            }
            var tarea = new Tarea(tar);
            _servicioTarea.Create(tarea);   
            return RedirectToAction("Index");
        }


        [HttpGet]
        public IActionResult TareasDeTablero(int Id) 
        {
            try
            {
                var tareas = _servicioTarea.ListarPorTablero(Id);
                var model = tareas.Select(u => new TareaViewModel
                {
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
                // logs
                return RedirectToRoute(new { controller = "Tablero", action = "Index" });
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
            if (HttpContext.Session != null && (HttpContext.Session.GetString("Rol") == "Admin" || HttpContext.Session.GetString("Rol") == "Operador"))
                return true;

            return false;
        }

    }
}
