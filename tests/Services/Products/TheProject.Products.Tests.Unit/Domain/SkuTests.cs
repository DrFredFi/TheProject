using TheProject.Products.Api.Domain;

namespace TheProject.Products.Tests.Unit.Domain;

public class SkuTests
{
    [Theory]
    [InlineData("abc-123", "ABC-123")]
    [InlineData("  KB-01 ", "KB-01")]
    public void Valid_sku_is_trimmed_and_upper_cased(string input, string expected) =>
        Sku.Create(input).Value.Value.Should().Be(expected);

    [Theory]
    [InlineData("")]
    [InlineData("-AB")]
    [InlineData("AB--1")]
    [InlineData("AB 1")]
    [InlineData("ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456")]
    public void Invalid_sku_is_rejected(string input) =>
        Sku.Create(input).Error.Should().Be(ProductErrors.SkuInvalid(input));

    [Fact]
    public void Skus_with_the_same_value_are_equal() =>
        Sku.Create("kb-01").Value.Should().Be(Sku.Create("KB-01").Value);
}