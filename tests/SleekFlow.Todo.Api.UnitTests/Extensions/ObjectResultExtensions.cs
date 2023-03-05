using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SleekFlow.Todo.Api.UnitTests.Extensions;

internal static class ObjectResultExtensions
{
	public static void ShouldBeOkObjectResult(this IActionResult actionResult, Type expectedBodyType, object expectedBody)
	{
		actionResult.ShouldBeOfType<OkObjectResult>();

		OkObjectResult responseObject = (actionResult as OkObjectResult)!;

		responseObject.Value.ShouldNotBeNull();
		responseObject.Value.ShouldBeOfType(expectedBodyType);
		responseObject.Value.ShouldBeEquivalentTo(expectedBody);
	}

	public static void ShouldBeBadRequestObjectResult(this IActionResult actionResult, Type expectedBodyType, object expectedBody)
	{
		actionResult.ShouldNotBeNull();
		actionResult.ShouldBeOfType<BadRequestObjectResult>();

		BadRequestObjectResult responseObject = (actionResult as BadRequestObjectResult)!;
		responseObject.Value.ShouldNotBeNull();
		responseObject.Value.ShouldBeOfType(expectedBodyType);
		responseObject.Value.ShouldBeEquivalentTo(expectedBody);
	}

	public static void ShouldBeNotFoundObjectResult(this IActionResult actionResult, Type expectedBodyType, object expectedBody)
	{
		actionResult.ShouldNotBeNull();
		actionResult.ShouldBeOfType<NotFoundObjectResult>();

		NotFoundObjectResult responseObject = (actionResult as NotFoundObjectResult)!;
		responseObject.Value.ShouldNotBeNull();
		responseObject.Value.ShouldBeOfType(expectedBodyType);
		responseObject.Value.ShouldBeEquivalentTo(expectedBody);
	}

	public static void ShouldBeInternalServerErrorObjectResult(this IActionResult actionResult, Type expectedBodyType, object expectedBody)
	{
		actionResult.ShouldNotBeNull();
		actionResult.ShouldBeOfType<ObjectResult>();

		ObjectResult responseObject = (actionResult as ObjectResult)!;
		responseObject.StatusCode.ShouldBe(StatusCodes.Status500InternalServerError);
		responseObject.Value.ShouldNotBeNull();
		responseObject.Value.ShouldBeOfType(expectedBodyType);
		responseObject.Value.ShouldBeEquivalentTo(expectedBody);
	}
}