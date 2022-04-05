using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiAutores.DTOs;
using WebApiAutores.Entidades;
using WebApiAutores.Filtros;
using WebApiAutores.Servicios;

namespace WebApiAutores.Controllers
{
    [ApiController]
    //[Route("api/autores")] //api/autores => ruta
    [Route("api/autores")] //api/autores => ruta
    public class AutoresController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public AutoresController(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet] //api/autores
        //[HttpGet("listado")] //api/autores/listado
        //[HttpGet("/listado")] //listado       
        //[ServiceFilter(typeof(MiFiltroDeAccion))]
        public async Task<ActionResult<List<AutoresDto>>> Get()
        {            

            
            var autores =  await context.Autores.ToListAsync();

            var result = mapper.Map<List<AutoresDto>>(autores);

            return result;
        }       

        

        [HttpGet("{id:int}")]
        public async Task<ActionResult<AutoresDto>> Get(int id )
        {
            var autor = await context.Autores.FirstOrDefaultAsync(autorBD => autorBD.Id == id);

            if (autor == null)
            {
                return NotFound();
            }           


            return mapper.Map<AutoresDto>(autor); ;
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

        //[HttpGet("maria")]
        //public async Task<ActionResult<Autor>> primerAutor([FromHeader]int miValor)
        //{

        //    return await context.Autores.FirstOrDefaultAsync();
        //}

    

        [HttpGet("{nombre}")]
        public async Task<ActionResult<List<AutoresDto>>> Get(string nombre)
        {
            var autores = await context.Autores.Where(autorBD => autorBD.Nombre.Contains(nombre)).ToListAsync();

            if (autores == null)
            {

                return NotFound();
            }

            return mapper.Map<List<AutoresDto>>(autores);
        }
        

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] AutorCreacionDTOS autorCreacion)
        {

            var existeAutorConElMismoNombre = await context.Autores.AnyAsync(x => x.Nombre == autorCreacion.Nombre);

            if (existeAutorConElMismoNombre)
            {
                return BadRequest($"Ya existe un autor con el nombre {autorCreacion.Nombre}");
            }


            var autor = mapper.Map<Autor>(autorCreacion);

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
