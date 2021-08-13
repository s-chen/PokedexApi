using System.Threading;
using System.Threading.Tasks;
using Pokedex.Services.PokemonService.Model;

namespace Pokedex.Services.PokemonService
{
    public interface IPokemonService
    {
         Task<PokemonInformationResponse> GetPokemonInformationAsync(string pokemonName, CancellationToken cancellationToken);
    }
}