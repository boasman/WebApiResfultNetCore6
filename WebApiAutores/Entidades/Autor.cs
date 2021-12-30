using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebApiAutores.Validaciones;

namespace WebApiAutores.Entidades
{
    public class Autor
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="El {0} es requerido")]
        [MaxLength(12)]
        public string Nombre { get; set; }

        [Url]
        [NotMapped]
        [PrimeraLetraMayuscula]
        public string  url { get; set; }
        [NotMapped]
        [CreditCard]
        public string  tarjetaCredito { get; set; }
        [NotMapped]
        [Range(18,120)]        
        public int edad { get; set; }
        public List<Libro> Libros { get; set; }
    }
}
