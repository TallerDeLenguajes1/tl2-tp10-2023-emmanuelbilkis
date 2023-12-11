using System.ComponentModel.DataAnnotations;
using TP10.Models;

namespace TP10.ViewModels
{
    public class CrearTareaViewModel
    {
        public CrearTareaViewModel(int id, int idTablero, string nombre, string descripcion, string color, EstadoTarea estado, int idUsuarioAsignado)
        {
            Id = id;
            IdTablero = idTablero;
            Nombre = nombre;
            Descripcion = descripcion;
            Color = color;
            Estado = estado;
            IdUsuarioAsignado = idUsuarioAsignado;
        }

        public CrearTareaViewModel()
        {
            
        }

        [Required (ErrorMessage ="Numero de id requerido.")]
        [Range(0, int.MaxValue, ErrorMessage = "El valor de {0} debe ser mayor o igual que cero.")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Numero de id requerido.")]
        [Range(0, int.MaxValue, ErrorMessage = "El valor de {0} debe ser mayor o igual que cero.")]
        public int IdTablero { get; set; }

        [Required(ErrorMessage = "Se requiere el campo nombre.")]
        public string Nombre { get; set; }
        public string Descripcion { get; set; }

        public string Color { get; set; }

        [Range(0,4, ErrorMessage = "Se debe ingresar un valor entre 0 y 4 | 0 (Ideas),1(ToDo),2(Doing),3(Review),4(Done)")]
        public EstadoTarea Estado { get; set; }

        [Required(ErrorMessage = "Número de id requerido")]
        [Range(0, int.MaxValue, ErrorMessage = "El valor de {0} debe ser mayor o igual que cero.")]
        public int IdUsuarioAsignado { get; set; }
    }
}

