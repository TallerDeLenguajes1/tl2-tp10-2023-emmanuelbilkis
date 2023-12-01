using Kanban.Models;
using Kanban.Repositorios;
using Microsoft.AspNetCore.Mvc;

namespace TableroKanban.Controllers
{
    public class UsuarioController : Controller
    {
        private UsuarioRepository _servicioUsuario;  
        public UsuarioController()
        {
            _servicioUsuario = new UsuarioRepository();
        }

        public IActionResult ListarUsuarios() 
        {
            if (IsUser())
            {
              var usuarios = _servicioUsuario.GetAll();
              return View(usuarios);
            }
            else
            {
                return RedirectToRoute("Login/Index");
            }

        } // este lo hare desp con un return de json 

        public IActionResult EditarIndex(int id) 
        {
            var usu = _servicioUsuario.GetById(id);
            return View(usu);   
        }

        [HttpPost]
        public IActionResult Editar(Usuario usuarioEditado)
        {
            _servicioUsuario.Update(usuarioEditado);
            return RedirectToAction("ListarUsuarios");   
        }

        public IActionResult Borrar(int id)
        {
            _servicioUsuario.Remove(id);
            return RedirectToAction("ListarUsuarios");
        }

        public IActionResult AltaIndex()
        {
            return View();  
        }

        [HttpPost]
        public IActionResult Alta(Usuario usu)
        {
            _servicioUsuario.Create(usu);
            return RedirectToAction("Index","Home");
        }
        private bool IsAdmin()
        {
            if (HttpContext.Session != null && HttpContext.Session.GetString("Rol") == "Admin")
                return true;

            return false;
        }

        private bool IsUser()
        {
            if (HttpContext.Session != null)
                return true;

            return false;
        }
    }
}
