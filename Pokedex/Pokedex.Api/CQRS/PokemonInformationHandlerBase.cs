using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Pokedex.Api.CQRS.Common;
using Pokedex.Services.PokemonService;
using Pokedex.Services.PokemonService.Model;

namespace Pokedex.Api.CQRS
{
    public abstract class PokemonInformationHandlerBase
    {
        protected IPokemonService PokemonService { get; }
        protected ILogger<PokemonInformationHandlerBase> Logger { get; }

        protected PokemonInformationHandlerBase(IPokemonService pokemonService, ILogger<PokemonInformationHandlerBase> logger)
        {
            PokemonService = pokemonService;
            Logger = logger;
        }
        
        protected async Task<PokemonInformation> GetPokemonInformationAsync(string pokemonName, CancellationToken cancellationToken)
        {
            var response = await PokemonService.GetPokemonInformationAsync(pokemonName, cancellationToken);
            Logger.LogInformation($"Response retrieved for pokemon {pokemonName}");
            return GetPokemonInformation(response, pokemonName);
        }
        
        private static PokemonInformation GetPokemonInformation(PokemonInformationResponse response, string pokemonName)
        {
            return new PokemonInformation
            {
                Description = response.Description,
                Habitat = response.Habitat,
                IsLegendary = response.IsLegendary,
                Name = pokemonName
            };
        }
    }
}