using System.ComponentModel.DataAnnotations;
using WebApiAutores.Validaciones;

namespace WebApiAutores.DTOs
{
    public class LibroPathDto
    {

        [PrimeraLetraMayuscula]
        [MaxLength(250)]
        public string Titulo { get; set; }
        
        public DateTime? FechaPublicacion { get; set; }
    }
}
