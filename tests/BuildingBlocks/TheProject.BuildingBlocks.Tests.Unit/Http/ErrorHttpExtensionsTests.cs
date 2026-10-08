using TheProject.BuildingBlocks.Http;
using TheProject.BuildingBlocks.Results;

namespace TheProject.BuildingBlocks.Tests.Unit.Http;

public class ErrorHttpExtensionsTests
{
    [Theory]
    [InlineData(ErrorType.Validation, 400)]
    [InlineData(ErrorType.NotFound, 404)]
    [InlineData(ErrorType.Conflict, 409)]
    [InlineData(ErrorType.Failure, 500)]
    public void Error_type_decides_the_status_code(ErrorType type, int expectedStatus)
    {
        var problem = new Error("Things.Code", "Something happened.", type).ToProblem();

        problem.StatusCode.Should().Be(expectedStatus);
        problem.ProblemDetails.Detail.Should().Be("Something happened.");
        problem.ProblemDetails.Extensions["code"].Should().Be("Things.Code");
    }
}