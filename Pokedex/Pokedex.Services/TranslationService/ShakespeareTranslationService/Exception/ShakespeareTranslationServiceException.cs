namespace Pokedex.Services.TranslationService.ShakespeareTranslationService.Exception
{
    public class ShakespeareTranslationServiceException : System.Exception
    {
        public ShakespeareTranslationServiceException(string message, System.Exception exception)
            : base(message, exception)
        {
        }
    }
}