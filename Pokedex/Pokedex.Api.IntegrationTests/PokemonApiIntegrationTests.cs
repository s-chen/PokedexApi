using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Flurl.Http;
using Flurl.Http.Configuration;
using Xunit;

namespace Pokedex.Api.IntegrationTests
{
    public class PokemonApiIntegrationTests
    {
        private const string Host = "http://localhost:8080";
        private IFlurlRequest _flurlRequest;
        private readonly IFlurlClient _flurlClient;
        
        public PokemonApiIntegrationTests()
        { ;
            var flurlClientFactory = new DefaultFlurlClientFactory();
            _flurlClient = flurlClientFactory.Get(Host);
        }
        
        [Fact]
        public async Task ShouldCallServiceAndReturnPokemonInformation()
        {
            // Arrange
            var url = $"{Host}/pokemon/mewtwo";
            _flurlRequest = _flurlClient.Request(url);
            
            using var response = await _flurlRequest.GetAsync(CancellationToken.None);
            var responseContent = await response.GetStringAsync();

            responseContent.Should().NotBeNullOrWhiteSpace();
        }
        
        [Fact]
        public async Task ShouldCallServiceAndReturnTranslatedPokemonInformation()
        {
            // Arrange
            var url = $"{Host}/pokemon/translated/mewtwo";
            _flurlRequest = _flurlClient.Request(url);
            
            using var response = await _flurlRequest.GetAsync(CancellationToken.None);
            var responseContent = await response.GetStringAsync();

            responseContent.Should().NotBeNullOrWhiteSpace();
        }
    }
}
