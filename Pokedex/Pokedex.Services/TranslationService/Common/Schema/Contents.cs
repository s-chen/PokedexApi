using Newtonsoft.Json;

namespace Pokedex.Services.TranslationService.Common.Schema
{
    public class Contents
    {
        [JsonProperty("translated")]
        public string Translated { get; set; }
        
        [JsonProperty("text")]
        public  string Text { get; set; }
        
        [JsonProperty("translation")]
        public string Translation { get; set; }
    }
}