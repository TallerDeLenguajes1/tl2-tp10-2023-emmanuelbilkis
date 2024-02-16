using Kanban.Models;
using TP10.Models;

namespace TP10.ViewModels
{
    public class TareaViewModel
    {
        public int IdUsuarioConectado { get; set; }
        public int UsuarioTableroAsignadoId { get; set; }
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string Color { get; set; }
        public EstadoTarea Estado { get; set; }  
        public string UsuarioAsignado { get; set; }
        public string RolUsu { get; set; }
        public string TableroAsignado { get; set; }
        public string TableroDesc { get; set; }
        public string RolUsuarioConectado  { get; set; }

        public TareaViewModel(int id,string nombre,string desc,string color,EstadoTarea estado,string usu,string rol,string tab,string descTab,int usuarioTableroAsignadoId,int usuarioConectado, string rolUsuarioConectado)
        {
            Id = id;
            Nombre = nombre;
            Descripcion = desc;
            Color = color;
            Estado = estado;
            UsuarioAsignado = usu;
            RolUsu = rol;
            TableroAsignado = tab;
            TableroDesc = descTab;
            IdUsuarioConectado = usuarioConectado;
            UsuarioTableroAsignadoId = usuarioTableroAsignadoId;
            RolUsuarioConectado = rolUsuarioConectado;
        }

        public TareaViewModel() { }
    }
}

