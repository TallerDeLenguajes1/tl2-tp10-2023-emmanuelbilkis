using Kanban.Models;

namespace TP10.ViewModels
{
    public class TableroViewModel
    {
        public int IdUsuarioConectado { get; set; }
        public int IdUsuarioAsignado { get; set; }
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string UsuarioNombre { get; set; }
        public string UsuarioRol { get; set; } 

        public TableroViewModel(int idTablero,string nombreTablero,string descripcionTablero,string nombreUsu,string rolUsu,int idUsuarioConectado,int idUsuarioAsignado) 
        {
            Id = idTablero;
            Nombre = nombreTablero; 
            Descripcion = descripcionTablero;   
            UsuarioNombre = nombreUsu;  
            UsuarioRol = rolUsu;
            IdUsuarioAsignado = idUsuarioAsignado;
            IdUsuarioConectado = idUsuarioConectado;
        }

        public TableroViewModel() { }
    }
}
