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

        public TareaViewModel(int id,string nombre,string desc,string color,EstadoTarea estado,string usuAsignado,string tabAsignado)
        {
            Id = id;
            Nombre = nombre;
            Descripcion = desc;
            Color = color;
            Estado = estado;
            UsuarioAsignado = usuAsignado;
            TableroAsignado = tabAsignado;     
        }

        public TareaViewModel() { }
    }
}

