namespace DomainValidation.Extensions.PropertySpecificationTreeExtensions;
public static class SetValidatorExtension
{
    public static PropertySpecificationsTree<T, IEnumerable<TElement>>
        SetValidator<T, TElement>(
        this PropertySpecificationsTree<T, IEnumerable<TElement>> tree,
        IDomainSpecificationsValidator<TElement> validator)
    {
        tree.Specifications.Add(new Specification<T>(entity =>
        PropertySpecificationTreeHelper.Validate(entity, tree, (errors, value) =>
        {
            if (value is IEnumerable<TElement> Collection)
            {
                int Index = 0;
                foreach (var Item in Collection)
                {
                    var Result = validator.ValidateAsync(Item)
                        .GetAwaiter().GetResult();

                    if (!Result.IsValid)
                    {
                        foreach (var Error in Result.Errors)
                        {
                            errors.Add(new SpecificationError(
                            $"{tree.PropertyName}[{Index}].{Error.PropertyName}",
                            Error.ErrorMessage));
                        }
                    }

                    Index++;
                }
            }
        })));

        return tree;
    }
}

