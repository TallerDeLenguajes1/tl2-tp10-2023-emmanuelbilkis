using Kanban.Models;

namespace TP10.ViewModels
{
    public class CrearTableroViewModel
    {
        public int IdUsuarioPropietario { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }

        public CrearTableroViewModel(Tablero tab) 
        {
            IdUsuarioPropietario = tab.IdUsuarioPropietario;
            Nombre = tab.Nombre;
            Descripcion = tab.Descripcion;
        }
    }
}
