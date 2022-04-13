using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiAutores.DTOs;
using WebApiAutores.Entidades;

namespace WebApiAutores.Controllers
{
    [Route("api/libros")]
    [ApiController]
    public class LibrosController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public LibrosController(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet("{id:int}")]

        public async Task<ActionResult<LibroDTO>> Get(int id)
        {

            var libro = await context.Libros.FirstOrDefaultAsync(libroDb => libroDb.Id == id);
            
            if(libro == null)
            {
                return NotFound("Libro no encontrado");
            }

            var result = mapper.Map<LibroDTO>(libro);

            return result;

        }

        [HttpPost]
        public async Task<ActionResult> Post(LibroCreacionDTOS libroCreacionDtos)
        {

            if(libroCreacionDtos.AutoresIds == null)
            {
                BadRequest("No se puede crear un libro sin autoes");
            }
            var autoresIds = await context.Autores.Where(autorBD => libroCreacionDtos.AutoresIds.Contains(autorBD.Id)).
                Select(x => x.Id).ToListAsync();

            if(libroCreacionDtos.AutoresIds.Count() != autoresIds.Count())
            {
                return BadRequest("No existe uno de los autores enviados");
            }

            var result = mapper.Map<Libro>(libroCreacionDtos);

            context.Add(result);

            await context.SaveChangesAsync();
            
            return Ok("Se ha creado el libro");

        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(Libro libro, int id)
        {

            if(libro.Id != id)
            {
                return BadRequest("El id del libro no coincide con el de la url");
            }

            var existe = await context.Libros.AnyAsync(x=>x.Id == id);

            if (!existe)
            {
                return NotFound("El id no se encuentra en la base de datos");
            }

            context.Update(libro);
            await context.SaveChangesAsync();
            return Ok("Se ha actualizado en registro");
        }

        [HttpDelete]

        public async Task<ActionResult> Delete (int id)
        {
            var existe = await context.Libros.AnyAsync(x=>x.Id == id);

            if (!existe)
            {
                return NotFound("El id no se encuentra en la base de datos");
            }

            context.Remove(new Libro { Id = id });
            await context.SaveChangesAsync();
            return Ok("Registro Eliminado");
        }
    }
}
