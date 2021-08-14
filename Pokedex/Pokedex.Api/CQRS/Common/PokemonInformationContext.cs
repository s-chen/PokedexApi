using MediatR;

namespace Pokedex.Api.CQRS.Common
{
    public struct PokemonInformationContext : IRequest<PokemonInformation>
    {
        public string PokemonName { get; }
        
        public PokemonInformationContext(string pokemonName)
        {
            PokemonName = pokemonName;
        }
    }
}