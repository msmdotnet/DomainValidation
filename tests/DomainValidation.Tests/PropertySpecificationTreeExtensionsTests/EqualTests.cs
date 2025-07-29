namespace DomainValidation.Tests.PropertySpecificationTreeExtensionsTests;
public class EqualTests
{
    [Theory]
    [InlineData(0, 1, false)]
    [InlineData(1, 1, true)]
    public void Equal_ShouldReturnExpectedResult_WhenNumericValueIsChecked(
        int productId, int comparisonValue, bool expectedResult)
    {
        // Arrange        
        var Tree = new PropertySpecificationsTree<CreateOrderDetail, int>(
            x => x.ProductId);
        Tree.Equal(comparisonValue);

        var Entity = new CreateOrderDetail { ProductId = productId };

        // Act
        bool Result = Tree.Specifications[0].IsSatisfiedBy(Entity);

        // Assert
        Assert.Equal(expectedResult, Result);

        if (!expectedResult)
        {
            Assert.Single(Tree.Specifications[0].Errors);
        }
        else
        {
            Assert.True(Tree.Specifications[0].Errors == null ||
                !Tree.Specifications[0].Errors.Any());
        }
    }

    [Theory]
    [InlineData(null, "ALFKI", false)]
    [InlineData("ALFKI", null, false)]
    [InlineData("ALF", "ALFKI", false)]
    [InlineData(null, null, true)]
    [InlineData("", "", true)]
    [InlineData("ALFKI", "ALFKI", true)]
    public void Equal_ShouldReturnExpectedResult_WhenStringValueIsChecked(
        string customerId, string comparisonValue, bool expectedResult)
    {
        // Arrange        
        var Tree = new PropertySpecificationsTree<CreateOrder, string>(
            x => x.CustomerId);
        Tree.Equal(comparisonValue);

        var Entity = new CreateOrder { CustomerId = customerId };

        // Act
        bool Result = Tree.Specifications[0].IsSatisfiedBy(Entity);

        // Assert
        Assert.Equal(expectedResult, Result);

        if (!expectedResult)
        {
            Assert.Single(Tree.Specifications[0].Errors);
        }
        else
        {
            Assert.True(Tree.Specifications[0].Errors == null ||
                !Tree.Specifications[0].Errors.Any());
        }
    }

    [Theory]
    [InlineData(null, "Mm1234567$", false)]
    [InlineData("Mm1234567$", null, false)]
    [InlineData("Mm1234567$", "Mm1234567", false)]
    [InlineData(null, null, true)]
    [InlineData("", "", true)]
    [InlineData("Mm1234567$", "Mm1234567$", true)]
    public void Equal_ShouldReturnExpectedResult_WhenPropertiesAreChecked(
        string password, string confirmPassword, bool expectedResult)
    {
        // Arrange        
        var Tree = new PropertySpecificationsTree<UserRegistration, string>(
            x => x.Password);

        Tree.Equal(x => x.ConfirmPassword);

        var Entity = new UserRegistration
        {
            Password = password,
            ConfirmPassword = confirmPassword
        };

        // Act
        bool Result = Tree.Specifications[0].IsSatisfiedBy(Entity);

        // Assert
        Assert.Equal(expectedResult, Result);

        if (!expectedResult)
        {
            Assert.Single(Tree.Specifications[0].Errors);
        }
        else
        {
            Assert.True(Tree.Specifications[0].Errors == null ||
                !Tree.Specifications[0].Errors.Any());
        }
    }

    [Fact]
    public void Equal_ShouldUseProvidedErrorMessage_WhenValidationFails()
    {
        // Arrange
        var Tree = new PropertySpecificationsTree<UserRegistration, string>(
            x => x.ConfirmPassword);
        string ExpectedMessage =
            UserRegistration.PasswordConfirmationDoesNotMatchErrorMessage;

        Tree.Equal(x => x.Password, ExpectedMessage);

        var Entity = new UserRegistration
        {
            Password = "Mm1234567$",
            ConfirmPassword = "Mm1234567"
        };

        // Act
        bool Result = Tree.Specifications[0].IsSatisfiedBy(Entity);

        // Assert
        Assert.False(Result);
        Assert.Equal(ExpectedMessage,
            Tree.Specifications[0].Errors.First().ErrorMessage);
    }

    [Fact]
    public void Equal_ShouldUseDefaultErrorMessage_WhenValidationFails()
    {
        // Arrange
        var Tree = new PropertySpecificationsTree<UserRegistration, string>(
            x => x.ConfirmPassword);
        string ExpectedMessage = ErrorMessages.Equal;

        Tree.Equal(x => x.Password);

        var Entity = new UserRegistration
        {
            Password = "Mm1234567$",
            ConfirmPassword = "Mm1234567"
        };

        // Act
        bool Result = Tree.Specifications[0].IsSatisfiedBy(Entity);

        // Assert
        Assert.False(Result);
        Assert.Equal(ExpectedMessage,
            Tree.Specifications[0].Errors.First().ErrorMessage);
    }
}

