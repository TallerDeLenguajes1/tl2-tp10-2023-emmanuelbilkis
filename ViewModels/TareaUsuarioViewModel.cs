using Kanban.Models;

namespace TP10.ViewModels
{
    public class TareaUsuarioViewModel
    {
        
        public TareaUsuarioViewModel(int id, string nombre, string? descripcion, IEnumerable<Usuario> usuarios, string usuarioAsignadoActual)
        {
            Id = id;
            Nombre = nombre;
            Descripcion = descripcion;
            Usuarios = usuarios;
            //IdUsuarioAsignadoActual = idUsuarioAsignado;
            UsuarioAsignadoActual = usuarioAsignadoActual;
        }



        public int Id { get; set; }
        public string Nombre { get; set; }
        public string? Descripcion { get; set; }
        public IEnumerable<Usuario> Usuarios { get; set; }
        //public int IdUsuarioAsignadoActual { get; set; }
        public string? UsuarioAsignadoActual { get; set; }
    }
}
