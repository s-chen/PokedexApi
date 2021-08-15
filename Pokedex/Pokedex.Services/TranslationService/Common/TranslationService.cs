using System.Threading;
using System.Threading.Tasks;
using Flurl.Http;
using Flurl.Http.Configuration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Pokedex.Services.TranslationService.Common.Exception;
using Pokedex.Services.TranslationService.Common.Mappers;
using Pokedex.Services.TranslationService.Common.Model;
using Pokedex.Services.TranslationService.Common.Options;
using Pokedex.Services.TranslationService.Common.Request;
using Pokedex.Services.TranslationService.Common.Schema;

namespace Pokedex.Services.TranslationService.Common
{
    public abstract class TranslationService
    {
        private IFlurlClient _flurlClient;
        protected TranslationServiceOptions Options { get; }

        protected TranslationService(IFlurlClientFactory flurlClientFactory, IOptions<TranslationServiceOptions> options)
        {
            Options = options.Value;
            _flurlClient = flurlClientFactory.Get(options.Value.Host);
        }
        
        public async Task<TranslatedResponse> GetTranslationAsync(string textToTranslate, CancellationToken cancellationToken)
        {
            var url = this.GetUrl();
            var flurlRequest = _flurlClient.Request(url);
            
            try
            {
                var request = new TranslationRequest { Text = textToTranslate };
                using (var response = await flurlRequest.PostJsonAsync(request, cancellationToken))
                {
                    var responseContent = await response.GetStringAsync();
                    var responseSchema = JsonConvert.DeserializeObject<TranslatedServiceResponse>(responseContent);
                    return TranslationMapper.Map(responseSchema);
                }
            }
            catch (FlurlHttpException exception)
            {
                throw new TranslationServiceException("Error occurred while trying to translate text", exception);
            }
        }

        protected abstract string GetUrl();
    }
}