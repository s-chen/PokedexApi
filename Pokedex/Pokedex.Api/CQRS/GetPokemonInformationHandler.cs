using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Pokedex.Api.CQRS.Common;
using Pokedex.Services.PokemonService;

namespace Pokedex.Api.CQRS
{
    public class GetPokemonInformationHandler : PokemonInformationHandlerBase, IRequestHandler<GetPokemonInformationHandler.Context, PokemonInformation>
    {
        public GetPokemonInformationHandler(IPokemonService pokemonService, ILogger<GetPokemonInformationHandler> logger)
            : base(pokemonService, logger)
        {
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
            return await GetPokemonInformationAsync(request.PokemonName, cancellationToken);
        }
    }
}