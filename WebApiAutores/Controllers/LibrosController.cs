using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiAutores.Entidades;

namespace WebApiAutores.Controllers
{
    [Route("api/libros")]
    [ApiController]
    public class LibrosController : ControllerBase
    {
        private readonly ApplicationDbContext context;

        public LibrosController(ApplicationDbContext context)
        {
            this.context = context;
        }

        [HttpGet("{id:int}")]

        public async Task<ActionResult<Libro>> Get(int id)
        {
            return await context.Libros.Include(x=>x.Autor).FirstOrDefaultAsync(x => x.Id == id);

        }

        [HttpPost]
        public async Task<ActionResult> Post(Libro libro)
        {
            var existeAutor = await context.Autores.AnyAsync(x=>x.Id == libro.AutorId);

            if (!existeAutor)
            {
                return BadRequest($"No existe el autor de Id: {libro.AutorId}");
            }
            context.Add(libro);
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
