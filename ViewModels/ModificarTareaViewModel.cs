using Kanban.Models;

namespace TP10.ViewModels
{
    public class ModificarTareaViewModel
    {
        public int Id { get; set; }
        public int IdTablero { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string Color { get; set; }
        public int Estado { get; set; }
        public int IdUsuarioAsignado { get; set; }
        
        public ModificarTareaViewModel(Tarea tarea) 
        {
            Id = tarea.Id;
            IdTablero = tarea.IdTablero;
            Nombre = tarea.Nombre;
            Descripcion = tarea.Descripcion;
            Color = tarea.Color;
            Estado = tarea.Estado;
            IdUsuarioAsignado = tarea.IdUsuarioAsignado;
        }

        public ModificarTareaViewModel()
        {
        
        }
    }
}
