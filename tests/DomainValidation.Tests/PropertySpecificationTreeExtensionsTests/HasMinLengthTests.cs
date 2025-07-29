namespace DomainValidation.Tests.PropertySpecificationTreeExtensionsTests;
public class HasMinLengthTests
{
    Expression<Func<CreateOrder, string>> PropertyExpression =
        (x => x.CustomerId);

    [Theory]
    [InlineData(null, 5, false)]
    [InlineData("", 5, false)]
    [InlineData("ALF", 5, false)]
    [InlineData("ALFKI", 5, true)]
    [InlineData("ALFKIS", 5, true)]
    public void HasMinLength_ShouldReturnExpectedResult_WhenValueIsChecked(
        string customerId, int length, bool expectedResult)
    {
        // Arrange        
        var Tree = new PropertySpecificationsTree<CreateOrder, string>(
            PropertyExpression);
        Tree.HasMinLength(length);

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
    public void HasMinLength_ShouldUseProvidedErrorMessage_WhenValidationFails()
    {
        // Arrange
        var Tree = new PropertySpecificationsTree<CreateOrder, string>(
            PropertyExpression);
        string ExpectedMessage = CreateOrder.CustomerIdHasMinLength;

        Tree.HasMinLength(5, ExpectedMessage);

        var Entity = new CreateOrder { CustomerId = "ALF" };

        // Act
        bool Result = Tree.Specifications[0].IsSatisfiedBy(Entity);

        // Assert
        Assert.False(Result);
        Assert.Equal(ExpectedMessage,
            Tree.Specifications[0].Errors.First().ErrorMessage);
    }

    [Fact]
    public void HasMinLength_ShouldUseDefaultErrorMessage_WhenValidationFails()
    {
        // Arrange
        var Tree = new PropertySpecificationsTree<CreateOrder, string>(
            PropertyExpression);
        string ExpectedMessage = string.Format(ErrorMessages.HasMinLength, 5);

        Tree.HasMinLength(5);

        var Entity = new CreateOrder { CustomerId = "ALF" };

        // Act
        bool Result = Tree.Specifications[0].IsSatisfiedBy(Entity);

        // Assert
        Assert.False(Result);
        Assert.Equal(ExpectedMessage,
            Tree.Specifications[0].Errors.First().ErrorMessage);
    }
}

