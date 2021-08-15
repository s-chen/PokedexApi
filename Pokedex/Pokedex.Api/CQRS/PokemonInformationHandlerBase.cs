using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Pokedex.Api.CQRS.Common;
using Pokedex.Services.PokemonService;
using Pokedex.Services.PokemonService.Exception;
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
            var pokemonInformation = new PokemonInformation();
            
            try
            {
                var response = await PokemonService.GetPokemonInformationAsync(pokemonName, cancellationToken);
                pokemonInformation = GetPokemonInformation(response, pokemonName);
            }
            catch (PokemonServiceException exception)
            {
                Logger.LogError("Error occurred while trying to get pokemon information", exception);
            }

            return pokemonInformation;
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