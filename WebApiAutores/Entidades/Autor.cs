using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebApiAutores.Validaciones;

namespace WebApiAutores.Entidades
{
    public class Autor 
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "el campo {0} es obligatorio")]
        [StringLength(maximumLength: 5, ErrorMessage ="El campo {0} no debe de tener mas de {1}, caracteres")]
        //[PrimeraLetraMayusculaAttribute]
        public string Nombre { get; set; }
        public List<Libro> Libros { get; set; }

        //[Range(18,120)]
        //[NotMapped]
        //public int edad { get; set; }

        [NotMapped]
        public int Menor { get; set; }

        [NotMapped]
        public int Mayor { get; set; }

        //public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        //{
        //    if (!string.IsNullOrEmpty(Nombre))
        //    {
        //        var primeraletra = Nombre[0].ToString();

        //        if(primeraletra != primeraletra.ToUpper())
        //        {
        //            yield return new ValidationResult("La primera letra debe ser mayuscula", 
                        
        //                new string[] { nameof(Nombre) }); 
                        
        //        }

        //    }

        //    if (Menor > Mayor)
        //    {

        //        yield return new ValidationResult("Este valor no puede ser mas grande que el campo mayor",
        //            new string[] { nameof(Menor) });
        //    }
        //}
    }
}
