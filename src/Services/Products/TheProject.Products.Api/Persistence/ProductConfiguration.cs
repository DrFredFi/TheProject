using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TheProject.Products.Api.Domain;

namespace TheProject.Products.Api.Persistence;

internal sealed class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id).ValueGeneratedNever();
        builder.Property(p => p.Name).HasMaxLength(Product.NAME_MAX_LENGTH);

        builder.Property(p => p.Sku)
            .HasConversion(sku => sku.Value, value => Sku.Create(value).Value)
            .HasMaxLength(Sku.MaxLength);
        builder.HasIndex(p => p.Sku).IsUnique();

        builder.ComplexProperty(p => p.Price, price =>
        {
            price.Property(m => m.Amount).HasPrecision(18, 2);
            price.Property(m => m.Currency).HasMaxLength(3).IsFixedLength();
        });


        builder.Property<uint>(ProductsDbContext.VersionProperty).IsRowVersion();

    }
}