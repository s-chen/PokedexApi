using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Pokedex.Api.CQRS;
using Pokedex.Services.PokemonService;
using Pokedex.Services.PokemonService.Model;
using Xunit;

namespace Pokedex.Api.UnitTests.CQRS
{
    public class GetPokemonInformationHandlerTests
    {
        private readonly Mock<IPokemonService> _mockPokemonService;
        private readonly Mock<ILogger<GetPokemonInformationHandler>> _mockLogger;
        private readonly GetPokemonInformationHandler _handler;
        
        public GetPokemonInformationHandlerTests()
        {
            _mockPokemonService = new Mock<IPokemonService>();
            _mockLogger = new Mock<ILogger<GetPokemonInformationHandler>>();
            _handler = new GetPokemonInformationHandler(_mockPokemonService.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task ShouldReturnPokemonInformation()
        {
            // Arrange
            _mockPokemonService.Setup(x => x.GetPokemonInformationAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new PokemonInformationResponse { Description = "description", Habitat = "cave", IsLegendary = false });
            
            // Act
            var result = await _handler.Handle(new GetPokemonInformationHandler.Context("pokemonName"), CancellationToken.None);
            
            // Assert
            result.Should().NotBeNull();
            result.Description.Should().Be("description");
            result.Habitat.Should().Be("cave");
            result.IsLegendary.Should().BeFalse();
            result.Name.Should().Be("pokemonName");
            _mockLogger.Verify(
                x => x.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.IsAny<It.IsAnyType>(),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
        }
    }
}