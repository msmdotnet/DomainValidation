namespace DomainValidation.Extensions.PropertySpecificationTreeExtensions;
public static class HasMinLenghtExtension
{
    public static PropertySpecificationsTree<T, string> HasMinLength<T>(
        this PropertySpecificationsTree<T, string> tree,
        int minLength, string errorMessage = default)
    {
        tree.Specifications.Add(new Specification<T>(entity =>
        PropertySpecificationTreeHelper.Validate(entity, tree, (errors, value) =>
        {
            if (value == null || value.Length < minLength)
            {
                errors.Add(new SpecificationError(tree.PropertyName,
                    errorMessage ??
                    string.Format(ErrorMessages.HasMinLength, minLength)));
            }
        })));

        return tree;
    }
}

