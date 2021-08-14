using System.Threading;
using System.Threading.Tasks;
using Pokedex.Services.TranslationService.Common.Model;

namespace Pokedex.Services.TranslationService.YodaTranslationService
{
    public interface IYodaTranslationService
    {
        Task<TranslatedResponse> GetTranslationAsync(string textToTranslate, CancellationToken cancellationToken);
    }
}