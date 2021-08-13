using Newtonsoft.Json;

namespace Pokedex.Services.PokemonService.Schema
{
    public class Language
    {
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}