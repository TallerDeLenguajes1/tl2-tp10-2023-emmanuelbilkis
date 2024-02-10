using TP10.ViewModels;
using TP10.Models;

    namespace Kanban.Models
    {
        public class Tarea
        {
            public Tarea(int id, int idTablero, string nombre, string descripcion, string color, int estado, int idUsuarioAsignado)
            {
                Id = id;
                IdTablero = idTablero;
                Nombre = nombre;
                Descripcion = descripcion;
                Color = color;
                Estado = (EstadoTarea)estado;
                IdUsuarioAsignado = idUsuarioAsignado;
            }

            public Tarea() { }

            public Tarea(ModificarTareaViewModel tareaModificada)
            {
                Id = tareaModificada.Id;
                IdTablero = tareaModificada.IdTablero;
                Nombre = tareaModificada.Nombre;
                Descripcion = tareaModificada.Descripcion;
                Color = tareaModificada.Color;
                Estado = tareaModificada.Estado;
                IdUsuarioAsignado = tareaModificada.IdUsuarioAsignado;
            }

            public Tarea(CrearTareaViewModel tarea)
            {
                IdTablero = tarea.IdTablero;
                Nombre = tarea.Nombre;
                Descripcion = tarea.Descripcion;
                Color = tarea.Color;
                Estado = tarea.Estado;
                IdUsuarioAsignado = tarea.IdUsuarioAsignado;
            }

            public int Id { get; set; }
            public int IdTablero { get; set; }
            public int IdUsuarioAsignado { get; set; }
            public string Nombre { get; set; }
            public string Descripcion { get; set; }
            public string Color { get; set; }
            public EstadoTarea Estado { get; set; }  
            
        }
    }

