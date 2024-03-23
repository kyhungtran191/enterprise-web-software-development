using System.Linq.Expressions;

namespace Server.Domain.Common.Specifications;

public class OrSpecification<T> : ISpecification<T>
{
    private readonly ISpecification<T> _left;
    private readonly ISpecification<T> _right;

    public OrSpecification(ISpecification<T> left, ISpecification<T> right)
    {
        _left = left;
        _right = right;
    }

    public bool IsSatisfiedBy(T entity)
    {
        return _left.IsSatisfiedBy(entity) || _right.IsSatisfiedBy(entity);
    }

    public Expression<Func<T, bool>> ToExpression()
    {
        var leftExpression = _left.ToExpression();
        var rightExpression = _right.ToExpression();

        var paramater = Expression.Parameter(typeof(T));
        
        var combinedExpression = Expression.OrElse(
            leftExpression.Body,
            Expression.Invoke(rightExpression, paramater)
        );
        
        return Expression.Lambda<Func<T, bool>>(combinedExpression, paramater);
    }
}