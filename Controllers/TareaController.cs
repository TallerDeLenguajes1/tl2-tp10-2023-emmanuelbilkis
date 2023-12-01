using Kanban.Models;
using Kanban.Repositorios;
using Microsoft.AspNetCore.Mvc;

namespace TableroKanban.Controllers
{
    public class TareaController : Controller
    {
        private ITareaRepositorio _servicioTarea;

        public TareaController()
        {
            _servicioTarea = new TareaRepositorio();
        }
        public IActionResult Index()
        {
            if (IsUser())
            {
                var tabs = _servicioTarea.GetAll();
                return View(tabs);
            }
            else
            {
                return RedirectToRoute("Login/Index");
            }
            
        }
        public IActionResult EditarIndex(int id)
        {
            var tab = _servicioTarea.GetById(id);
            return View(tab);
        }

        [HttpPost]
        public IActionResult Editar(Tarea tareaEditada)
        {
            _servicioTarea.Update(tareaEditada);
            return RedirectToAction("Index");
        }

        public IActionResult Borrar(int id)
        {
            _servicioTarea.Remove(id);
            return RedirectToAction("Index");
        }

        public IActionResult AltaIndex()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Alta(Tarea tar)
        {
            _servicioTarea.Create(tar);
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
            if (HttpContext.Session != null)
                return true;

            return false;
        }
    }
}
