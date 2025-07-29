namespace DomainValidation.Tests.PropertySpecificationTreeExtensionsTests;
public class HasFixedLenghtTests
{
    Expression<Func<CreateOrder, string>> PropertyExpression =
        (x => x.CustomerId);

    [Theory]
    [InlineData(null, 5, false)]
    [InlineData("", 5, false)]
    [InlineData("ALF", 5, false)]
    [InlineData("ALFKI", 5, true)]
    public void HasFixedLenght_ShouldReturnExpectedResult_WhenValueIsChecked(
        string customerId, int length, bool expectedResult)
    {
        // Arrange        
        var Tree = new PropertySpecificationsTree<CreateOrder, string>(
            PropertyExpression);
        Tree.HasFixedLength(length);

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
    public void HasFixedLenght_ShouldUseProvidedErrorMessage_WhenValidationFails()
    {
        // Arrange
        var Tree = new PropertySpecificationsTree<CreateOrder, string>(
            PropertyExpression);
        string ExpectedMessage = CreateOrder.CustomerIdFixedLength;

        Tree.HasFixedLength(5, ExpectedMessage);

        var Entity = new CreateOrder { CustomerId = "" };

        // Act
        bool Result = Tree.Specifications[0].IsSatisfiedBy(Entity);

        // Assert
        Assert.False(Result);
        Assert.Equal(ExpectedMessage,
            Tree.Specifications[0].Errors.First().ErrorMessage);
    }

    [Fact]
    public void HasFixedLenght_ShouldUseDefaultErrorMessage_WhenValidationFails()
    {
        // Arrange
        var Tree = new PropertySpecificationsTree<CreateOrder, string>(
            PropertyExpression);
        string ExpectedMessage = string.Format(ErrorMessages.HasFixedLength, 5);

        Tree.HasFixedLength(5);

        var Entity = new CreateOrder { CustomerId = "" };

        // Act
        bool Result = Tree.Specifications[0].IsSatisfiedBy(Entity);

        // Assert
        Assert.False(Result);
        Assert.Equal(ExpectedMessage,
            Tree.Specifications[0].Errors.First().ErrorMessage);
    }
}
