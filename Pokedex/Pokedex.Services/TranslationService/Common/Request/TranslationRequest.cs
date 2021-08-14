using Newtonsoft.Json;

namespace Pokedex.Services.TranslationService.Common.Request
{
    public class TranslationRequest
    {
        [JsonProperty("text")]
        public string Text { get; set; }
    }
}