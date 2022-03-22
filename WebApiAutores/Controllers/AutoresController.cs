using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiAutores.Entidades;
using WebApiAutores.Servicios;

namespace WebApiAutores.Controllers
{
    [ApiController]
    //[Route("api/autores")] //api/autores => ruta
    [Route("api/[controller]")] //api/autores => ruta
    public class AutoresController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IServicio Servicio;
        private readonly ServicioSinleton servicioSinleton;
        private readonly ServicioTransient servicioTransient;
        private readonly ServicioScoped servicioScoped;
        

        public AutoresController(ApplicationDbContext context, IServicio servicio, ServicioSinleton servicioSinleton, 
            ServicioTransient servicioTransient, ServicioScoped servicioScoped)
        {
            this.context = context;
            this.Servicio = servicio;            
            this.servicioSinleton = servicioSinleton;
            this.servicioTransient = servicioTransient;
            this.servicioScoped = servicioScoped;
        }

        [HttpGet] //api/autores
        [HttpGet("listado")] //api/autores/listado
        [HttpGet("/listado")] //listado
        public async Task<ActionResult<List<Autor>>> Get()
        {
            Servicio.RealizarTarea();
            return await context.Autores.Include(x=>x.Libros).ToListAsync();
        }

        [HttpGet("GUID")]
        public ActionResult ObtenerGui()
        {
            return Ok(new
            {

                AutoresControllerTransient = servicioTransient.Guid,
                ServicioA_Transient = Servicio.ObtenerTransient(),
                AutoresControllerScoped = servicioScoped.Guid,
                ServicioA_Scoped = Servicio.ObtenerScoped(),
                AutoresControllerSingleton = servicioSinleton.Guid,
                ServicioA_Singleton = Servicio.ObtenerSingleton()
                
                


            }) ;
            
               

        }

        [HttpGet("primero")] //api/autores/primero => ruta
        public async Task<ActionResult<Autor>> PrimerAutor()
        {

            return await context.Autores.FirstOrDefaultAsync();
        }

        [HttpGet("{id:int}/{param2=Michael}")]
        public IActionResult Get(int id, string param2)
        {
            var autor = context.Autores.FirstOrDefault(x => x.Id == id);

            if (autor == null)
            {

                return NotFound();
            }

            return Ok(autor);
        }

         /* Hay tres forma de devolver tipo de datos en una api 
          
          1- ActionResolt<T>
          2- El tipo de datos que quiere devolver en este caso Autor
          3- y IActionResult 
         
          */
        //[HttpGet("{id:int}/{param2=Michael}")]
        //public IActionResult  Get(int id, string param2)
        //{
        //    var autor =  context.Autores.FirstOrDefault(x => x.Id == id);

        //    if (autor == null)
        //    {

        //        return NotFound();
        //    }

        //    return Ok(autor);
        //}

        [HttpGet("maria")]
        public async Task<ActionResult<Autor>> primerAutor([FromHeader]int miValor)
        {

            return await context.Autores.FirstOrDefaultAsync();
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
        

        [HttpPost]
        public async Task<ActionResult> Post(Autor autor)
        {
            context.Add(autor);
            await context.SaveChangesAsync();
            return Ok();

        }
        


        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(Autor autor, int id)
        {

            if (autor.Id != id)
            {
                return BadRequest("El id no coincide con el id de la url");
            }

            var existe = await context.Autores.AnyAsync(x => x.Id == id);

            if (!existe)
            {
                return NotFound("El autor no ha sido encontrado");
            }

            context.Update(autor);
            await context.SaveChangesAsync();
            return Ok("Autor actualizado");
        }

        [HttpDelete]
        public async Task<ActionResult> Delete(int id)
        {

            var existe = await context.Autores.AnyAsync(x => x.Id == id);

            if (!existe)
            {
                return NotFound("El autor no ha sido encontrado");
            }

            context.Remove(new Autor() { Id = id });
            await context.SaveChangesAsync();
            return Ok("Autor eliminado");

        }

    
    }
}
