using TheProject.BuildingBlocks.Results;

namespace TheProject.Products.Api.Domain;

public static class ProductErrors
{
    public static Error SkuInvalid(string sku) => Error.Validation("Products.SkuInvalid",
        $"SKU '{sku}' must be 1-{Sku.MaxLength} letters, digits or single dashes between them.");
    public static Error NameRequired() => Error.Validation("Products.NameRequired", "Name is required");
    public static Error NameTooLong() => Error.Validation("Products.NameTooLong",
        $"Name is too long it must be at most {Sku.MaxLength} characters");
    public static Error PriceNotPositive(decimal amount) => Error.Validation("Products.PriceNotPositive",
        "Price must be a positive number");
    public static Error PriceTooPrecise(decimal amount) => Error.Validation("Products.PriceTooPrecise",
        $"Price must have at most two decimal places, but was {amount}.");
    public static Error CurrencyInvalid(string currency) => Error.Validation("Products.CurrencyInvalid",
        $"The currency '{currency}' must be a three-letter ISO 4217 code in upper case.");

}