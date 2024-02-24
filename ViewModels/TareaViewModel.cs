using Kanban.Models;
using TP10.Models;

namespace TP10.ViewModels
{
    public class TareaViewModel
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string Color { get; set; }
        public EstadoTarea Estado { get; set; }
        public string UsuarioAsignado { get; set; }
        public string TableroAsignado { get; set; }
        public int TableroAsignadoId { get; set; }
        public string UsuPropTableroAsignado { get; set; }
        

        public TareaViewModel() { }

        public TareaViewModel(int id, string nombre, string descripcion, string color, EstadoTarea estado, string usuarioAsignado, string tableroAsignado, int tableroAsignadoId, string usuPropTableroAsignado)
        {
            Id = id;
            Nombre = nombre;
            Descripcion = descripcion;
            Color = color;
            Estado = estado;
            UsuarioAsignado = usuarioAsignado;
            TableroAsignado = tableroAsignado;
            TableroAsignadoId = tableroAsignadoId;
            UsuPropTableroAsignado = usuPropTableroAsignado;
            
        }
    }
}

