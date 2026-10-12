using TheProject.BuildingBlocks.Results;

namespace TheProject.Products.Api.Domain;

public sealed class Product
{
    public const int NAME_MAX_LENGTH = 200;
    public Guid Id { get; private set; }
    public Sku Sku { get; private set; }
    public string Name { get; private set; }
    public Money Price { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset UpdatedAt { get; private set; }


    private Product(Guid id, Sku sku, string name, Money price, DateTimeOffset createdAt)
    {
        Id = id;
        Sku = sku;
        Name = name;
        Price = price;
        CreatedAt = createdAt;
        UpdatedAt = createdAt;
    }

    public static Result<Product> Create(Sku sku, string name, Money price, DateTimeOffset now)
    {
        var error = Validate(name, price);
        if (error is not null)
        {
            return error;
        }

        return new Product(Guid.CreateVersion7(now), sku, name.Trim(), price, now);
    }

    public Result Update(string name, Money price, DateTimeOffset now)
    {
        var error = Validate(name, price);
        if (error is not null)
        {
            return error;
        }
        Name = name.Trim();
        Price = price;
        UpdatedAt = now;

        return Result.Success();
    }






    private static Error? Validate(string name, Money price)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return ProductErrors.NameRequired();
        }

        if (name.Length > NAME_MAX_LENGTH)
        {
            return ProductErrors.NameTooLong();
        }

        if (price.Amount <= 0)
        {
            return ProductErrors.PriceNotPositive(price.Amount);
        }

        return null;
    }

}