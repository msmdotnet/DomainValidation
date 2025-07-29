namespace DomainValidation.Extensions.PropertySpecificationTreeExtensions;
public static class HasFixedLenghtSpecification
{
    public static PropertySpecificationsTree<T, string> HasFixedLength<T>(
        this PropertySpecificationsTree<T, string> tree,
        int length, string errorMessage = null)
    {
        tree.Specifications.Add(new Specification<T>(entity =>
        PropertySpecificationTreeHelper.Validate(entity, tree, (errors, value) =>
        {
            if (value == null || value.Length != length)
            {
                errors.Add(new SpecificationError(tree.PropertyName,
                    errorMessage ??
                    string.Format(ErrorMessages.HasFixedLength, length)));
            }
        })));

        return tree;
    }
}

