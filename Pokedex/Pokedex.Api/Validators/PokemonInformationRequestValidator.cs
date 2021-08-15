using FluentValidation;
using Pokedex.Api.Models.v1.Requests;

namespace Pokedex.Api.Validators
{
    public class PokemonInformationRequestValidator : AbstractValidator<GetPokemonInformationRequest>
    {
        private const string LettersOnlyRegex = @"^[a-zA-Z]+$";
        
        public PokemonInformationRequestValidator()
        {
            RuleFor(x => x.PokemonName).NotEmpty();
            RuleFor(x => x.PokemonName).Matches(LettersOnlyRegex);
        }
    }
}