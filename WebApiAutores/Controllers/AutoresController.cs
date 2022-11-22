using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiAutores.DTOs;
using WebApiAutores.Entidades;
using WebApiAutores.Filtros;


namespace WebApiAutores.Controllers
{
    [ApiController]
    //[Route("api/autores")] //api/autores => ruta
    [Route("api/autores")] //api/autores => ruta
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "EsAdmin")]//Protegiendo el controllador
    public class AutoresController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        private readonly IConfiguration configuration;
        private readonly IAuthorizationService authorizationService;

        public AutoresController(ApplicationDbContext context, IMapper mapper, IConfiguration configuration,
            IAuthorizationService authorizationService)
        {
            this.context = context;
            this.mapper = mapper;
            this.configuration = configuration;
            this.authorizationService = authorizationService;
        }

        //[HttpGet()] //api/autores
        //[HttpGet("listado")] //api/autores/listado
        //[HttpGet("/listado")] //listado       
        //[ServiceFilter(typeof(MiFiltroDeAccion))]

        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]//Protegiendo el endpoint
        [AllowAnonymous]
        [HttpGet(Name = "obtenerAutores")]
        public async Task<ColeccionDeRecursos<AutoresDto>> Get()
        {


            var autores = await context.Autores.ToListAsync();

            var dtos = mapper.Map<List<AutoresDto>>(autores);

            var esAdmin = await authorizationService.AuthorizeAsync(User, "esAdmin");

            var resultado = new ColeccionDeRecursos<AutoresDto> { Valores = dtos };

            resultado.Enlaces.Add(new DtoHATEOS(
                enlace: Url.Link("obtenerAutores", new { }),
                description: "self",
                metodo: "GET"));

            if (esAdmin.Succeeded)
            {
                resultado.Enlaces.Add(new DtoHATEOS(
                enlace: Url.Link("crearAutor", new { }),
                description: "crear-autor",
                metodo: "POST"));
            }



            dtos.ForEach(dto => GenerarEnlace(dto, esAdmin.Succeeded));


            return resultado;
        }

        [HttpGet("Configuraciones")]
        public ActionResult<string> obtenerApellido()
        {
            var ambiente = configuration["apellido"];
            return ambiente;
            //return configuration.GetConnectionString("defaultConnection");
            //return  configuration["ConnectionStrings:defaultConnection"];
        }



        [HttpGet("{id:int}", Name = "obtenerAutor")]
        [AllowAnonymous]
        public async Task<ActionResult<AutorConLibroDto>> Get(int id)
        {
            var autor = await context.Autores
                .Include(AutorDb => AutorDb.AutoresLibros)
                .ThenInclude(autorlibro => autorlibro.Libro)
                .FirstOrDefaultAsync(autorBD => autorBD.Id == id);

            if (autor == null)
            {
                return NotFound();
            }


            var dto = mapper.Map<AutorConLibroDto>(autor);

            var esAdmin = await authorizationService.AuthorizeAsync(User, "esAdmin");

            GenerarEnlace(dto,esAdmin.Succeeded);

            return dto;
        }

        private void GenerarEnlace(AutoresDto autoreDto, bool esAdmin)
        {
            autoreDto.Enlaces.Add(new DtoHATEOS(
                enlace: Url.Link("obtenerAutor", new { id = autoreDto.Id }),
                description: "self",
                metodo: "GET"));

            if (esAdmin)
            {
                autoreDto.Enlaces.Add(new DtoHATEOS(
               enlace: Url.Link("actualizarAutor", new { id = autoreDto.Id }),
               description: "atualizar-autor",
               metodo: "PUT"));

                autoreDto.Enlaces.Add(new DtoHATEOS(
                    enlace: Url.Link("borrarAutor", new { id = autoreDto.Id }),
                    description: "borrar-autor",
                    metodo: "DELETE"));
            }


        }



        [HttpGet("{nombre}", Name = "obtenerAutoresPorNombre")]
        public async Task<ActionResult<List<AutoresDto>>> Get(string nombre)
        {
            var autores = await context.Autores.Where(autorBD => autorBD.Nombre.Contains(nombre)).ToListAsync();

            if (autores == null)
            {

                return NotFound();
            }

            return mapper.Map<List<AutoresDto>>(autores);
        }


        [HttpPost(Name = "crearAutor")]
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

            var autorDto = mapper.Map<AutoresDto>(autor);

            return CreatedAtRoute("obtenerAutor", new { Id = autor.Id }, autorDto);

        }



        [HttpPut("{id:int}", Name = "actualizarAutor")]
        public async Task<ActionResult> Put(AutorCreacionDTOS autorCreacionDTOS, int id)
        {

            var existe = await context.Autores.AnyAsync(x => x.Id == id);

            if (!existe)
            {
                return NotFound("El autor no ha sido encontrado");
            }

            var autor = mapper.Map<Autor>(autorCreacionDTOS);
            autor.Id = id;

            context.Update(autor);
            await context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete(Name = "borrarAutor")]
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


    }
}
