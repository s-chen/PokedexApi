using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Flurl.Http.Configuration;
using Flurl.Http.Testing;
using Microsoft.Extensions.Options;
using Moq;
using Pokedex.Services.PokemonService.Exception;
using Pokedex.Services.PokemonService.Options;
using Xunit;

namespace Pokedex.Services.UnitTests.PokemonService
{
    public class PokemonServiceTests
    {
        private readonly Services.PokemonService.PokemonService _service;
        private readonly Mock<IOptions<PokemonServiceOptions>> _mockOptions;
        
        public PokemonServiceTests()
        {
            _mockOptions = new Mock<IOptions<PokemonServiceOptions>>();
            _mockOptions.Setup(x => x.Value).Returns(new PokemonServiceOptions { Host = "http://testapi.com" });

            var flurlClientFactory = new DefaultFlurlClientFactory();
            _service = new Services.PokemonService.PokemonService(flurlClientFactory, _mockOptions.Object);
        }

        [Fact]
        public async Task GetPokemonInformationAsyncShouldReturnPokemonInformation()
        {
            using (var httpTest = new HttpTest())
            {
                // Arrange
                httpTest.RespondWith(LoadJson("pokemonSuccess.json"), 200);
                
                // Act
                var result = await _service.GetPokemonInformationAsync("mewtwo", CancellationToken.None);
                
                // Assert
                result.Should().NotBeNull();
                result.Description.Should().NotBeNullOrWhiteSpace();
                result.Habitat.Should().Be("rare");
                result.IsLegendary.Should().BeTrue();
            }
        }
        
        [Fact]
        public void GetPokemonInformationAsyncShouldThrowPokemonNoContentExceptionWhenResponseIsNull()
        {
            using (var httpTest = new HttpTest())
            {
                // Arrange
                httpTest.RespondWith(LoadJson("pokemonNoContent.json"), 200);
                
                // Act
                Func<Task> func = async () => await _service.GetPokemonInformationAsync("mewtwo", CancellationToken.None);
                
                // Assert
                func.Should().ThrowAsync<PokemonNoContentException>();
            }
        }
        
        [Fact]
        public void GetPokemonInformationAsyncShouldThrowPokemonNotFoundExceptionWhenStatusIs404()
        {
            using (var httpTest = new HttpTest())
            {
                // Arrange
                httpTest.RespondWith(string.Empty, 404);
                
                // Act
                Func<Task> func = async () => await _service.GetPokemonInformationAsync("mewtwo", CancellationToken.None);
                
                // Assert
                func.Should().ThrowAsync<PokemonNotFoundException>();
            }
        }
        
        [Theory]
        [InlineData(500)]
        [InlineData(503)]
        public void GetPokemonInformationAsyncShouldThrowPokemonServiceExceptionForErrorStatus(int status)
        {
            using (var httpTest = new HttpTest())
            {
                // Arrange
                httpTest.RespondWith(string.Empty, status);
                
                // Act
                Func<Task> func = async () => await _service.GetPokemonInformationAsync("mewtwo", CancellationToken.None);
                
                // Assert
                func.Should().ThrowAsync<PokemonServiceException>();
            }
        }
        
        private string LoadJson(string fileName)
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), $"PokemonService/Responses/{fileName}");
            using (StreamReader r = new StreamReader(filePath))
            {
                string json = r.ReadToEnd();
                return json;
            }
        }
    }
}