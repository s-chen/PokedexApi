namespace Pokedex.Services.PokemonService.Exception
{
    public class PokemonServiceException : System.Exception
    {
        public PokemonServiceException(string message, System.Exception exception)
            : base(message, exception)
        {
        }
    }
}