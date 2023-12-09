using Kanban.Models;

namespace TP10.ViewModels
{
    public class ModificarUsuarioViewModel
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Contrasenia { get; set; }
        public string Rol { get; set; }

        public ModificarUsuarioViewModel(Usuario usu)
        {
            Id = usu.Id;
            Nombre = usu.Nombre;
            Contrasenia = usu.Contrasenia;
            Rol = usu.Rol;
        }

        public ModificarUsuarioViewModel()
        {
           
        }
    }
}
