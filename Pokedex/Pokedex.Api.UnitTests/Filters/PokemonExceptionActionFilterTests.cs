using System;
using System.Collections.Generic;
using System.Net;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Moq;
using Pokedex.Api.Filters;
using Pokedex.Api.Models.v1.Errors;
using Pokedex.Services.PokemonService.Exception;
using Pokedex.Services.TranslationService.Common.Exception;
using Xunit;

namespace Pokedex.Api.UnitTests.Filters
{
    public class PokemonExceptionActionFilterTests
    {
        private readonly PokemonExceptionActionFilter _filter;
        private readonly ActionContext _actionContext;
        
        public PokemonExceptionActionFilterTests()
        {
            _filter = new PokemonExceptionActionFilter();
            
            var httpContext = new Mock<HttpContext>();
            var routeData = new Mock<RouteData>();
            var actionDescriptor = new Mock<ActionDescriptor>();
            _actionContext = new ActionContext(httpContext.Object, routeData.Object, actionDescriptor.Object);
        }

        [Fact]
        public void ShouldReturnNotFoundObjectResultForPokemonNotFoundException()
        {
            // Arrange
            var actionExecutedContext = new ActionExecutedContext(_actionContext, new List<IFilterMetadata>(),
                new Mock<Controller>().Object)
            {
                Exception = new PokemonNotFoundException("error")
            };

            // Act
            _filter.OnActionExecuted(actionExecutedContext);

            // Assert
            actionExecutedContext.ExceptionHandled.Should().BeTrue();
            actionExecutedContext.Result.Should().BeOfType<NotFoundObjectResult>();
        }
        
        [Fact]
        public void ShouldReturnNoContentResultForPokemonNoContentException()
        {
            // Arrange
            var actionExecutedContext = new ActionExecutedContext(_actionContext, new List<IFilterMetadata>(),
                new Mock<Controller>().Object)
            {
                Exception = new PokemonNoContentException()
            };

            // Act
            _filter.OnActionExecuted(actionExecutedContext);

            // Assert
            actionExecutedContext.ExceptionHandled.Should().BeTrue();
            actionExecutedContext.Result.Should().BeOfType<NoContentResult>();
        }
        
        [Fact]
        public void ShouldReturnJsonResultWithInternalServerErrorResponseForPokemonServiceException()
        {
            // Arrange
            var actionExecutedContext = new ActionExecutedContext(_actionContext, new List<IFilterMetadata>(),
                new Mock<Controller>().Object)
            {
                Exception = new PokemonServiceException("error", It.IsAny<Exception>())
            };

            // Act
            _filter.OnActionExecuted(actionExecutedContext);

            // Assert
            actionExecutedContext.ExceptionHandled.Should().BeTrue();
            actionExecutedContext.Result.Should().BeOfType<JsonResult>();
            actionExecutedContext.Result.As<JsonResult>().Value.Should().BeOfType<InternalServerErrorResponse>();
            var response = actionExecutedContext.Result.As<JsonResult>().Value.As<InternalServerErrorResponse>();
            response.Title.Should().Be("Internal Server Error");
            response.Detail.Should().Be("Error encountered while trying to retrieve Pokemon Information");
            response.Status.Should().Be((int)HttpStatusCode.InternalServerError);
        }
        
        [Fact]
        public void ShouldReturnJsonResultWithInternalServerErrorResponseForTranslationServiceException()
        {
            // Arrange
            var actionExecutedContext = new ActionExecutedContext(_actionContext, new List<IFilterMetadata>(),
                new Mock<Controller>().Object)
            {
                Exception = new TranslationServiceException("error", It.IsAny<Exception>())
            };

            // Act
            _filter.OnActionExecuted(actionExecutedContext);

            // Assert
            actionExecutedContext.ExceptionHandled.Should().BeTrue();
            actionExecutedContext.Result.Should().BeOfType<JsonResult>();
            actionExecutedContext.Result.As<JsonResult>().Value.Should().BeOfType<InternalServerErrorResponse>();
            var response = actionExecutedContext.Result.As<JsonResult>().Value.As<InternalServerErrorResponse>();
            response.Title.Should().Be("Internal Server Error");
            response.Detail.Should().Be("Error encountered while trying to retrieve Pokemon Information");
            response.Status.Should().Be((int)HttpStatusCode.InternalServerError);
        }
    }
}