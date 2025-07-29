namespace DomainValidation.Core;
public class PropertySpecificationsTree<T, TProperty>(
    Expression<Func<T, TProperty>> propertyExpression,
    bool stopOnFirstPropertySpecificationError = false) : IPropertySpecificationsTree<T>
{
    readonly Func<T, TProperty> PropertyExpressionDelegate =
        propertyExpression.Compile();

    public string PropertyName { get; } =
        propertyExpression.GetPropertyName();

    public List<ISpecification<T>> Specifications { get; } = [];
    public bool StopOnFirstPropertySpecificationError =>
        stopOnFirstPropertySpecificationError;
    public object GetPropertyValue(T entity) =>
        PropertyExpressionDelegate(entity);
}

