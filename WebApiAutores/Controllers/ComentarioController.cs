using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiAutores.DTOs;
using WebApiAutores.Entidades;

namespace WebApiAutores.Controllers
{
    [ApiController]
    [Route("api/libros/{LibroId:int}/comentarios")]
    public class ComentariosController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public ComentariosController(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet]       
        public async Task<List<ComentarioDTOS>> Get(int LibroId)
        {

            var comentario = await context.Comentarios.
                Where(x=>x.LibroId == LibroId).ToListAsync();

            return mapper.Map<List<ComentarioDTOS>>(comentario);


        }

        [HttpGet]
        [Route("{id:int}", Name = "obtenerComentario")]

        public async Task<ActionResult<ComentarioDTOS>> GetById(int id)
        {
            var comentario = await context.Comentarios.FirstOrDefaultAsync(x => x.Id == id);

            if(comentario == null)
            {
                return NotFound("El comentario no existe");
            }

            return mapper.Map<ComentarioDTOS>(comentario);

        }

        [HttpPost]
        public async Task<ActionResult> Post(int LibroId, ComentarioCreacionDTOS model)
        {

            var libro = await context.Libros.AnyAsync(x => x.Id == LibroId);

            if (!libro)
            {
                return NotFound("El libro no existe");
            }

            var comentario = mapper.Map<Comentario>(model);
            comentario.LibroId = LibroId;

            context.Add(comentario);
            context.SaveChanges();

            var comentarioDto = mapper.Map<ComentarioDTOS>(comentario);

            return CreatedAtRoute("obtenerComentario", new { id = comentario.Id, libroId = LibroId }, comentarioDto);

        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int LibroId, int id,ComentarioCreacionDTOS comentarioCreacion )
        {

            var existelibro = await context.Libros.AnyAsync(libroBD => libroBD.Id == LibroId);

            if (!existelibro)
            {
                return NotFound();
            }

            var existeComentario = await context.Comentarios.AnyAsync(comentarioBd => comentarioBd.Id == id);

            if (!existeComentario)
            {
                return NotFound();
            }

            var comentario = mapper.Map<Comentario>(comentarioCreacion);

            comentario.Id = id;
            comentario.LibroId = LibroId;
            context.Update(comentario);
            await context.SaveChangesAsync();

            return NoContent();

        }


    }
}
