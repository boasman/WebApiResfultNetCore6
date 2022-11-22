using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApiAutores.DTOs;

namespace WebApiAutores.Controllers
{

    [ApiController]
    [Route("api")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class RootController : ControllerBase
    {
        private readonly IAuthorizationService authorization;

        public RootController(IAuthorizationService authorization)
        {
            this.authorization = authorization;
        }

        [HttpGet(Name ="ObtenerRoot")]
        [AllowAnonymous]
        public async Task<ActionResult<List<DtoHATEOS>>> Get()
        {

            var datosHatero = new List<DtoHATEOS>();

            var esAdmin = await authorization.AuthorizeAsync(User, "esAdmin");

            datosHatero.Add(new DtoHATEOS(enlace: Url.Link("ObtenerRoot", new {}), description: "self",
                metodo: "GET"));

            datosHatero.Add(new DtoHATEOS(enlace: Url.Link("obtenerAutores", new { }), description: "autores",
                metodo: "GET"));

            if (esAdmin.Succeeded)
            {
                datosHatero.Add(new DtoHATEOS(enlace: Url.Link("crearAutor", new { }), description: "crear-autor",
                    metodo: "POST"));

                datosHatero.Add(new DtoHATEOS(enlace: Url.Link("crearLibro", new { }), description: "crear-libro",
                    metodo: "POST"));
            }



            return datosHatero;
        }
    }
}
