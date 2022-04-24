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

            CreateMap<Autor, AutoresDto>();
            CreateMap<Autor, AutorConLibroDto>()
                .ForMember(autorDTo => autorDTo.Libros, opciones => opciones.MapFrom(MapAutorDtoLibro));

            CreateMap<LibroCreacionDTOS, Libro>()
                .ForMember(libro => libro.AutoresLibros, opciones => opciones.MapFrom(MapAutoreslibros));

            CreateMap<Libro, LibroDTO>();
            CreateMap<LibroPathDto, Libro>().ReverseMap();
            CreateMap<Libro, LibroConAutoresDto>()
                .ForMember(libroDto => libroDto.Autores, opciones => opciones.MapFrom(MapLibroDtoAutores));

            CreateMap<ComentarioCreacionDTOS, Comentario>();
            CreateMap<Comentario, ComentarioDTOS>();
            
        }

        private List<LibroDTO> MapAutorDtoLibro(Autor autor, AutoresDto autoresDto)
        {

            var resultado = new List<LibroDTO>();

            if(autor.AutoresLibros == null) { return resultado; }


            foreach (var autorLibro in autor.AutoresLibros)
            {

                resultado.Add(new LibroDTO()
                {
                    Id = autorLibro.LibroId,
                    Titulo = autorLibro.Libro.Titulo

                });
            }


            return resultado;
        }

        private List<AutoresDto> MapLibroDtoAutores(Libro libro, LibroDTO libroDTO)
        {

            var resultado = new List<AutoresDto>();

            if(libro.AutoresLibros == null) { return resultado; }

            foreach (var autorLibro in libro.AutoresLibros)
            {

                resultado.Add(new AutoresDto()
                {
                    Id = autorLibro.AutorId,
                    Nombre = autorLibro.Autor.Nombre
                });

            }

            return resultado;
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
