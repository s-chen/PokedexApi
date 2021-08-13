using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Pokedex.Services.PokemonService;
using Pokedex.Services.PokemonService.Exception;
using Pokedex.Services.PokemonService.Model;

namespace Pokedex.Api.CQRS
{
    public class GetPokemonInformationHandler : IRequestHandler<GetPokemonInformationHandler.Context, PokemonInformation>
    {
        private IPokemonService _pokemonService;
        
        public GetPokemonInformationHandler(IPokemonService pokemonService)
        {
            _pokemonService = pokemonService;
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
                
            }

            return pokemonInformation;
        }
        
        public struct Context : IRequest<PokemonInformation>
        {
            public string PokemonName { get; }
        
            public Context(string pokemonName)
            {
                PokemonName = pokemonName;
            }
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