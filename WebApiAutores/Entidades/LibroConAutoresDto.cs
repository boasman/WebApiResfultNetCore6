using WebApiAutores.DTOs;

namespace WebApiAutores.Entidades
{
    public class LibroConAutoresDto : LibroDTO
    {
        public List<AutoresDto> Autores { get; set; }
    }
}
