using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Pokedex.Api.CQRS.Common;
using Pokedex.Services.PokemonService;
using Pokedex.Services.PokemonService.Exception;
using Pokedex.Services.PokemonService.Model;

namespace Pokedex.Api.CQRS
{
    public class GetPokemonInformationHandler : IRequestHandler<GetPokemonInformationHandler.Context, PokemonInformation>
    {
        private readonly IPokemonService _pokemonService;
        private readonly ILogger<GetPokemonInformationHandler> _logger;

        public GetPokemonInformationHandler(IPokemonService pokemonService, ILogger<GetPokemonInformationHandler> logger)
        {
            _pokemonService = pokemonService;
            _logger = logger;
        }

        public struct Context : IRequest<PokemonInformation>
        {
            public string PokemonName { get; }
            
            public Context(string pokemonName)
            {
                PokemonName = pokemonName;
            }
        }

        public async Task<PokemonInformation> Handle(Context request, CancellationToken cancellationToken)
        {
            var pokemonInformation = new PokemonInformation();
            
            try
            {
                var response = await _pokemonService.GetPokemonInformationAsync(request.PokemonName, cancellationToken);
                pokemonInformation = GetPokemonInformation(response, request.PokemonName);
            }
            catch (PokemonServiceException exception)
            {
                _logger.LogError("Error occurred while trying to get pokemon information", exception);
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