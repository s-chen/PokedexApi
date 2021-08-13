using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Flurl.Http;
using Flurl.Http.Configuration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Pokedex.Services.PokemonService.Exception;
using Pokedex.Services.PokemonService.Model;
using Pokedex.Services.PokemonService.Options;
using Pokedex.Services.PokemonService.Schema;

namespace Pokedex.Services.PokemonService
{
    public class PokemonService : IPokemonService
    {
        private readonly PokemonServiceOptions _options;
        private readonly IFlurlClient _flurlClient;


        public PokemonService(IFlurlClientFactory flurlClientFactory, IOptions<PokemonServiceOptions> options)
        {
            _options = options.Value; 
            _flurlClient = flurlClientFactory.Get(_options.Host);
        }
        
        public async Task<PokemonInformationResponse> GetPokemonInformationAsync(string pokemonName, CancellationToken cancellationToken)
        {
            var url = $"{_options.Host}/pokemon-species/{pokemonName}";
            var flurlRequest = _flurlClient.Request(url);

            try
            {
                using (var response = await flurlRequest.GetAsync(cancellationToken))
                {
                    var responseContent = await response.GetStringAsync();
                    var responseSchema = JsonConvert.DeserializeObject<PokemonInformationServiceResponse>(responseContent);
                    return MapPokemonInformation(responseSchema);
                }
            }
            catch (System.Exception exception)
            {
                throw new PokemonServiceException("Error occurred while trying to retrieve Pokemon information", exception);
            }
        }

        private static PokemonInformationResponse MapPokemonInformation(PokemonInformationServiceResponse response)
        {
            if (response == null)
            {
                return new PokemonInformationResponse();
            }

            var description = response.FlavorTexts?.First(x => x.Language?.Name == "en").Description;

            var pokemonInformation = new PokemonInformationResponse
            {
                Description = description,
                Habitat = response.Habitat?.Name,
                IsLegendary = bool.Parse(response.IsLegendary)
            };

            return pokemonInformation;
        }
    }
}