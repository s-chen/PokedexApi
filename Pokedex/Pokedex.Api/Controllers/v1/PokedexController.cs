using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Pokedex.Api.Models;

namespace Pokedex.Api.Controllers.v1
{
    [Route("api/v1")]
    public class PokedexController : ControllerBase
    {
        [HttpGet("/pokemon/{pokemonName}")]
        public async Task<IActionResult> GetPokemonInformation([FromRoute] GetPokemonInformationRequest request, CancellationToken cancellationToken)
        {
            return await Task.FromResult(this.Ok());
        }
    }
}