using Newtonsoft.Json;

namespace Pokedex.Services.TranslationService.Common.Schema
{
    public class TranslatedServiceResponse
    {
        [JsonProperty("contents")]
        public Contents Contents { get; set; }
    }
}