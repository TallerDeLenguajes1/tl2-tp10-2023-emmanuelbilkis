using Kanban.Models;
using System.ComponentModel.DataAnnotations;

namespace TP10.ViewModels
{
    public class ModificarUsuarioViewModel
    {
        
        [Required(ErrorMessage = "Campo requerido")]
        public int Id { get; set; }
        [Required(ErrorMessage = "Campo requerido")]
        public string Nombre { get; set; }
        [Required(ErrorMessage = "Campo requerido")]
        [MinLength(8), MaxLength(15)]
        public string Contrasenia { get; set; }
        [Required(ErrorMessage = "Campo requerido")] // hacer el rol con un enum despues
        public string Rol { get; set; }

       
        public ModificarUsuarioViewModel(Usuario usu)
        {
            Id = usu.Id;
            Nombre = usu.Nombre;
            Contrasenia = usu.Contrasenia;
            Rol = usu.Rol;
                
        }

        public ModificarUsuarioViewModel()
        {
           
        }
    }
}
