namespace DomainValidation.Tests.PropertySpecificationTreeExtensionsTests;
public class HasMaxLengthTests
{
    Expression<Func<CreateOrder, string>> PropertyExpression =
        (x => x.CustomerId);

    [Theory]
    [InlineData("ALFKIS", 5, false)]
    [InlineData(null, 5, true)]
    [InlineData("", 5, true)]
    [InlineData("ALF", 5, true)]
    [InlineData("ALFKI", 5, true)]
    public void HasMaxLength_ShouldReturnExpectedResult_WhenValueIsChecked(
        string customerId, int length, bool expectedResult)
    {
        // Arrange        
        var Tree = new PropertySpecificationsTree<CreateOrder, string>(
            PropertyExpression);
        Tree.HasMaxLength(length);

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
    public void HasMaxLength_ShouldUseProvidedErrorMessage_WhenValidationFails()
    {
        // Arrange
        var Tree = new PropertySpecificationsTree<CreateOrder, string>(
            PropertyExpression);
        string ExpectedMessage = CreateOrder.CustomerIdHasMaxLength;

        Tree.HasMaxLength(5, ExpectedMessage);

        var Entity = new CreateOrder { CustomerId = "ALFKIS" };

        // Act
        bool Result = Tree.Specifications[0].IsSatisfiedBy(Entity);

        // Assert
        Assert.False(Result);
        Assert.Equal(ExpectedMessage,
            Tree.Specifications[0].Errors.First().ErrorMessage);
    }

    [Fact]
    public void HasMaxLength_ShouldUseDefaultErrorMessage_WhenValidationFails()
    {
        // Arrange
        var Tree = new PropertySpecificationsTree<CreateOrder, string>(
            PropertyExpression);
        string ExpectedMessage = string.Format(ErrorMessages.HasMaxLength, 5);

        Tree.HasMaxLength(5);

        var Entity = new CreateOrder { CustomerId = "ALFKIS" };

        // Act
        bool Result = Tree.Specifications[0].IsSatisfiedBy(Entity);

        // Assert
        Assert.False(Result);
        Assert.Equal(ExpectedMessage,
            Tree.Specifications[0].Errors.First().ErrorMessage);
    }
}

