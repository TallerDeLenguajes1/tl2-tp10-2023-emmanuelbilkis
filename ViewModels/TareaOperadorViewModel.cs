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


        public TareaOperadorViewModel(int id, string nombre, string desc, string color, EstadoTarea estado, string usuAsignado, int usuAsignadoId, string tabAsignado,int tabAsig, int usuarioConectado)
        {
            Id = id;
            Nombre = nombre;
            Descripcion = desc;
            Color = color;
            Estado = estado;
            UsuarioAsignado = usuAsignado;
            UsuarioAsignadoId = usuAsignadoId;
            TableroAsignado = tabAsignado;
            UsuPropTableroAsignadoId = tabAsig;
            UsuarioConectado = usuarioConectado;    
        }

        public TareaOperadorViewModel() { }
    }
}
