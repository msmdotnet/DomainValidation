namespace DomainValidation.Tests.PropertySpecificationTreeExtensionsTests;
public class IsRequiredTests
{
    Expression<Func<CreateOrder, string>> PropertyExpression =
        x => x.CustomerId;

    [Theory]
    [InlineData(null, false)]
    [InlineData("   ", false)]
    [InlineData("ALFKI", true)]
    public void IsRequired_ShouldReturnExpectedResult_WhenValueIsChecked(
        string customerId, bool expectedResult)
    {
        // Arrange
        var Tree = new PropertySpecificationsTree<CreateOrder, string>(
            PropertyExpression);

        Tree.IsRequired();

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

    [Fact]
    public void IsRequired_ShouldUseProvidedErrorMessage_WhenValidationFails()
    {
        // Arrange
        var Tree = new PropertySpecificationsTree<CreateOrder, string>(
            PropertyExpression);
        string ExpectedErrorMessage = CreateOrder.CustomerIdIsRequired;

        Tree.IsRequired(ExpectedErrorMessage);

        var Entity = new CreateOrder { CustomerId = null };

        // Act
        bool Result = Tree.Specifications[0].IsSatisfiedBy(Entity);

        // Assert
        Assert.False(Result);
        Assert.Equal(ExpectedErrorMessage,
            Tree.Specifications[0].Errors.First().ErrorMessage);
    }

    [Fact]
    public void IsRequired_ShouldUseDefaultErrorMessage_WhenValidationFails()
    {
        // Arrange
        var Tree = new PropertySpecificationsTree<CreateOrder, string>(
            PropertyExpression);
        string ExpectedErrorMessage = ErrorMessages.IsRequired;

        Tree.IsRequired();

        var Entity = new CreateOrder { CustomerId = null };

        // Act
        bool Result = Tree.Specifications[0].IsSatisfiedBy(Entity);

        // Assert
        Assert.False(Result);
        Assert.Equal(ExpectedErrorMessage,
            Tree.Specifications[0].Errors.First().ErrorMessage);
    }

}

