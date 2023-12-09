using Kanban.Models;

namespace TP10.ViewModels
{
    public class TareaViewModel
    {
        public int Id { get; set; }
        public string Tablero { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string Color { get; set; }
        public int Estado { get; set; }
        public string UsuarioAsignado { get; set; }
        public string TableroAsignado { get; set; }

        public TareaViewModel(Tarea tarea,TableroViewModel tab,UsuarioViewModel usu) 
        {
            Id= tarea.Id;
            Tablero = tab.Nombre;
            Nombre = tarea.Nombre;
            Descripcion = tarea.Descripcion;
            Color = tarea.Color;
            Estado = tarea.Estado;
            UsuarioAsignado = usu.Nombre;
            TableroAsignado = tab.Nombre;
        }

        public TareaViewModel() { }
    }
}
