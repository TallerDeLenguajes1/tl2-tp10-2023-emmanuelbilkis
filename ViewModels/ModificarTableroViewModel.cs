using Kanban.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace TP10.ViewModels
{
    public class ModificarTableroViewModel
    {
        [Required(ErrorMessage = "El campo es obligatorio.")]
        public int Id { get; set; }

        [Display(Name = "Id del Usuario Propietario")]
        [Required(ErrorMessage = "El campo es obligatorio.")]
        [Range(0,int.MaxValue)]
        public int IdUsuarioPropietario { get; set; }

        [Required(ErrorMessage = "El campo es obligatorio.")]
        public string Nombre { get; set; }

        [ValidateNever]
        public string Descripcion { get; set; }

        public ModificarTableroViewModel(Tablero tab)
        {
            Id = tab.Id;    
            IdUsuarioPropietario = tab.IdUsuarioPropietario;
            Descripcion = tab.Descripcion;
            Nombre = tab.Nombre;
        }

        public ModificarTableroViewModel()
        {
        }
    }
}
