using TP10.ViewModels;

namespace Kanban.Models
{
    public class Tablero
    {
        public Tablero(int id, int idUsuarioPropietario, string nombre, string descripcion)
        {
            Id = id;
            IdUsuarioPropietario = idUsuarioPropietario;
            Nombre = nombre;
            Descripcion = descripcion;
        }

        public Tablero (ModificarTableroViewModel tab) 
        {
            IdUsuarioPropietario = tab.IdUsuarioPropietario;
            Nombre= tab.Nombre;
            Descripcion= tab.Descripcion;
        }

        public Tablero(CrearTableroViewModel tab)
        {
            IdUsuarioPropietario = tab.IdUsuarioPropietario;
            Nombre = tab.Nombre;
            Descripcion = tab.Descripcion;
        }

        public Tablero() { }
        public int Id { get; set; }
        public int IdUsuarioPropietario { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
    }
}
