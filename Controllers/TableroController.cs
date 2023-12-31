﻿using Kanban.Models;
using Kanban.Repositorios;
using Microsoft.AspNetCore.Mvc;

namespace TableroKanban.Controllers
{
    public class TableroController : Controller
    {
        private ITableroRepositorio _servicioTablero;

        public TableroController()
        {
            _servicioTablero = new TableroRepositorio();   
        }
        public IActionResult Index()
        {
            var tabs = _servicioTablero.GetAll();
            return View(tabs);
        }
        public IActionResult EditarIndex(int id)
        {
            var tab = _servicioTablero.GetById(id);
            return View(tab);
        }

        [HttpPost]
        public IActionResult Editar(Tablero tableroEditado)
        {
            _servicioTablero.Update(tableroEditado);
            return RedirectToAction("Index");
        }

        public IActionResult Borrar(int id)
        {
            _servicioTablero.Remove(id);
            return RedirectToAction("Index");
        }

        public IActionResult AltaIndex()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Alta(Tablero tab)
        {
            _servicioTablero.Create(tab);
            return RedirectToAction("Index");
        }
    }
}
