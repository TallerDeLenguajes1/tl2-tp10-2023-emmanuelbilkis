using Kanban.Models;
using Kanban.Repositorios;
using Microsoft.AspNetCore.Mvc;
using TP10.ViewModels;

namespace TableroKanban.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly IUsuarioRepository _servicioUsuario;  
        public UsuarioController(IUsuarioRepository usuarioRepositorio)
        {
            _servicioUsuario = usuarioRepositorio;
        }

        public IActionResult Index() 
        {
            if (IsUser())
            {
                var usuarios = _servicioUsuario.GetAll();
                var model = usuarios.Select(u => new UsuarioViewModel
                {
                    Id = u.Id,
                    Nombre = u.Nombre,
                    Rol = u.Rol
                }).ToList();
                return View(model);
            }
            else
            {
                return RedirectToRoute(new { controller = "Login", action = "Index" });
            }
        }  

        public IActionResult Editar(int id) 
        {
            var usuario = _servicioUsuario.GetById(id);

            if (usuario != null)
            {
                var model = new ModificarUsuarioViewModel
                {
                    Id = usuario.Id,
                    Nombre = usuario.Nombre,
                    Contrasenia = usuario.Contrasenia,
                    Rol = usuario.Rol
                };

                return View(model); 
            }

            return RedirectToAction("Index"); 
        }

        [HttpPost]
        public IActionResult Editar(ModificarUsuarioViewModel usuarioEditado)
        {
            if (!ModelState.IsValid)
            {
                return View(usuarioEditado);
            }

            var usuario = new Usuario(usuarioEditado);
            _servicioUsuario.Update(usuario);
            return RedirectToRoute(new { controller = "Usuario", action = "Index" });
        }

        public IActionResult Borrar(int id)
        {
            _servicioUsuario.Remove(id);
            return RedirectToAction("Index");
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
            var usuario = new Usuario(usu);
            _servicioUsuario.Create(usuario);
            return RedirectToAction("Index");
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
