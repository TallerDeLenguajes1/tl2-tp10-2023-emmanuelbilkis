﻿using Kanban.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TP10.ViewModels
{
    public class CrearTableroViewModel
    {
        [Required(ErrorMessage = "El campo es obligatorio.")]
        [Display(Name = "Id del Usuario Propietario")]
        [Range(0,int.MaxValue,ErrorMessage ="El valor debe ser mayor o igual q cero.")]
        public int IdUsuarioPropietario { get; set; }

        [Required(ErrorMessage = "El campo Nombre es obligatorio.")]
        public string Nombre { get; set; }

        [ValidateNever]
        public string Descripcion { get; set; }

        public CrearTableroViewModel(Tablero tab)
        {
            IdUsuarioPropietario = tab.IdUsuarioPropietario;
            Nombre = tab.Nombre;
            if (string.IsNullOrEmpty(tab.Descripcion))
            {
                Descripcion = "Sin descripción";
            }
            Descripcion = tab.Descripcion;
        }

        public CrearTableroViewModel()
        {
        }
    }
}
