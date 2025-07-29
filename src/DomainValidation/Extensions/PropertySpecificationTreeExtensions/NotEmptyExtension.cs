namespace DomainValidation.Extensions.PropertySpecificationTreeExtensions;
public static class NotEmptyExtension
{
    public static PropertySpecificationsTree<T, TProperty>
        NotEmpty<T, TProperty>(
        this PropertySpecificationsTree<T, TProperty> tree,
        string errorMessage = default)
    {
        tree.Specifications.Add(new Specification<T>(entity =>
        PropertySpecificationTreeHelper.Validate(entity, tree, (errors, value) =>
        {
            if (value == null || value is IEnumerable collection &&
                !collection.Cast<object>().Any())
            {
                errors.Add(new SpecificationError(tree.PropertyName,
                    errorMessage ?? ErrorMessages.NotEmpty));
            }
        })));

        return tree;
    }
}

