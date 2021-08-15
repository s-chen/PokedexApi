using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Pokedex.Api.CQRS.Common;
using Pokedex.Services.PokemonService;
using Pokedex.Services.TranslationService.Common.Model;
using Pokedex.Services.TranslationService.ShakespeareTranslationService;
using Pokedex.Services.TranslationService.YodaTranslationService;

namespace Pokedex.Api.CQRS
{
    public class GetPokemonTranslatedInformationHandler : PokemonInformationHandlerBase, IRequestHandler<GetPokemonTranslatedInformationHandler.Context, PokemonInformation>
    {
        private readonly IShakespeareTranslationService _shakespeareTranslationService;
        private readonly IYodaTranslationService _yodaTranslationService;

        public GetPokemonTranslatedInformationHandler(IPokemonService pokemonService, IShakespeareTranslationService shakespeareTranslationService, IYodaTranslationService yodaTranslationService, ILogger<GetPokemonTranslatedInformationHandler> logger)
            : base(pokemonService, logger)
        {
            _shakespeareTranslationService = shakespeareTranslationService;
            _yodaTranslationService = yodaTranslationService;
        }
        
        public struct Context : IRequest<PokemonInformation>
        {
            public string PokemonName { get; }
            
            public Context(string pokemonName)
            {
                PokemonName = pokemonName;
            }
        }
        
        public async Task<PokemonInformation> Handle(Context request, CancellationToken cancellationToken)
        {
            var pokemonInformation = await GetPokemonInformationAsync(request.PokemonName, cancellationToken);
            pokemonInformation.Description = await GetTranslatedDescriptionAsync(pokemonInformation.Description, pokemonInformation.Habitat, pokemonInformation.IsLegendary, cancellationToken);

            return pokemonInformation;
        }

        private async Task<string> GetTranslatedDescriptionAsync(string description, string habitat, bool isLegendary, CancellationToken cancellationToken)
        {
            var translatedResponse = new TranslatedResponse();
            
            if (habitat.Equals("cave", StringComparison.InvariantCultureIgnoreCase) || isLegendary)
            {
                translatedResponse = await _yodaTranslationService.GetTranslationAsync(description, cancellationToken);
            }
            else
            {
                translatedResponse = await _shakespeareTranslationService.GetTranslationAsync(description, cancellationToken);
            }

            var result = !string.IsNullOrWhiteSpace(translatedResponse.TranslatedText)
                ? translatedResponse.TranslatedText
                : description;

            return result;
        }
    }
}