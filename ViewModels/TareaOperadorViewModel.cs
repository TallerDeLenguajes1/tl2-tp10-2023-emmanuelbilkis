using Kanban.Models;
using TP10.Models;

namespace TP10.ViewModels
{
    public class TareaOperadorViewModel
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string Color { get; set; }
        public EstadoTarea Estado { get; set; }
        public string UsuarioAsignado { get; set; }
        public int UsuarioAsignadoId { get; set; }
        public string TableroAsignado { get; set; }
        public int UsuPropTableroAsignadoId { get; set; }
        public int UsuarioConectado { get; set; }
        public int TableroID { get; set; }
        public string UsuPropTableroAsignado { get; set; }
        public bool CreadaPorMi { get; set; }


        public TareaOperadorViewModel() { }

        public TareaOperadorViewModel(int id, string nombre, string descripcion, string color, EstadoTarea estado, string usuarioAsignado, int usuarioAsignadoId, string tableroAsignado, int usuPropTableroAsignadoId, int usuarioConectado, int tableroID, string usuPropTableroAsignado)
        {
            Id = id;
            Nombre = nombre;
            Descripcion = descripcion;
            Color = color;
            Estado = estado;
            UsuarioAsignado = usuarioAsignado;
            UsuarioAsignadoId = usuarioAsignadoId;
            TableroAsignado = tableroAsignado;
            UsuPropTableroAsignadoId = usuPropTableroAsignadoId;
            UsuarioConectado = usuarioConectado;
            TableroID = tableroID;
            UsuPropTableroAsignado = usuPropTableroAsignado;
        }
    }
}
