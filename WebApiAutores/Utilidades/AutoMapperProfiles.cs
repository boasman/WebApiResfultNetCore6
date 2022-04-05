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
            CreateMap<LibroCreacionDTOS, Libro>();
            CreateMap<Libro, LibroCreacionDTOS>();
            
        }
    }
}
