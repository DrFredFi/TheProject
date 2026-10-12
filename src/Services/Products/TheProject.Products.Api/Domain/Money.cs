using TheProject.BuildingBlocks.Results;


namespace TheProject.Products.Api.Domain;

public sealed record Money
{
    public decimal Amount { get; }

    public string Currency { get; }

    private Money(decimal amount, string currency)
    {
        this.Amount = amount;
        this.Currency = currency;
    }

    public static Result<Money> Create(decimal amount, string currency)
    {


        if (decimal.Round(amount, 2) != amount)
        {
            return ProductErrors.PriceTooPrecise(amount);
        }
        if (currency is not { Length: 3 } || !currency.All(char.IsAsciiLetterUpper))
        {
            return ProductErrors.CurrencyInvalid(currency);
        }

        return new Money(amount, currency);

    }

}