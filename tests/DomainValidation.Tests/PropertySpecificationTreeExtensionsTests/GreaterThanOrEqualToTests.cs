namespace DomainValidation.Tests.PropertySpecificationTreeExtensionsTests;
public class GreaterThanOrEqualToTests
{
    Expression<Func<CreateOrderDetail, int>> PropertyExpression =
        (x => x.ProductId);

    [Theory]
    [InlineData(0, 1, false)]
    [InlineData(1, 1, true)]
    [InlineData(2, 1, true)]
    public void GreaterThanOrEqualTo_ShouldReturnExpectedResult_WhenValueIsChecked(
        int productId, int comparisonValue, bool expectedResult)
    {
        // Arrange        
        var Tree = new PropertySpecificationsTree<CreateOrderDetail, int>(
            PropertyExpression);
        Tree.GreaterThanOrEqualTo(comparisonValue);

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
    public void GreaterThanOrEqualTo_ShouldUseProvidedErrorMessage_WhenValidationFails()
    {
        // Arrange
        var Tree = new PropertySpecificationsTree<CreateOrderDetail, int>(
            PropertyExpression);
        string ExpectedMessage = CreateOrderDetail.ProductIdMessage;

        Tree.GreaterThanOrEqualTo(1, ExpectedMessage);

        var Entity = new CreateOrderDetail { ProductId = 0 };

        // Act
        bool Result = Tree.Specifications[0].IsSatisfiedBy(Entity);

        // Assert
        Assert.False(Result);
        Assert.Equal(ExpectedMessage,
            Tree.Specifications[0].Errors.First().ErrorMessage);
    }

    [Fact]
    public void GreaterThanOrEqualTo_ShouldUseDefaultErrorMessage_WhenValidationFails()
    {
        // Arrange
        var Tree = new PropertySpecificationsTree<CreateOrderDetail, int>(
            PropertyExpression);
        string ExpectedMessage =
            string.Format(ErrorMessages.GreaterThanOrEqualTo, 1);

        Tree.GreaterThanOrEqualTo(1);

        var Entity = new CreateOrderDetail { ProductId = 0 };

        // Act
        bool Result = Tree.Specifications[0].IsSatisfiedBy(Entity);

        // Assert
        Assert.False(Result);
        Assert.Equal(ExpectedMessage,
            Tree.Specifications[0].Errors.First().ErrorMessage);
    }
}

