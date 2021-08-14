using System;
using Pokedex.Services.TranslationService.Common.Enums;
using Pokedex.Services.TranslationService.Common.Model;
using Pokedex.Services.TranslationService.Common.Schema;

namespace Pokedex.Services.TranslationService.Common.Mappers
{
    public static class TranslationMapper
    {
        public static TranslatedResponse Map(TranslatedServiceResponse response)
        {
            if (response?.Contents == null)
            {
                return new TranslatedResponse();
            }

            Enum.TryParse<TranslationType>(response.Contents.Translation, out var translationType);

            return new TranslatedResponse
            {
                StandardDescription = response.Contents.Text,
                TranslatedText = response.Contents.Translated,
                TranslationType = translationType
            };
        }
    }
}