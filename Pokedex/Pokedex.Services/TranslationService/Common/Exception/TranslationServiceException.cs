namespace Pokedex.Services.TranslationService.Common.Exception
{
    public class TranslationServiceException : System.Exception
    {
        public TranslationServiceException(string message, System.Exception exception)
            : base(message, exception)
        {
        }
    }
}