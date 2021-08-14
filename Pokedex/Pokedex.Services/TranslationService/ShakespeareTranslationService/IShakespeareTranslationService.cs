using System.Threading;
using System.Threading.Tasks;
using Pokedex.Services.TranslationService.Common.Model;

namespace Pokedex.Services.TranslationService.ShakespeareTranslationService
{
    public interface IShakespeareTranslationService
    {
        Task<TranslatedResponse> GetTranslationAsync(string pokemonName, CancellationToken cancellationToken);
    }
}