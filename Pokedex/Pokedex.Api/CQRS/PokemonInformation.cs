namespace Pokedex.Api.CQRS
{
    public class PokemonInformation
    {
        public string Name { get; set; }
        
        public string Habitat { get; set; }
        
        public bool IsLegendary { get; set; }
        
        public string Description { get; set; }
    }
}