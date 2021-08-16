using System.Collections.Generic;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Routing;
using Moq;
using Pokedex.Api.Filters;
using Xunit;

namespace Pokedex.Api.UnitTests.Filters
{
    public class ValidationActionFilterTests
    {
        private readonly ValidationActionFilter _filter;
        private readonly ActionContext _actionContext;
        
        public ValidationActionFilterTests()
        {
            _filter = new ValidationActionFilter();

            var modelState = new ModelStateDictionary();
            modelState.AddModelError("PokemonName", "PokemonName is invalid");
            
            var httpContext = new Mock<HttpContext>();
            var routeData = new Mock<RouteData>();
            var actionDescriptor = new Mock<ActionDescriptor>();
            _actionContext = new ActionContext(httpContext.Object, routeData.Object, actionDescriptor.Object, modelState);
            
        }

        [Fact]
        public void ShouldReturnBadRequestObjectResultWhenModelStateIsInvalid()
        {
            // Arrange
            var actionExecutingContext = new ActionExecutingContext(
                _actionContext,
                new List<IFilterMetadata>(),
                new Dictionary<string, object>(),
                new Mock<Controller>().Object
            );
            
            // Act
            _filter.OnActionExecuting(actionExecutingContext);

            // Assert
            actionExecutingContext.Result.Should().BeOfType<BadRequestObjectResult>();
        }
    }
}