using System;
using System.Collections.Generic;
using System.Text;

using TheProject.BuildingBlocks.Results;

namespace TheProject.BuildingBlocks.Tests.Unit.Results;

public class ResultTests
{
    private static readonly Error someError = Error.Failure("c", "d");

    public ResultTests()
    {

    }


    [Fact]
    public void WhenSuccess_Result_ShouldHaveValue()
    {
        //ARRANGE
        //ACT
        Result<int> result = 42;

        //ASSERT
        result.Value.Should().Be(42);
        result.IsSuccess.Should().BeTrue();
        result.Error.Should().BeNull();
    }

    [Fact]
    public void WhenError_Result_ShouldHaveError()
    {
        //ARRANGE
        //ACT
        Result<int> result = Error.Failure("code", "Description");

        //ASSERT
        result.IsFailure.Should().BeTrue();
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().NotBeNull();

    }

    [Fact]
    public void WhenAccess_Value_Null_ShouldThrowException()
    {
        //ARRANGE
        Result<int> result = Error.Failure("", "");
        //ACT
        var valore = () => result.Value;
        //ASSERT
        valore.Should().Throw<InvalidOperationException>()
            .WithMessage($"Cannot read the value of a failed result({result.Error!.Code})");

    }


    [Fact]
    public void WhenResultImplicitOperator_Works_ShouldCreateCorrectResult()
    {
        //ARRANGE
        Result result = someError;

        //ASSERT
        result.IsFailure.Should().BeTrue();
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().NotBeNull();
    }

    [Fact]
    public void WhenSuccessGenericResultImplicitOperator_Works_ShouldCreateCorrectResult()
    {
        //ARRANGE
        Result<int> result = 42;

        //ASSERT
        result.IsFailure.Should().BeFalse();
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(42);
        result.Error.Should().BeNull();
    }


    [Fact]
    public void WhenErrorGenericResultImplicitOperator_Works_ShouldCreateCorrectResult()
    {
        //ARRANGE
        Result<int> result = someError;

        //ASSERT
        result.IsFailure.Should().BeTrue();
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().NotBeNull();
        result.Error.Code.Should().Be("c");
        result.Error.Description.Should().Be("d");
    }


    [Theory]
    [InlineData(true, "ok:7")]
    [InlineData(false, "error:d")]
    public void WhenMatch_takes_the_branch_matching_the_outcome(bool succeed, string expected)
    {
        Result<int> result = succeed ? 7 : someError;

        var text = result.Match(value => $"ok:{value}", error => $"error:{error.Description}");

        text.Should().Be(expected);
    }

    [Fact]
    public void WhenSuccess_Match_ShouldReturnSuccessBranch()
    {
        var text = Result<int>.Success(42).Match((value) => $"{value}", (error) => $"{error.Code}");

        text.Should().Be("42");

    }

    [Theory]
    [MemberData(nameof(Factories))]
    public void WhenErrorFactory_Used_ShouldReturnRightType(Error error, ErrorType errorType)
    {
        error.Type.Should().Be(errorType);
    }


    public static TheoryData<Error, ErrorType> Factories => new()
    {
        {Error.Failure("c","d"),ErrorType.Failure },
        {Error.Conflict("c","d"),ErrorType.Conflict },
        {Error.NotFound("c","d"),ErrorType.NotFound },
        {Error.Validation("c","d"),ErrorType.Validation},
    };


}
