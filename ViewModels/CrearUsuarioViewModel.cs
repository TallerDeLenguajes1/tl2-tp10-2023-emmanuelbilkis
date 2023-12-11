using Kanban.Models;
using System.ComponentModel.DataAnnotations;
using TP10.Models;

namespace TP10.ViewModels
{
    public class CrearUsuarioViewModel
    {
        [Required(ErrorMessage = "Campo requerido")]
        public string Nombre { get; set; }
        [Required(ErrorMessage = "Campo requerido")]
        [MinLength(8),MaxLength(15)]
        public string Contrasenia { get; set; }
        [Required(ErrorMessage = "Campo requerido")] // hacer el rol con un enum despues
        public string Rol { get; set; }

        public CrearUsuarioViewModel(Usuario usu)
        {
            Nombre = usu.Nombre;
            Contrasenia = usu.Contrasenia;
            Rol = usu.Rol;
        }

        public CrearUsuarioViewModel()
        {
            
        }
    }
}
