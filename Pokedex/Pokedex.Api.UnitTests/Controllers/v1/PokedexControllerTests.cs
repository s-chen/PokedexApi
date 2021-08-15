using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Pokedex.Api.Controllers.v1;
using Pokedex.Api.CQRS.Common;
using Pokedex.Api.Models.v1.Requests;
using Xunit;

namespace Pokedex.Api.UnitTests.Controllers.v1
{
    public class PokedexControllerTests
    {
        private readonly Mock<IMediator> _mockMediator;
        private readonly PokedexController _controller;
        
        public PokedexControllerTests()
        {
            _mockMediator = new Mock<IMediator>();
            _controller = new PokedexController(_mockMediator.Object);
        }
        
        [Fact]
        public async Task GetPokemonInformationShouldReturnOkObjectResult()
        {
            // Arrange
            _mockMediator.Setup(x => x.Send(It.IsAny<IRequest<PokemonInformation>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new PokemonInformation());

            // Act
            var result = await _controller.GetPokemonInformation(new GetPokemonInformationRequest { PokemonName = "pokemonName"}, CancellationToken.None);
            
            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<OkObjectResult>();
        }
        
        [Fact]
        public async Task GetPokemonTranslatedInformationShouldReturnOkObjectResult()
        {
            // Arrange
            _mockMediator.Setup(x => x.Send(It.IsAny<IRequest<PokemonInformation>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new PokemonInformation());

            // Act
            var result = await _controller.GetPokemonTranslatedInformation(new GetPokemonInformationRequest { PokemonName = "pokemonName"}, CancellationToken.None);
            
            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<OkObjectResult>();
        }
    }
}