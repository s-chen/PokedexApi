using Newtonsoft.Json;

namespace Pokedex.Services.PokemonService.Schema
{
    public class Habitat
    {
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}