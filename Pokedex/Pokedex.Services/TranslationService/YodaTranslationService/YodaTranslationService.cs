using Flurl.Http.Configuration;
using Microsoft.Extensions.Options;
using Pokedex.Services.TranslationService.Common.Options;

namespace Pokedex.Services.TranslationService.YodaTranslationService
{
    public class YodaTranslationService : Common.TranslationService, IYodaTranslationService
    {
        public YodaTranslationService(IFlurlClientFactory flurlClientFactory, IOptions<TranslationServiceOptions> options) 
            : base(flurlClientFactory, options)
        {
        }
        
        protected override string GetUrl()
        {
            return $"{Options.Host}/yoda";
        }
    }
}