using Kanban.Models;
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
            Tableros = new List<Tablero>(); 
            Usuarios = new List<Usuario>(); 
        }

        public CrearTareaViewModel(List<Tablero> tableros, List<Usuario> usuarios)
        {
            Tableros = tableros;
            Usuarios = usuarios;
        }

        public CrearTareaViewModel(List<Usuario> usuarios, Tablero tab)
        {
            Tableros = new List<Tablero>();
            this.Tableros.Add(tab);
            this.IdTablero = tab.Id;
            Usuarios = usuarios;
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

        [Required(ErrorMessage = "Se requiere el campo nombre.")]
        public EstadoTarea Estado { get; set; }

        [Required(ErrorMessage = "Número de id requerido")]
        [Range(0, int.MaxValue, ErrorMessage = "El valor de {0} debe ser mayor o igual que cero.")]
        public int IdUsuarioAsignado { get; set; }
        public List<Usuario> Usuarios { get; set; }
        public List<Tablero> Tableros { get; set; }
    }
}

