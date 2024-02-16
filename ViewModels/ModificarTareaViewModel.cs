using Kanban.Models;
using System.ComponentModel.DataAnnotations;
using TP10.Models;

namespace TP10.ViewModels
{
    public class ModificarTareaViewModel
    {
        [Required(ErrorMessage = "Numero de id requerido.")]
        [Range(0, int.MaxValue, ErrorMessage = "El valor de {0} debe ser mayor o igual que cero.")]
        public int Id { get; set; } 

        [Required(ErrorMessage = "Numero de id requerido.")]
        [Range(0, int.MaxValue, ErrorMessage = "El valor de {0} debe ser mayor o igual que cero.")]
        public int IdTablero { get; set; }

        [Required(ErrorMessage = "Se requiere el campo nombre.")]
        public string Nombre { get; set; }

        public string Descripcion { get; set; }

        public string Color { get; set; }

        [Required(ErrorMessage ="Se debe ingresar un valor")]
        public EstadoTarea Estado { get; set; }

        [Required(ErrorMessage = "Numero de id requerido.")]
        [Range(0, int.MaxValue, ErrorMessage = "El valor de {0} debe ser mayor o igual que cero.")]
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
