using Kanban.Models;
using Kanban.Repositorios;
using Microsoft.AspNetCore.Mvc;
using MVC.ViewModels;

namespace MVC.Controllers;

public class LoginController : Controller
{

    private readonly ILogger<LoginController> _logger;
    private readonly IUsuarioRepository _servicioUsuario;
    public LoginController(ILogger<LoginController> logger,IUsuarioRepository usuarioRepositorio)
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
        if (usuarioLogeado == null) return RedirectToAction("Index");

        //Registro el usuario
        logearUsuario(usuarioLogeado);
        
        return RedirectToRoute(new { controller = "Usuario", action = "Index" });
    }

    private void logearUsuario(Usuario user)
    {
        HttpContext.Session.SetString("Usuario", user.Nombre);
        HttpContext.Session.SetString("Contrasenia", user.Contrasenia);
        HttpContext.Session.SetString("Rol",user.Rol);
        HttpContext.Session.SetString("Id", Convert.ToString(user.Id));
    }
}