namespace TheProject.BuildingBlocks.Results;

public static class ResultExtensions
{

    extension(Result result)
    {
        public TOut Match<TOut>(Func<TOut> onSuccess, Func<Error, TOut> onFailure)
        {
            return result.IsSuccess ? onSuccess()
                                    : onFailure(result.Error);
        }
    }

    extension<T>(Result<T> result)
    {
        public TOut Match<TOut>(Func<T, TOut> onSuccess, Func<Error, TOut> onFailure)
        {
            return result.IsSuccess ? onSuccess(result.Value)
                                    : onFailure(result.Error);
        }
    }
}

