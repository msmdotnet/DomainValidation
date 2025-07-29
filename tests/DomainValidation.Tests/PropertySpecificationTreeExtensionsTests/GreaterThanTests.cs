namespace DomainValidation.Tests.PropertySpecificationTreeExtensionsTests;
public class GreaterThanTests
{
    Expression<Func<CreateOrderDetail, int>> PropertyExpression =
        (x => x.ProductId);

    [Theory]
    [InlineData(0, 0, false)]
    [InlineData(1, 0, true)]
    public void GreaterThan_ShouldReturnExpectedResult_WhenValueIsChecked(
        int productId, int comparisonValue, bool expectedResult)
    {
        // Arrange        
        var Tree = new PropertySpecificationsTree<CreateOrderDetail, int>(
            PropertyExpression);
        Tree.GreaterThan(comparisonValue);

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

    [Fact]
    public void GreaterThan_ShouldUseProvidedErrorMessage_WhenValidationFails()
    {
        // Arrange
        var Tree = new PropertySpecificationsTree<CreateOrderDetail, int>(
            PropertyExpression);
        string ExpectedMessage = CreateOrderDetail.ProductIdMessage;

        Tree.GreaterThan(0, ExpectedMessage);

        var Entity = new CreateOrderDetail { ProductId = 0 };

        // Act
        bool Result = Tree.Specifications[0].IsSatisfiedBy(Entity);

        // Assert
        Assert.False(Result);
        Assert.Equal(ExpectedMessage,
            Tree.Specifications[0].Errors.First().ErrorMessage);
    }

    [Fact]
    public void GreaterThan_ShouldUseDefaultErrorMessage_WhenValidationFails()
    {
        // Arrange
        var Tree = new PropertySpecificationsTree<CreateOrderDetail, int>(
            PropertyExpression);
        string ExpectedMessage = string.Format(ErrorMessages.GreaterThan, 0);

        Tree.GreaterThan(0);

        var Entity = new CreateOrderDetail { ProductId = 0 };

        // Act
        bool Result = Tree.Specifications[0].IsSatisfiedBy(Entity);

        // Assert
        Assert.False(Result);
        Assert.Equal(ExpectedMessage,
            Tree.Specifications[0].Errors.First().ErrorMessage);
    }
}

