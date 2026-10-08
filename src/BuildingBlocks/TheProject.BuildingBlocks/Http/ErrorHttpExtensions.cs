using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

using TheProject.BuildingBlocks.Results;

namespace TheProject.BuildingBlocks.Http;

public static class ErrorHttpExtensions
{
    extension(Error error)
    {

        public ProblemHttpResult ToProblem() => TypedResults.Problem(
            detail: error.Description,
            statusCode: error.Type switch
            {
                ErrorType.Validation => StatusCodes.Status400BadRequest,
                ErrorType.NotFound => StatusCodes.Status404NotFound,
                ErrorType.Conflict => StatusCodes.Status409Conflict,
                ErrorType.Failure => StatusCodes.Status500InternalServerError,
                _ => throw new ArgumentOutOfRangeException(nameof(error), error.Type, "Unmapped error type.")
            },
            extensions: new Dictionary<string, object?> { ["code"] = error.Code }
            );
    }
}
