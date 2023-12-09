using TP10.Models;
using TP10.ViewModels;

namespace Kanban.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Contrasenia { get; set; }
        public string Rol { get; set; }

        public Usuario(ModificarUsuarioViewModel usu) 
        {
            Id = usu.Id;
            Nombre = usu.Nombre;
            Rol = usu.Rol;
            Contrasenia = usu.Contrasenia;
        }

        public Usuario(CrearUsuarioViewModel usu)
        {
            Nombre = usu.Nombre;
            Rol = usu.Rol;
            Contrasenia = usu.Contrasenia;
        }

        public Usuario() { }
    }
}
