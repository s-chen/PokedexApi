using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Pokedex.Api.Models.v1.Errors;
using Pokedex.Services.PokemonService.Exception;
using Pokedex.Services.TranslationService.Common.Exception;

namespace Pokedex.Api.Filters
{
    public class PokemonExceptionActionFilter : IActionFilter
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
                
                case PokemonServiceException:
                case TranslationServiceException:
                    context.ExceptionHandled = true;
                    context.Result = new JsonResult(new InternalServerErrorResponse { Title = "Internal Server Error", Detail = "Error encountered while trying to retrieve Pokemon Information", Status = (int)HttpStatusCode.InternalServerError });
                    break;
            }
        }
    }
}