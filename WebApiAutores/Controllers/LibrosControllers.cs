using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiAutores.Entidades;

namespace WebApiAutores.Controllers
{
    [ApiController()]
    [Route("api/libros")]
    public class LibrosControllers : ControllerBase
    {
        private readonly ApplicationDBContext context;

        public LibrosControllers(ApplicationDBContext context)
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

            var existeautor = await context.Autores.AnyAsync(x => x.Id == libro.AutorId);


            if (!existeautor)
            {
                return BadRequest($" No existe el autor de Id:{libro.AutorId} ");
            }
            context.Add(libro);
            await context.SaveChangesAsync();
            return Ok();

        }
    }
}
