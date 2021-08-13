using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Flurl.Http;
using Flurl.Http.Configuration;
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


        public PokemonService(IFlurlClientFactory flurlClientFactory, PokemonServiceOptions options)
        {
            _options = options; 
            _flurlClient = flurlClientFactory.Get(_options.Endpoint);
        }
        
        public async Task<PokemonInformation> GetPokemonInformationAsync(string pokemonName, CancellationToken cancellationToken)
        {
            var flurlRequest = _flurlClient.Request();

            try
            {
                using (var response = await flurlRequest.GetAsync(cancellationToken))
                {
                    var responseContent = await response.GetStringAsync();
                    var responseSchema = JsonConvert.DeserializeObject<PokemonInformationResponse>(responseContent);
                    return MapPokemonInformation(responseSchema);
                }
            }
            catch (System.Exception exception)
            {
                throw new PokemonServiceException("Error occurred while trying to retrive Pokemon information", exception);
            }
        }

        private static PokemonInformation MapPokemonInformation(PokemonInformationResponse response)
        {
            if (response == null)
            {
                return new PokemonInformation();
            }

            var description = response.FlavorTexts?.First(x => x.Language?.Name == "en").Description;

            var pokemonInformation = new PokemonInformation
            {
                Description = description,
                Habitat = response.Habitat?.Name,
                IsLegendary = bool.Parse(response.IsLegendary)
            };

            return pokemonInformation;
        }
    }
}