using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ReltechApi.CustomValidation;

#nullable disable

namespace ReltechApi.Models
{
    public partial class Usuarios
    {
        [Key]
        public int Id { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Nombre requerido")]
        [StringLength(200, MinimumLength = 3, ErrorMessage = "El nombre debe contener enre 3 y 200 caracteres")]
        [RegularExpression("([a-z A-Z]+)", ErrorMessage = "El nombre contiene caracteres no permitidos")]
        public string NombreCompleto { get; set; }

        [StringLength(8, MinimumLength = 6, ErrorMessage = "La cédula debe contener entre 6 y 8 caracteres")]
        [RegularExpression("([0-9]+)", ErrorMessage = "La cédula debe incluir solamente números")]
        public string Cedula { get; set; }

        [ValidBirthDate("1900-01-01", ErrorMessage = "La fecha no es válida")]
        public DateTime? FechaNacimiento { get; set; }

        [EmailAddress(ErrorMessage = "Dirección de correo electrónico no válida")]
        public string CorreoElectronico { get; set; }
        
        [Phone(ErrorMessage = "Número de teléfono no válido")]
        public string Telefono { get; set; }

        [Required(ErrorMessage = "Organización requerida")]
        public int? OrganizacionId { get; set; }
    }
}
