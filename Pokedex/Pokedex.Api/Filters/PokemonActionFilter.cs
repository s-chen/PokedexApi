using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Pokedex.Services.PokemonService.Exception;

namespace Pokedex.Api.Filters
{
    public class PokemonActionFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            switch (context.Exception)
            {
                case PokemonNotFoundException:
                    context.ExceptionHandled = true;
                    context.Result = new NotFoundObjectResult(context.Exception.Message);
                    break;
                
                case PokemonNoContentException:
                    context.ExceptionHandled = true;
                    context.Result = new NoContentResult();
                    break;
            }
        }
    }
}