using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Pokedex.Api.CQRS;
using Pokedex.Api.CQRS.Common;
using Pokedex.Api.Models.v1.Requests;

namespace Pokedex.Api.Controllers.v1
{
    [Route("api/v1")]
    public class PokedexController : ControllerBase
    {
        private IMediator _mediator;
        public PokedexController(IMediator mediator)
        {
            _mediator = mediator;
        }
        
        [HttpGet("/pokemon/{pokemonName}")]
        public async Task<IActionResult> GetPokemonInformation([FromRoute] GetPokemonInformationRequest request, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetPokemonInformationHandler.Context(request.PokemonName), cancellationToken);
            return Ok(result);
        }

        [HttpGet("/pokemon/translated/{pokemonName}")]
        public async Task<IActionResult> GetPokemonTranslatedInformation([FromRoute] GetPokemonInformationRequest request, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetPokemonTranslatedInformationHandler.Context(request.PokemonName), cancellationToken);
            return Ok(result);
        }
    }
}