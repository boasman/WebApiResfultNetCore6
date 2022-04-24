using WebApiAutores.DTOs;

namespace WebApiAutores.Entidades
{
    public class AutorConLibroDto : AutoresDto
    {
        public List<LibroDTO> Libros { get; set; }
    }
}
