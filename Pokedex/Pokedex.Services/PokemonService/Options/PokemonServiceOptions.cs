namespace Pokedex.Services.PokemonService.Options
{
    public struct PokemonServiceOptions
    {
        public PokemonServiceOptions(string endpoint)
        {
            Endpoint = endpoint;
        }
        
        public string Endpoint { get; }
    }
}