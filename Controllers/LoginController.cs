using Kanban.Models;
using Kanban.Repositorios;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MVC.ViewModels;
using TP10.Models;

namespace MVC.Controllers;

public class LoginController : Controller
{

    private readonly ILogger<LoginController> _logger;
    private readonly IUsuarioRepositorio _servicioUsuario;
    public LoginController(ILogger<LoginController> logger,IUsuarioRepositorio usuarioRepositorio)
    {
        _logger = logger;
        _servicioUsuario = usuarioRepositorio;
    }

    public IActionResult Index()
    {
        return View(new LoginViewModel());
    }

    public IActionResult Login(Usuario usuario)
    {
        //existe el usuario?
        var usuarios = _servicioUsuario.GetAll();
        var usuarioLogeado = usuarios.FirstOrDefault(u => u.Nombre == usuario.Nombre && u.Contrasenia == usuario.Contrasenia);

        // si el usuario no existe devuelvo al index
        if (usuarioLogeado == null) 
        {
            string mensaje = $"Intento de acceso inválido - Usuario: {usuarioLogeado} ";
            _logger.LogWarning(mensaje);
            return RedirectToAction("Index");
        }
        else
        {
            string mensaje = $"El usuario {usuarioLogeado} ingresó correctamente.";
            _logger.LogInformation(mensaje);
        }

        //Registro el usuario
        logearUsuario(usuarioLogeado);
        
        return RedirectToRoute(new { controller = "Home", action = "Index" });
    }

    public IActionResult CerrarSesion()
    {
        HttpContext.Session.Clear();
        HttpContext.Session.Remove("Usuario");
        HttpContext.Session.Remove("Contrasenia");
        return RedirectToAction("Index", "Login"); 
    }
    private void logearUsuario(Usuario user)
    {
        HttpContext.Session.SetString("Usuario", user.Nombre);
        HttpContext.Session.SetString("Contrasenia", user.Contrasenia);
        HttpContext.Session.SetString("Rol",user.Rol.ToString());
        HttpContext.Session.SetString("Id", user.Id.ToString());
    }
}