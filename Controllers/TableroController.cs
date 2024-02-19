﻿using Kanban.Models;
using Kanban.Repositorios;
using Microsoft.AspNetCore.Mvc;
using MVC.Controllers;
using System.Data.Entity.Core.Metadata.Edm;
using System.Diagnostics;
using TP10.Models;
using TP10.Servicios;
using TP10.ViewModels;
using Microsoft.Extensions.Logging;
//using AspNetCore;
using System;

namespace TableroKanban.Controllers
{
    public class TableroController : Controller
    {
        private readonly ITableroRepositorio _servicioTablero;
        private readonly IUsuarioRepositorio _servicioUsuario;
        private readonly ILogger<LoginController> _logger;


        public TableroController(ITableroRepositorio tableroRepositorio,IUsuarioRepositorio usuarioRepositorio, ILogger<LoginController> logger)
        {
            _servicioTablero = tableroRepositorio;
            _servicioUsuario = usuarioRepositorio;
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
                            var tabs = _servicioTablero.GetAll();
                            var model = tabs.Select(u => new TableroViewModel
                            {
                                Id = u.Id,
                                Nombre = u.Nombre,
                                Descripcion = u.Descripcion,
                                UsuarioNombre = u.IdUsuarioPropietario == 0 ? "Sin usuario asignado" : _servicioUsuario.GetById(u.IdUsuarioPropietario).Nombre,
                            }).ToList();

                            return View(model);
                        }
                        else
                        {

                           return RedirectToRoute(new { controller = "Tablero", action = "IndexOperador" });

                        }
                    }
                    catch (Exception e)
                    {
                        _logger.LogError(e.Message);
                        TempData["ErrorMessage"] = "Error al obtener los tableros, consulte con el administrador.";
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
            try
            {
                int id = Convert.ToInt32(HttpContext.Session.GetString("Id"));
                var tabs = _servicioTablero.ListarPorUsuario(id);
                if (tabs.Count == 0)
                {
                    TempData["SinTableros"] = "Usted no tiene tableros";
                    return RedirectToRoute(new { controller = "Tablero", action = "AltaOperador" });
                }
                var model = tabs.Select(u => new TableroViewModel
                {
                    Id = u.IdTablero,
                    Nombre = u.Nombre,
                    Descripcion = u.Descripcion,
                    UsuarioNombre = u.UsuarioNombre,
                }).ToList();

                return View(model);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                TempData["ErrorMessage"] = "Error al obtener los tableros, consulte con el administrador.";
                return RedirectToRoute(new { controller = "Home", action = "Index" });
             
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

                _logger.LogError(e.Message);
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

                    _logger.LogError(e.Message);
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
                _logger.LogError(e.Message);
            }

            return RedirectToAction("Index");
        }

        public IActionResult Alta()
        {
            
            var listaUsuarios = _servicioUsuario.GetAll(); 

            
            var viewModel = new CrearTableroViewModel(listaUsuarios);

            
            return View(viewModel);
        }

        public IActionResult AltaOperador()
        {

            var model = new CrearTableroViewModel();
            model.IdUsuarioPropietario = Convert.ToInt32(HttpContext.Session.GetString("Id"));

            return View(model);
        }

        [HttpPost]
        public IActionResult AltaOperador(CrearTableroViewModel tab)
        {
            if (ModelState.IsValid)
            {
                var _tab = new Tablero(tab);

                try
                {
                    _servicioTablero.Create(_tab);
                    _logger.LogInformation("Se creo con éxito el tablero - Fecha: " + DateTime.Now.ToString());
                    return RedirectToAction("Index");
                }
                catch (Exception e)
                {
                    _logger.LogError(e.Message);
                }

            }

            return View(tab);
        }

        [HttpPost]
        public IActionResult Alta(CrearTableroViewModel tab)
        {
            if (ModelState.IsValid)
            {
                var _tab = new Tablero(tab);

                try
                {
                    _servicioTablero.Create(_tab);
                    _logger.LogInformation("Se creo con éxito el tablero - Fecha: " + DateTime.Now.ToString());
                    return RedirectToAction("Index");
                }
                catch (Exception e)
                {
                    _logger.LogError(e.Message);
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
