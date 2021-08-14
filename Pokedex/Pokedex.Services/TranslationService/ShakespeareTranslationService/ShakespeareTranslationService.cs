using System.Threading;
using System.Threading.Tasks;
using Flurl.Http;
using Flurl.Http.Configuration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Pokedex.Services.TranslationService.Common.Mappers;
using Pokedex.Services.TranslationService.Common.Model;
using Pokedex.Services.TranslationService.Common.Options;
using Pokedex.Services.TranslationService.Common.Schema;
using Pokedex.Services.TranslationService.ShakespeareTranslationService.Exception;

namespace Pokedex.Services.TranslationService.ShakespeareTranslationService
{
    public class ShakespeareTranslationService : IShakespeareTranslationService
    {
        private readonly IFlurlClient _flurlClient;
        private readonly TranslationServiceOptions _options;

        public ShakespeareTranslationService(IFlurlClientFactory flurlClientFactory, IOptions<TranslationServiceOptions> options)
        {
            _options = options.Value;
            _flurlClient = flurlClientFactory.Get(_options.Host);
        }
        
        public async Task<TranslatedResponse> GetTranslationAsync(string pokemonName, CancellationToken cancellationToken)
        {
            var url = $"{_options.Host}/shakespeare";
            var flurlRequest = _flurlClient.Request(url);
            
            try
            {
                using (var response = await flurlRequest.GetAsync(cancellationToken))
                {
                    var responseContent = await response.GetStringAsync();
                    var responseSchema = JsonConvert.DeserializeObject<TranslatedServiceResponse>(responseContent);
                    return TranslationMapper.Map(responseSchema);
                }
            }
            catch (System.Exception exception)
            {
                throw new ShakespeareTranslationServiceException("Error occurred while trying to translate text to Shakespeare", exception);
            }
        }
    }
}