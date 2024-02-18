using Kanban.Models;

namespace TP10.ViewModels
{
    public class UsuarioOperadorViewModel
    {
        public int IdUsuarioConectado { get; set; }
        public List<Usuario> Usuarios { get; set; }

        public UsuarioOperadorViewModel(int id, List<Usuario> usus) 
        {
            Usuarios = usus;    
            IdUsuarioConectado = id;    
        }

        public UsuarioOperadorViewModel()
        {
            Usuarios = new List<Usuario>();
        }

    }
}
