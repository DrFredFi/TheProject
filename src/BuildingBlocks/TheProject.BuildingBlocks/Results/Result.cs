using System.Diagnostics.CodeAnalysis;

namespace TheProject.BuildingBlocks.Results;


public class Result
{
    public Error? Error { get; }

    [MemberNotNullWhen(false, nameof(Error))]
    public bool IsSuccess => Error is null;
    [MemberNotNullWhen(true, nameof(Error))]
    public bool IsFailure => !IsSuccess;

    public static Result Success() => new();
    public static Result Failure(Error error) => new(error);

    protected Result()
    {
    }
    protected Result(Error error)
    {
        ArgumentNullException.ThrowIfNull(error);
        this.Error = error;
    }

    public static implicit operator Result(Error error) => Failure(error);

}

public sealed class Result<T> : Result
{
    private readonly T? value;
    public T Value => IsFailure ? throw new InvalidOperationException($"Cannot read the value of a failed result({Error.Code})")
                                  : value!;
    private Result(T value) => this.value = value;
    private Result(Error error) : base(error) { }



    public static Result<T> Success(T value) => new(value);
    public static new Result<T> Failure(Error error) => new(error);

    public static implicit operator Result<T>(T value) => Success(value);
    public static implicit operator Result<T>(Error error) => Failure(error);



}