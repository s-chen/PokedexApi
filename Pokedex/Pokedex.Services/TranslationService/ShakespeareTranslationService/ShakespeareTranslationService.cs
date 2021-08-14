using Flurl.Http.Configuration;
using Microsoft.Extensions.Options;
using Pokedex.Services.TranslationService.Common.Options;

namespace Pokedex.Services.TranslationService.ShakespeareTranslationService
{
    public class ShakespeareTranslationService : Common.TranslationService, IShakespeareTranslationService
    {
        public ShakespeareTranslationService(IFlurlClientFactory flurlClientFactory, IOptions<TranslationServiceOptions> options) 
            : base(flurlClientFactory, options)
        {
        }
        
        protected override string GetUrl()
        {
            return $"{Options.Host}/shakespeare";
        }
    }
}