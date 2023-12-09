using Kanban.Models;

namespace TP10.ViewModels
{
    public class UsuarioViewModel
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Rol { get; set; }

        public UsuarioViewModel(Usuario usu) 
        {
            Id = usu.Id;
            Nombre = usu.Nombre;
            Rol = usu.Rol;
        }

        public UsuarioViewModel() { }
    }
}
