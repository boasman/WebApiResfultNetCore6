using Microsoft.AspNetCore.Mvc.Filters;

namespace WebApiAutores.Filtros
{
    public class MiFiltroDeAccion : IActionFilter
    {
        public ILogger<MiFiltroDeAccion> Logger { get; }

        public MiFiltroDeAccion(ILogger<MiFiltroDeAccion> logger)
        {
            Logger = logger;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            Logger.LogInformation("antes de ejecutar la accion ");
        }
        public void OnActionExecuted(ActionExecutedContext context)
        {
            Logger.LogInformation("Despues de ejecutar la accion");
        }

       
    }
}
