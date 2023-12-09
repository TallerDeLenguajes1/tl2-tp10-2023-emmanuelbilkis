using Kanban.Models;

namespace TP10.ViewModels
{
    public class TableroViewModel
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string UsuarioNombre { get; set; }
        public string UsuarioRol { get; set; }

        public TableroViewModel(Tablero tab, UsuarioViewModel usu) 
        {
            Id = tab.Id;
            Nombre = tab.Nombre;
            Descripcion = tab.Descripcion;
            UsuarioNombre = usu.Nombre;
            UsuarioRol = usu.Rol;
        }

        public TableroViewModel() { }
    }
}
