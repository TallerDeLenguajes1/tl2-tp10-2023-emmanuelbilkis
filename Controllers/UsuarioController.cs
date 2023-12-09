using Kanban.Models;
using Kanban.Repositorios;
using Microsoft.AspNetCore.Mvc;
using TP10.ViewModels;

namespace TableroKanban.Controllers
{
    public class UsuarioController : Controller
    {
        private UsuarioRepository _servicioUsuario;  
        public UsuarioController()
        {
            _servicioUsuario = new UsuarioRepository();
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

        public IActionResult EditarIndex(int id) 
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
            var usuario = new Usuario(usuarioEditado);
            _servicioUsuario.Update(usuario);
            return RedirectToRoute(new { controller = "Usuario", action = "Index" });
        }

        public IActionResult Borrar(int id)
        {
            _servicioUsuario.Remove(id);
            return RedirectToAction("Index");
        }

        public IActionResult AltaIndex()
        {
            return View();  
        }

        [HttpPost]
        public IActionResult Alta(CrearUsuarioViewModel usu)
        {
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
