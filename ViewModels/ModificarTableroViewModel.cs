using Kanban.Models;

namespace TP10.ViewModels
{
    public class ModificarTableroViewModel
    {
        public int IdUsuarioPropietario { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }

        public ModificarTableroViewModel(Tablero tab)
        {
            IdUsuarioPropietario = tab.IdUsuarioPropietario;
            Descripcion = tab.Descripcion;
            Nombre = tab.Nombre;
        }
    }
}
