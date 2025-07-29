namespace DomainValidation.Core;
public abstract class DomainSpecificationBase<T> : IDomainSpecification<T>
{
    public DomainSpecificationBase(bool evaluateOnlyIfNoPreviousErrors = false)
    {
        EvaluateOnlyIfNoPreviousErrors = evaluateOnlyIfNoPreviousErrors;
    }

    public bool EvaluateOnlyIfNoPreviousErrors { get; }
    public bool StopOnFirstEntitySpecificationError { get; protected set; } = false;
    public IEnumerable<SpecificationError> Errors { get; protected set; }
    List<IPropertySpecificationsTree<T>> PropertySpecificationsForest = [];

    protected PropertySpecificationsTree<T, TProperty> Property<TProperty>(
        Expression<Func<T, TProperty>> propertyExpression,
        bool stopOnFirstPropertySpecificationError = false)
    {
        var Tree = new PropertySpecificationsTree<T, TProperty>(
            propertyExpression, stopOnFirstPropertySpecificationError);

        PropertySpecificationsForest.Add(Tree);

        return Tree;
    }

    List<SpecificationError> ValidatePropertySpecificationsForest(T entity)
    {
        List<SpecificationError> SpecificationErrors = [];

        var TreesEnumerator = PropertySpecificationsForest.GetEnumerator();
        bool ContinueValidatingTrees = true;

        while (TreesEnumerator.MoveNext() && ContinueValidatingTrees)
        {
            var TreeSpecificationsEnumerator =
                TreesEnumerator.Current.Specifications.GetEnumerator();
            bool ContinueValidatingTreeSpecifications = true;

            while (TreeSpecificationsEnumerator.MoveNext() &&
                ContinueValidatingTreeSpecifications)
            {
                if (!TreeSpecificationsEnumerator.Current.IsSatisfiedBy(entity))
                {
                    SpecificationErrors.AddRange(
                        TreeSpecificationsEnumerator.Current.Errors);

                    if (TreesEnumerator.Current.StopOnFirstPropertySpecificationError)
                        ContinueValidatingTreeSpecifications = false;

                }
            }

            if (SpecificationErrors.Count != 0 && StopOnFirstEntitySpecificationError)
                ContinueValidatingTrees = false;
        }

        return SpecificationErrors;
    }

    protected virtual Task<List<SpecificationError>> ValidateSpecificationsAsync(
        T entity) => Task.FromResult(new List<SpecificationError>());

    public async Task<bool> ValidateAsync(T entity)
    {
        Errors = [
            .. ValidatePropertySpecificationsForest(entity),
            .. await ValidateSpecificationsAsync(entity) ?? []
            ];

        return !Errors.Any();
    }

}

