using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
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

        [HttpGet("{id:int}", Name ="obtenerLibro")]

        public async Task<ActionResult<LibroConAutoresDto>> Get(int id)
        {

            var libro = await context.Libros
                .Include(libroDb=>libroDb.AutoresLibros)
                .ThenInclude(autorLibroDB=>autorLibroDB.Autor).FirstOrDefaultAsync(libroDb => libroDb.Id == id);
            
            if(libro == null)
            {
                return NotFound("Libro no encontrado");
            }


            libro.AutoresLibros = libro.AutoresLibros.OrderBy(x => x.Orden).ToList();

            var result = mapper.Map<LibroConAutoresDto>(libro);

            return result;

        }

        [HttpPost(Name ="crearLibro")]
        public async Task<ActionResult> Post(LibroCreacionDTOS libroCreacionDtos)
        {

            if(libroCreacionDtos.AutoresIds == null)
            {
                return BadRequest("No se puede crear un libro sin autoes");
            }
            var autoresIds = await context.Autores.Where(autorBD => libroCreacionDtos.AutoresIds.Contains(autorBD.Id)).
                Select(x => x.Id).ToListAsync();

            if(libroCreacionDtos.AutoresIds.Count() != autoresIds.Count())
            {
                return BadRequest("No existe uno de los autores enviados");
            }

            var libro = mapper.Map<Libro>(libroCreacionDtos);

            asignarOrden(libro);
            
            context.Add(libro);

            await context.SaveChangesAsync();

            var resultado = mapper.Map<LibroDTO>(libro);

            return CreatedAtRoute("obtenerLibro", new { id = libro.Id }, resultado);

        }

        //[HttpPut("{id:int}")]
        //public async Task<ActionResult> Put(Libro libro, int id)
        //{

        //    if(libro.Id != id)
        //    {
        //        return BadRequest("El id del libro no coincide con el de la url");
        //    }

        //    var existe = await context.Libros.AnyAsync(x=>x.Id == id);

        //    if (!existe)
        //    {
        //        return NotFound("El id no se encuentra en la base de datos");
        //    }

        //    context.Update(libro);
        //    await context.SaveChangesAsync();
        //    return Ok("Se ha actualizado en registro");
        //}

        [HttpPut("{id:int}", Name ="actualizarLibro")]
        
        public async Task<ActionResult> Put(int id, LibroCreacionDTOS libroCreacionDTOS)
        {

            var libroDB = await context.Libros
                .Include(x => x.AutoresLibros)
                .FirstOrDefaultAsync(x => x.Id == id);

            if(libroDB == null)
            {
                return NotFound();
            }

            libroDB = mapper.Map(libroCreacionDTOS, libroDB);

            asignarOrden(libroDB);

            await context.SaveChangesAsync();

            return NotFound();
        }

        [HttpDelete(Name = "borrarLibro")]
        public async Task<ActionResult> Delete (int id)
        {
            var existe = await context.Libros.AnyAsync(x=>x.Id == id);

            if (!existe)
            {
                return NotFound("El id no se encuentra en la base de datos");
            }

            context.Remove(new Libro { Id = id });
            await context.SaveChangesAsync();
            return Ok("Libro Eliminado");
        }

        private void asignarOrden(Libro libro)
        {

            if (libro.AutoresLibros != null)
            {

                for (int i = 0; i < libro.AutoresLibros.Count; i++)
                {
                    libro.AutoresLibros[i].Orden = i;
                }

            }
        }

        [HttpPatch("{id:int}", Name ="actualizarLibro")]

        public async Task<ActionResult> Path(int id, JsonPatchDocument<LibroPathDto> patchDocument)
        {

            if(patchDocument == null)
            {
                return BadRequest();
            }

            var libroDb = await context.Libros.FirstOrDefaultAsync(x => x.Id == id);

            if(libroDb == null)
            {
                return NotFound();
            }

            var libroDto = mapper.Map<LibroPathDto>(libroDb); 
            
            patchDocument.ApplyTo(libroDto, ModelState);

            var esValido = TryValidateModel(libroDto);

            if (!esValido)
            {
                return BadRequest(ModelState);
            }

            mapper.Map(libroDto, libroDb);

            await context.SaveChangesAsync();

            return NoContent();


        }
    }
}
