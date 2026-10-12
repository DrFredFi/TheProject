using System.Text.RegularExpressions;
using TheProject.BuildingBlocks.Results;

namespace TheProject.Products.Api.Domain;

public sealed partial record Sku
{
    public const int MaxLength = 32;
    public string Value { get; }

    private Sku(string value) => Value = value.Trim().ToUpperInvariant();
    public static Result<Sku> Create(string Value)
    {
        var normalized = Value.Trim().ToUpperInvariant();
        return normalized.Length <= MaxLength && Format().IsMatch(normalized)
            ? new Sku(normalized)
            : ProductErrors.SkuInvalid(Value);

    }

    public override string ToString() => Value;

    [GeneratedRegex("^[A-Z0-9]+(-[A-Z0-9]+)*$")]
    private static partial Regex Format();

}