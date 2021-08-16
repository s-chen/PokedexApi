using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Flurl.Http.Configuration;
using Flurl.Http.Testing;
using Microsoft.Extensions.Options;
using Moq;
using Pokedex.Services.TranslationService.Common.Exception;
using Pokedex.Services.TranslationService.Common.Options;
using Pokedex.Services.TranslationService.YodaTranslationService;
using Xunit;

namespace Pokedex.Services.UnitTests.TranslationService.YodaTranslationService
{
    public class YodaTranslationServiceTests
    {
        private readonly IYodaTranslationService _service;
        private readonly Mock<IOptions<TranslationServiceOptions>> _mockOptions;

        public YodaTranslationServiceTests()
        {
            _mockOptions = new Mock<IOptions<TranslationServiceOptions>>();
            _mockOptions.Setup(x => x.Value).Returns(new TranslationServiceOptions() {Host = "http://testapi.com"});

            var flurlClientFactory = new DefaultFlurlClientFactory();
            _service = new Services.TranslationService.YodaTranslationService.YodaTranslationService(flurlClientFactory,
                _mockOptions.Object);
        }

        [Fact]
        public async Task GetTranslationAsyncShouldReturnTranslatedResponse()
        {
            using (var httpTest = new HttpTest())
            {
                // Arrange
                httpTest.RespondWith(LoadJson("translationSuccess.json"), 200);

                // Act
                var result = await _service.GetTranslationAsync("text", CancellationToken.None);

                // Assert
                result.Should().NotBeNull();
                result.StandardDescription.Should().Be("text");
                result.TranslatedText.Should().Be("translatedText");
            }
        }
        
        [Fact]
        public async Task GetTranslationAsyncShouldReturnEmptyTranslatedResponseWhenContentIsEmpty()
        {
            using (var httpTest = new HttpTest())
            {
                // Arrange
                httpTest.RespondWith(LoadJson("translationNoContent.json"), 200);
                
                // Act
                var result = await _service.GetTranslationAsync("text", CancellationToken.None);
                
                // Assert
                result.Should().NotBeNull();
                result.StandardDescription.Should().BeNullOrWhiteSpace();
                result.TranslatedText.Should().BeNullOrWhiteSpace();
            }
        }

        [Theory]
        [InlineData(500)]
        [InlineData(503)]
        public void GetTranslationAsyncShouldThrowTranslationServiceExceptionForErrorStatus(int status)
        {
            using (var httpTest = new HttpTest())
            {
                // Arrange
                httpTest.RespondWith(string.Empty, status);

                // Act
                Func<Task> func = async () => await _service.GetTranslationAsync("text", CancellationToken.None);

                // Assert
                func.Should().ThrowAsync<TranslationServiceException>();
            }
        }

        private string LoadJson(string fileName)
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), $"TranslationService/Responses/{fileName}");
            using (StreamReader r = new StreamReader(filePath))
            {
                string json = r.ReadToEnd();
                return json;
            }
        }
    }
}