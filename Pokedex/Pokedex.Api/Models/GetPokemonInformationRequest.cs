using Microsoft.AspNetCore.Mvc;

namespace Pokedex.Api.Models
{
    public class GetPokemonInformationRequest
    {
        [FromRoute]
        public string PokemonName { get; set; }
    }
}