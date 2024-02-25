using Kanban.Models;
using Kanban.Repositorios;
using Microsoft.AspNetCore.Mvc;
using TP10.ViewModels;
using Microsoft.Extensions.Logging;
using MVC.Controllers;

namespace TableroKanban.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly IUsuarioRepositorio _servicioUsuario;
        private readonly ITareaRepositorio _tareaRepositorio;
        private readonly ILogger<LoginController> _logger;
        public UsuarioController(IUsuarioRepositorio usuarioRepositorio, ILogger<LoginController> logger, ITareaRepositorio tareaRepositorio)
        {
            _servicioUsuario = usuarioRepositorio;
            _logger = logger;
            _tareaRepositorio = tareaRepositorio;
        }

        public IActionResult Index() 
        {
            var model = ObtenerUsuarios();

            if (IsAdmin())
            {
                return View(model);
            }
            else
            {
                return RedirectToAction("IndexOperador", "Usuario");
            }
        }  
        private List<UsuarioViewModel> ObtenerUsuarios() 
        {
            var usuarios = _servicioUsuario.GetAll();
            var model = usuarios.Select(u => new UsuarioViewModel
            {
                Id = u.Id,
                Nombre = u.Nombre,
                Rol = u.Rol
            }).ToList();
            return model;
        }
        public IActionResult IndexOperador() 
        {

            int id = Convert.ToInt32(HttpContext.Session.GetString("Id"));
            var usus = _servicioUsuario.GetAll();   
            UsuarioOperadorViewModel model = new UsuarioOperadorViewModel(id, usus);

            if (model is not null)
            {
                return View(model);
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }

        public IActionResult Editar(int id) 
        {
            try
            {
                int idUsuConectado = Convert.ToInt32(HttpContext.Session.GetString("Id"));
                if (id == idUsuConectado)
                {
                    return RedirectToAction("EditarSinRol", new { id = id });
                }
                var usuario = _servicioUsuario.GetById(id);

                var model = new ModificarUsuarioViewModel(usuario);

                return View(model);
                
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                TempData["ErrorMessage"] = e.Message;
                return View();
            }
        }

        [HttpPost]
        public IActionResult Editar(ModificarUsuarioViewModel usuarioEditado)
        {
            if (!ModelState.IsValid)
            {
                return View(usuarioEditado);
            }

            try
            {
                var usuario = new Usuario(usuarioEditado);
                _servicioUsuario.Update(usuario);
                return RedirectToRoute(new { controller = "Usuario", action = "Index" });
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                TempData["ErrorMessage"] = e.Message;
                return View();
            }
            
        }

        public IActionResult EditarSinRol(int id) 
        {
            try
            {
                var usuario = _servicioUsuario.GetById(id);

                var model = new ModificarUsuarioViewModel
                {
                    Id = usuario.Id,
                    Nombre = usuario.Nombre,
                    Contrasenia = usuario.Contrasenia,
                    Rol = usuario.Rol
                };

                return View(model);

            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                TempData["ErrorMessage"] = e.Message;
                return View();
            }
        }


        [HttpPost]
        public IActionResult EditarSinRol(ModificarUsuarioViewModel usuarioEditado)
        {
            if (!ModelState.IsValid)
            {
                return View(usuarioEditado);
            }

            try
            {
                var usuario = new Usuario(usuarioEditado);
                _servicioUsuario.Update(usuario);
                return RedirectToRoute(new { controller = "Usuario", action = "Index" });
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                TempData["ErrorMessage"] = e.Message;
                return View();
            }

        }
        public IActionResult Borrar(int id)
        {
            try
            {
                int idUsuConec = Convert.ToInt32(HttpContext.Session.GetString("Id"));
                // _servicioUsuario.UpdateTareaUsu(id);
                var tareas = _tareaRepositorio.ListarPorUsuario(id);
                if (tareas.Count !=0 )
                {
                    foreach (var tar in tareas)
                    {
                        tar.IdUsuarioAsignado = 0;
                        _tareaRepositorio.Update(tar);
                    }
                }
                
                _servicioUsuario.Remove(id);
                
                if (id == idUsuConec)
                {
                    return RedirectToRoute(new { controller = "Login", action = "Index" });
                }
                else
                {
                    return RedirectToRoute(new { controller = "Usuario", action = "Index" });
                }
                
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                TempData["ErrorMessage"] = e.Message;
                return View();
            }
         
        }

        public IActionResult Alta()
        {
            return View();  
        }

        [HttpPost]
        public IActionResult Alta(CrearUsuarioViewModel usu)
        {
            if (!ModelState.IsValid)
            {
                return View(usu);
            }

            try
            {
                var usuario = new Usuario(usu);
                _servicioUsuario.Create(usuario);
                _logger.LogInformation("Se creé con éxito el usuario | Fecha: " + DateTime.Now.ToString());
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                TempData["ErrorMessage"] = e.Message;
                return View();
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
