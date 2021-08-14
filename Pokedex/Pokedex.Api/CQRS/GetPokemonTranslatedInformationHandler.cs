using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Pokedex.Api.CQRS.Common;
using Pokedex.Services.PokemonService;
using Pokedex.Services.PokemonService.Exception;
using Pokedex.Services.PokemonService.Model;
using Pokedex.Services.TranslationService.Common;
using Pokedex.Services.TranslationService.Common.Model;
using Pokedex.Services.TranslationService.ShakespeareTranslationService;
using Pokedex.Services.TranslationService.YodaTranslationService;

namespace Pokedex.Api.CQRS
{
    public class GetPokemonTranslatedInformationHandler : IRequestHandler<GetPokemonTranslatedInformationHandler.Context, PokemonInformation>
    {
        private readonly IPokemonService _pokemonService;
        private readonly IShakespeareTranslationService _shakespeareTranslationService;
        private readonly IYodaTranslationService _yodaTranslationService;
        private readonly ILogger<GetPokemonTranslatedInformationHandler> _logger;

        public GetPokemonTranslatedInformationHandler(IPokemonService pokemonService, IShakespeareTranslationService shakespeareTranslationService, IYodaTranslationService yodaTranslationService, ILogger<GetPokemonTranslatedInformationHandler> logger)
        {
            _pokemonService = pokemonService;
            _shakespeareTranslationService = shakespeareTranslationService;
            _yodaTranslationService = yodaTranslationService;
            _logger = logger;
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

        private async Task<PokemonInformation> GetPokemonInformationAsync(string pokemonName, CancellationToken cancellationToken)
        {
            var pokemonInformation = new PokemonInformation();
            
            try
            {
                var response = await _pokemonService.GetPokemonInformationAsync(pokemonName, cancellationToken);
                pokemonInformation = GetPokemonInformation(response, pokemonName);
            }
            catch (PokemonServiceException exception)
            {
                _logger.LogError("Error occurred while trying to get pokemon information", exception);
            }

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
        
        private static PokemonInformation GetPokemonInformation(PokemonInformationResponse response, string pokemonName)
        {
            return new PokemonInformation
            {
                Description = response.Description,
                Habitat = response.Habitat,
                IsLegendary = response.IsLegendary,
                Name = pokemonName
            };
        }
    }
}