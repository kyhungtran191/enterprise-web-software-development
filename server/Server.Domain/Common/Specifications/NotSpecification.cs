using System.Linq.Expressions;

namespace Server.Domain.Common.Specifications;

public class NotSpecification<T> : ISpecification<T>
{
    private readonly ISpecification<T> _specification;

    public NotSpecification(ISpecification<T> specification)
    {
        _specification = specification;
    }

    public bool IsSatisfiedBy(T entity)
    {
        return !_specification.IsSatisfiedBy(entity);
    }

    public Expression<Func<T, bool>> ToExpression()
    {
        var epxression = _specification.ToExpression();
        var parameter = epxression.Parameters[0];
        var notExpression = Expression.Not(parameter);

        return Expression.Lambda<Func<T, bool>>(notExpression, parameter);
    }
}