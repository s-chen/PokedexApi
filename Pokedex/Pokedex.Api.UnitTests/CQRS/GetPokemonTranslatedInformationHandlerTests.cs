using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Pokedex.Api.CQRS;
using Pokedex.Services.PokemonService;
using Pokedex.Services.PokemonService.Model;
using Pokedex.Services.TranslationService.Common.Model;
using Pokedex.Services.TranslationService.ShakespeareTranslationService;
using Pokedex.Services.TranslationService.YodaTranslationService;
using Xunit;

namespace Pokedex.Api.UnitTests.CQRS
{
    public class GetPokemonTranslatedInformationHandlerTests
    {
        private readonly Mock<IPokemonService> _mockPokemonService;
        private readonly Mock<IShakespeareTranslationService> _mockShakespeareTranslationService;
        private readonly Mock<IYodaTranslationService> _mockYodaTranslationService;
        private readonly Mock<ILogger<GetPokemonTranslatedInformationHandler>> _mockLogger;
        private readonly GetPokemonTranslatedInformationHandler _handler;

        public GetPokemonTranslatedInformationHandlerTests()
        {
            _mockPokemonService = new Mock<IPokemonService>();
            _mockShakespeareTranslationService = new Mock<IShakespeareTranslationService>();
            _mockYodaTranslationService = new Mock<IYodaTranslationService>();
            _mockLogger = new Mock<ILogger<GetPokemonTranslatedInformationHandler>>();
            
            _handler = new GetPokemonTranslatedInformationHandler(_mockPokemonService.Object, _mockShakespeareTranslationService.Object, _mockYodaTranslationService.Object, _mockLogger.Object);
        }
        
        [Fact]
        public async Task ShouldReturnPokemonInformation()
        {
            // Arrange
            _mockPokemonService.Setup(x => x.GetPokemonInformationAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new PokemonInformationResponse { Description = "description", Habitat = "cave", IsLegendary = false });
            
            // Act
            var result = await _handler.Handle(new GetPokemonTranslatedInformationHandler.Context("pokemonName"), CancellationToken.None);
            
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

        [Theory]
        [InlineData("cave", false)]
        [InlineData("cave", true)]
        [InlineData("forest", true)]
        public async Task ShouldInvokeYodaTranslationServiceAndReturnPokemonInformationWithTranslatedDescription(string habitat, bool isLegendary)
        {
            // Arrange
            _mockPokemonService.Setup(x => x.GetPokemonInformationAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new PokemonInformationResponse { Description = "description", Habitat = habitat, IsLegendary = isLegendary });

            _mockYodaTranslationService.Setup(x => x.GetTranslationAsync("description", CancellationToken.None))
                .ReturnsAsync(new TranslatedResponse { StandardDescription = "description", TranslatedText = "translatedDescription" });
            
            // Act
            var result = await _handler.Handle(new GetPokemonTranslatedInformationHandler.Context("pokemonName"), CancellationToken.None);
            
            // Assert
            result.Should().NotBeNull();
            result.Habitat.Should().Be(habitat);
            result.Description.Should().Be("translatedDescription");
            result.IsLegendary.Should().Be(isLegendary);
            result.Name.Should().Be("pokemonName");
            _mockYodaTranslationService.Verify(x => x.GetTranslationAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
            _mockShakespeareTranslationService.Verify(x => x.GetTranslationAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
        }
        
        [Theory]
        [InlineData("forest", false)]
        [InlineData("land", false)]
        [InlineData("water", false)]
        public async Task ShouldInvokeShakespeareTranslationServiceAndReturnPokemonInformationWithTranslatedDescription(string habitat, bool isLegendary)
        {
            // Arrange
            _mockPokemonService.Setup(x => x.GetPokemonInformationAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new PokemonInformationResponse { Description = "description", Habitat = habitat, IsLegendary = isLegendary });

            _mockShakespeareTranslationService.Setup(x => x.GetTranslationAsync("description", CancellationToken.None))
                .ReturnsAsync(new TranslatedResponse { StandardDescription = "description", TranslatedText = "translatedDescription" });
            
            // Act
            var result = await _handler.Handle(new GetPokemonTranslatedInformationHandler.Context("pokemonName"), CancellationToken.None);
            
            // Assert
            result.Should().NotBeNull();
            result.Habitat.Should().Be(habitat);
            result.Description.Should().Be("translatedDescription");
            result.IsLegendary.Should().Be(isLegendary);
            result.Name.Should().Be("pokemonName");
            _mockYodaTranslationService.Verify(x => x.GetTranslationAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
            _mockShakespeareTranslationService.Verify(x => x.GetTranslationAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task ShouldReturnStandardDescriptionWhenNoTranslatedTextReturnedFromTranslationService()
        {
            // Arrange
            _mockPokemonService.Setup(x => x.GetPokemonInformationAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new PokemonInformationResponse { Description = "description", Habitat = "forest", IsLegendary = false });

            _mockShakespeareTranslationService.Setup(x => x.GetTranslationAsync("description", CancellationToken.None))
                .ReturnsAsync(new TranslatedResponse { StandardDescription = "description", TranslatedText = string.Empty });
            
            // Act
            var result = await _handler.Handle(new GetPokemonTranslatedInformationHandler.Context("pokemonName"), CancellationToken.None);
            
            // Assert
            result.Should().NotBeNull();
            result.Description.Should().Be("description");
        }
    }
}