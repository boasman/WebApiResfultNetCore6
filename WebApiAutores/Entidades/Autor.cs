using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebApiAutores.Validaciones;

namespace WebApiAutores.Entidades
{
    public class Autor 
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "el campo {0} es obligatorio")]
        [StringLength(maximumLength: 250, ErrorMessage ="El campo {0} no debe de tener mas de {1}, caracteres")]
        [PrimeraLetraMayusculaAttribute]
        public string Nombre { get; set; } 
        
    }
}
