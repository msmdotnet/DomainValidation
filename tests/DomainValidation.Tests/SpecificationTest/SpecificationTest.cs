namespace DomainValidation.Tests.SpecificationTest;
public class SpecificationTest
{
    ISpecification<CreateOrder> Specification =
        new Specification<CreateOrder>(entity =>
        {
            List<SpecificationError> Errors = null;
            
            if (string.IsNullOrWhiteSpace(entity.CustomerId))
            {
                Errors = new List<SpecificationError>
                {
                    new SpecificationError("CustomerId", 
                        CreateOrder.CustomerIdIsRequired)
                };
            }
            
            return Errors;
        });

    [Fact]
    public void IsSatisfiedBy_ShouldReturnFalse_WhenValidationFails()
    {
        // Arrange
        var Entity = new CreateOrder { CustomerId = null };

        // Act
        var Result = Specification.IsSatisfiedBy(Entity);

        // Assert
        Assert.False(Result);
        Assert.Single(Specification.Errors);
        Assert.Equal(CreateOrder.CustomerIdIsRequired,
            Specification.Errors.First().ErrorMessage);
    }

    [Fact]
    public void IsSatisfiedBy_ShouldReturnTrue_WhenValidationPasses()
    {
        // Arrange
        var Entity = new CreateOrder { CustomerId = "ALFKI" };

        // Act
        var Result = Specification.IsSatisfiedBy(Entity);

        // Assert
        Assert.True(Result);
        Assert.True(Specification.Errors == null ||
            !Specification.Errors.Any(), "Errors should be null or empty");
    }
}

