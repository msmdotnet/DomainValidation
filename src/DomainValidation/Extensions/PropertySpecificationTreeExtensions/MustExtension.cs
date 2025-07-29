namespace DomainValidation.Extensions.PropertySpecificationTreeExtensions;
public static class MustExtension
{
    public static PropertySpecificationsTree<T, TProperty> Must<T, TProperty>(
        this PropertySpecificationsTree<T, TProperty> tree,
        Func<T, bool> predicate, string errorMessage)
    {
        tree.Specifications.Add(new Specification<T>(entity =>
        PropertySpecificationTreeHelper.Validate(entity, tree, (errors, value) =>
        {
            if (!predicate(entity))
            {
                errors.Add(
                    new SpecificationError(tree.PropertyName, errorMessage));
            }
        })));

        return tree;
    }

    public static PropertySpecificationsTree<T, TProperty> Must<T, TProperty>(
        this PropertySpecificationsTree<T, TProperty> tree,
        Func<TProperty, bool> predicate, string errorMessage)
    {
        tree.Specifications.Add(new Specification<T>(entity =>
            PropertySpecificationTreeHelper.Validate(
                entity, tree, (errors, value) =>
                {
                    if (value is TProperty propertyValue &&
                        !predicate(propertyValue))
                    {
                        errors.Add(
                            new SpecificationError(tree.PropertyName, errorMessage));
                    }
                })));

        return tree;
    }
}
