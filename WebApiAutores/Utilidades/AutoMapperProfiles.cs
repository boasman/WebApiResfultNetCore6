using AutoMapper;
using WebApiAutores.DTOs;
using WebApiAutores.Entidades;

namespace WebApiAutores.Utilidades
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<AutorCreacionDTOS, Autor>();
            CreateMap<Autor,AutoresDto>();
            CreateMap<LibroCreacionDTOS, Libro>()
                .ForMember(libro => libro.AutoresLibros, opciones => opciones.MapFrom(MapAutoreslibros));
            CreateMap<Libro, LibroDTO>();
            CreateMap<ComentarioCreacionDTOS, Comentario>();
            CreateMap<Comentario, ComentarioDTOS>();
            
        }

        private List<AutorLibro> MapAutoreslibros(LibroCreacionDTOS libroCreacionDTOS, Libro libro)
        {
            var resultado = new List<AutorLibro>();

             if(libroCreacionDTOS.AutoresIds == null) { return resultado; }

            foreach (var autorId in libroCreacionDTOS.AutoresIds)
            {
                resultado.Add(new AutorLibro() { AutorId = autorId });
            }

            return resultado;

            
        }
    }
}
