using System.Collections.Generic;
using Newtonsoft.Json;

namespace Pokedex.Services.PokemonService.Schema
{
    public class PokemonInformationResponse
    {
        [JsonProperty("is_legendary")]
        public string IsLegendary { get; set; }
        
        [JsonProperty("habitat")]
        public Habitat Habitat { get; set; }
        
        [JsonProperty("flavor_text_entries")]
        public IEnumerable<FlavorText> FlavorTexts { get; set; }
    }
}