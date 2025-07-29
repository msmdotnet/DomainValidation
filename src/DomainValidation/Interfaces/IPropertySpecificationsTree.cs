namespace DomainValidation.Interfaces;
public interface IPropertySpecificationsTree<T>
{
    string PropertyName { get; }
    List<ISpecification<T>> Specifications { get; }
    bool StopOnFirstPropertySpecificationError { get; }
    object GetPropertyValue(T entity);
}

