using Xunit;
using CleanArchitecture.Domain.Entities;

namespace CleanArchitecture.Domain.Tests.Entities;

public class ProductTests
{
    [Fact]
    public void Product_ShouldInheritFromBaseEntity()
    {
        // Arrange & Act
        var product = new Product();

        // Assert
        Assert.IsAssignableFrom<BaseEntity>(product);
    }

    [Fact]
    public void Product_ShouldHaveDefaultValues()
    {
        // Arrange & Act
        var product = new Product();

        // Assert
        Assert.Equal(Guid.Empty, product.Id);
        Assert.Equal(string.Empty, product.Name);
        Assert.Equal(string.Empty, product.Description);
        Assert.Equal(0, product.Price);
        Assert.Equal(0, product.StockQuantity);
        Assert.True(product.IsActive);
        Assert.Equal(string.Empty, product.Category);
    }

    [Fact]
    public void Product_ShouldSetPropertiesCorrectly()
    {
        // Arrange
        var id = Guid.NewGuid();
        var name = "Test Product";
        var description = "Test Description";
        var price = 99.99m;
        var stockQuantity = 10;
        var category = "Electronics";

        // Act
        var product = new Product
        {
            Id = id,
            Name = name,
            Description = description,
            Price = price,
            StockQuantity = stockQuantity,
            Category = category
        };

        // Assert
        Assert.Equal(id, product.Id);
        Assert.Equal(name, product.Name);
        Assert.Equal(description, product.Description);
        Assert.Equal(price, product.Price);
        Assert.Equal(stockQuantity, product.StockQuantity);
        Assert.Equal(category, product.Category);
    }
}