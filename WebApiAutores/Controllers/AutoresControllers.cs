using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using WebApiAutores.Entidades;

namespace WebApiAutores.Controllers
{
    [ApiController]
    [Route("api/autores")]
    public class AutoresControllers : ControllerBase
    {
        private readonly ApplicationDBContext context;

        public AutoresControllers(ApplicationDBContext context)
        {
            this.context = context;
        }


        [HttpGet]
        [HttpGet("listado")] //api/autores/listado
        [HttpGet("/listado")] //listado
        public async Task<ActionResult<List<Autor>>> Get()
        {

            return await  context.Autores.Include(x=>x.Libros).ToListAsync();

        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Autor>> Get([FromRoute]int id)
        {
            var autor = await context.Autores.FirstOrDefaultAsync(x => x.Id == id);

            if (autor == null)
            {
                return NotFound();
            }

            return autor;
        }

        [HttpGet("{nombre}")]
        public async Task<ActionResult<Autor>> Get(string nombre)
        {
            var autor = await context.Autores.FirstOrDefaultAsync(x => x.Nombre.Contains(nombre));

            if (autor == null)
            {
                return NotFound();
            }

            return autor;
        }

        [HttpGet("primero")]
        public async Task<ActionResult<Autor>> PrimerAutor()
        {

            return await context.Autores.FirstOrDefaultAsync();

        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody]Autor autor)
        {
            context.Add(autor);
            await context.SaveChangesAsync();   
            return Ok();
        }

        [HttpPut("{id:int}")] //api/autores/1 o 2 etc....

        public async Task<ActionResult> Put(Autor autor, int id)
        {

            if(autor.Id != id)
            {

                return BadRequest("El id del autor no coincide con el id de la url");
            }

            var existe = await context.Autores.AnyAsync(x => x.Id == id);

            if (!existe)
            {
                return NotFound();
            }

            context.Update(autor);
            await context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Borrar(int id)
        {

            var existe = await context.Autores.AnyAsync(x => x.Id == id);

            if (!existe)
            {
                return NotFound();
            }

            context.Remove(new Autor() { Id =  id});
            await context.SaveChangesAsync();
            return Ok();

           
        }

    }
}
