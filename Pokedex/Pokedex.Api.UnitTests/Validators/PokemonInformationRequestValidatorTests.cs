using System.Linq;
using FluentAssertions;
using Pokedex.Api.Models.v1.Requests;
using Pokedex.Api.Validators;
using Xunit;

namespace Pokedex.Api.UnitTests.Validators
{
    public class PokemonInformationRequestValidatorTests
    {
        private readonly PokemonInformationRequestValidator _validator;
        
        public PokemonInformationRequestValidatorTests()
        {
            _validator = new PokemonInformationRequestValidator();
        }
        
        [Fact]
        public void ShouldReturnValidationErrorWhenPokemonNameIsEmpty()
        {
            // Arrange
            var model = new GetPokemonInformationRequest();
            
            // Act
            var result = _validator.Validate(model);
            
            // Assert
            result.Errors.Should().HaveCount(1);
            result.Errors.First().PropertyName.Should().Be("PokemonName");
        }
        
        [Theory]
        [InlineData("mewtwo123")]
        [InlineData("&m3wtw0*")]
        public void ShouldReturnValidationErrorWhenPokemonNameContainsNonAlphabetCharacters(string pokemonName)
        {
            // Arrange
            var model = new GetPokemonInformationRequest { PokemonName = pokemonName };
            
            // Act
            var result = _validator.Validate(model);
            
            // Assert
            result.Errors.Should().HaveCount(1);
            result.Errors.First().PropertyName.Should().Be("PokemonName");
            result.Errors.First().ErrorMessage.Should().Be("'Pokemon Name' is not in the correct format.");
        }
    }
}