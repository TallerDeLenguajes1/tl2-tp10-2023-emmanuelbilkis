using Kanban.Models;
using TP10.Models;

namespace TP10.ViewModels
{
    public class CrearUsuarioViewModel
    {
        public string Nombre { get; set; }
        public string Contrasenia { get; set; }
        public string Rol { get; set; }

        public CrearUsuarioViewModel(Usuario usu)
        {
            Nombre = usu.Nombre;
            Contrasenia = usu.Contrasenia;
            Rol = usu.Rol;
        }

        public CrearUsuarioViewModel()
        {
            
        }
    }
}
