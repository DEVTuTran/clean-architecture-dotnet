using Xunit;
using CleanArchitecture.Domain.Entities;

namespace CleanArchitecture.Domain.Tests.Entities;

public class OrganizationTests
{
    [Fact]
    public void Product_ShouldInheritFromBaseEntity()
    {
        // Arrange & Act
        var organization = new Organization();

        // Assert
        Assert.IsAssignableFrom<BaseEntity>(organization);
    }

    [Fact]
    public void Product_ShouldHaveDefaultValues()
    {
        // Arrange & Act
        var organization = new Organization();

        // Assert
        // Assert.Equal(organization.Id, Guid.Empty);
    }

    [Fact]
    public void Organization_ShouldSetPropertiesCorrectly()
    {
        // Arrange
        var id = 1;
        var name = "Test";
        var code = "Test";
        var statusId = 2;
        var maxPaymentUsers = 2;
        var paymentFeeId = 10;
        var useIpWhitelist = false;
        var isDeleted = false;

        // Act
        var organization = new Organization
        {
            Id = id,
            Name = name,
            Code = code,
            StatusId = statusId,
            MaxPaymentUsers = maxPaymentUsers,
            PaymentFeeId = paymentFeeId,
            UseIpWhitelist = useIpWhitelist,
            IsDeleted = isDeleted
        };

        // Assert
        Assert.Equal(id, organization.Id);
        Assert.Equal(name, organization.Name);
        Assert.Equal(code, organization.Code);
        Assert.Equal(statusId, organization.StatusId);
        Assert.Equal(maxPaymentUsers, organization.MaxPaymentUsers);
        Assert.Equal(paymentFeeId, organization.PaymentFeeId);
        Assert.Equal(useIpWhitelist, organization.UseIpWhitelist);
        Assert.Equal(isDeleted, organization.IsDeleted);
    }
}