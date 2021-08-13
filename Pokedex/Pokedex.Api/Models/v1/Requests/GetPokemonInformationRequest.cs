using Microsoft.AspNetCore.Mvc;

namespace Pokedex.Api.Models.v1.Requests
{
    public class GetPokemonInformationRequest
    {
        [FromRoute]
        public string PokemonName { get; set; }
    }
}