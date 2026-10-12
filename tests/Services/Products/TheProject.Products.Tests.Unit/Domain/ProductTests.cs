using TheProject.Products.Api.Domain;

namespace TheProject.Products.Tests.Unit.Domain;

public class ProductTests
{
    private static readonly DateTimeOffset Now = new(2026, 10, 8, 9, 0, 0, TimeSpan.Zero);
    private static readonly Sku Sku = Sku.Create("KB-01").Value;
    private static readonly Money Price = Money.Create(49.90m, "EUR").Value;

    [Fact]
    public void Create_sets_state_and_both_timestamps()
    {
        var product = Product.Create(Sku, "  Keyboard ", Price, Now).Value;

        product.Id.Version.Should().Be(7);
        product.Sku.Should().Be(Sku);
        product.Name.Should().Be("Keyboard");
        product.Price.Should().Be(Price);
        product.CreatedAt.Should().Be(Now);
        product.UpdatedAt.Should().Be(Now);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Blank_name_is_rejected(string name) =>
        Product.Create(Sku, name, Price, Now).Error.Should().Be(ProductErrors.NameRequired());

    [Fact]
    public void Name_longer_than_the_limit_is_rejected() =>
        Product.Create(Sku, new string('x', Product.NAME_MAX_LENGTH + 1), Price, Now).Error
            .Should().Be(ProductErrors.NameTooLong());

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Price_must_be_positive(decimal amount) =>
        Product.Create(Sku, "Keyboard", Money.Create(amount, "EUR").Value, Now).Error
            .Should().Be(ProductErrors.PriceNotPositive(amount));

    [Fact]
    public void Update_changes_name_price_and_only_the_updated_timestamp()
    {
        var product = Product.Create(Sku, "Keyboard", Price, Now).Value;
        var later = Now.AddHours(1);
        var newPrice = Money.Create(39.90m, "EUR").Value;

        var result = product.Update("Mechanical keyboard", newPrice, later);

        result.IsSuccess.Should().BeTrue();
        product.Name.Should().Be("Mechanical keyboard");
        product.Price.Should().Be(newPrice);
        product.CreatedAt.Should().Be(Now);
        product.UpdatedAt.Should().Be(later);
    }

    [Fact]
    public void Rejected_update_leaves_the_product_unchanged()
    {
        var product = Product.Create(Sku, "Keyboard", Price, Now).Value;

        var result = product.Update(" ", Price, Now.AddHours(1));

        result.Error.Should().Be(ProductErrors.NameRequired());
        product.Name.Should().Be("Keyboard");
        product.UpdatedAt.Should().Be(Now);
    }
}