using TheProject.Products.Api.Domain;

namespace TheProject.Products.Tests.Unit.Domain;

public class MoneyTests
{
    [Fact]
    public void Valid_money_keeps_amount_and_currency()
    {
        var money = Money.Create(19.99m, "EUR").Value;

        money.Amount.Should().Be(19.99m);
        money.Currency.Should().Be("EUR");
    }

    [Fact]
    public void More_than_two_decimals_is_rejected() =>
        Money.Create(1.999m, "EUR").Error.Should().Be(ProductErrors.PriceTooPrecise(1.999m));

    [Theory]
    [InlineData("eur")]
    [InlineData("EURO")]
    [InlineData("E1R")]
    public void Currency_must_be_three_upper_case_letters(string currency) =>
        Money.Create(10m, currency).Error.Should().Be(ProductErrors.CurrencyInvalid(currency));

    [Fact]
    public void Money_with_the_same_amount_and_currency_is_equal() =>
        Money.Create(10m, "EUR").Value.Should().Be(Money.Create(10.00m, "EUR").Value);
}