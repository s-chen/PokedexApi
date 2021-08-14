using Pokedex.Services.TranslationService.Common.Enums;

namespace Pokedex.Services.TranslationService.Common.Model
{
    public class TranslatedResponse
    {
        public string TranslatedText { get; set; }
        
        public string StandardDescription { get; set; }
        
        public TranslationType TranslationType { get; set; }
    }
}