using TP10.ViewModels;

namespace Kanban.Models
{
    public class Tablero
    {
        public Tablero() { }
        public int Id { get; set; }
        public int IdUsuarioPropietario { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }

        public Tablero(int id, int idUsuarioPropietario, string nombre, string descripcion)
        {
            Id = id;
            IdUsuarioPropietario = idUsuarioPropietario;
            Nombre = nombre;
            if (!string.IsNullOrEmpty(descripcion))
            {
                Descripcion = descripcion;
            }
            else
            {
                Descripcion = "Sin descripción";
            }
        }

        public Tablero (ModificarTableroViewModel tab) 
        {
            Id = tab.Id;
            IdUsuarioPropietario = tab.IdUsuarioPropietario;
            Nombre= tab.Nombre;
            if (!string.IsNullOrEmpty(tab.Descripcion))
            {
                Descripcion = tab.Descripcion;
            }
            else
            {
                Descripcion = "Sin descripción";
            }
        }

        public Tablero(CrearTableroViewModel tab)
        {
            IdUsuarioPropietario = tab.IdUsuarioPropietario;
            Nombre = tab.Nombre;
            if (!string.IsNullOrEmpty(tab.Descripcion))
            {
                Descripcion = tab.Descripcion;
            }
            else
            {
                Descripcion = "Sin descripción";
            }
        }
    }
}
