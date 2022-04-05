using System.ComponentModel.DataAnnotations;
using WebApiAutores.Validaciones;

namespace WebApiAutores.DTOs
{
    public class AutorCreacionDTOS
    {
        [Required(ErrorMessage = "el campo {0} es obligatorio")]
        [StringLength(maximumLength: 250, ErrorMessage = "El campo {0} no debe de tener mas de {1}, caracteres")]
        [PrimeraLetraMayusculaAttribute]
        public string  Nombre { get; set; }
    }
}
