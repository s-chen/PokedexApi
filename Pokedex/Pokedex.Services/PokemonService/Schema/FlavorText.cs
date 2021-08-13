using Newtonsoft.Json;

namespace Pokedex.Services.PokemonService.Schema
{
    public class FlavorText
    {
        [JsonProperty("flavor_text")]
        public string Description { get; set; }
        
        [JsonProperty("language")]
        public Language Language { get; set; }
    }
}