using Kanban.Models;
using Kanban.Repositorios;
using Microsoft.AspNetCore.Mvc;
using MVC.ViewModels;

namespace MVC.Controllers;

public class LoginController : Controller
{
  
    private readonly ILogger<LoginController> _logger;
    private IUsuarioRepository _servicioUsuario;
    public LoginController(ILogger<LoginController> logger)
    {
        _logger = logger;
        _servicioUsuario = new UsuarioRepository();     
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
        
        //Devuelvo el usuario al Home
        return RedirectToRoute(new { controller = "Home", action = "Index" });
    }

    private void logearUsuario(Usuario user)
    {
        HttpContext.Session.SetString("Usuario", user.Nombre);
        HttpContext.Session.SetString("NivelDeAcceso", user.Contrasenia);
        HttpContext.Session.SetString("Rol", ObtenerRol(user.IdRol));
    }

    private string ObtenerRol(int id) 
    {
        string rol = _servicioUsuario.GetRolById(id).ToString();
        return rol;
    }

}