using System.Linq.Expressions;

namespace Server.Domain.Common.Specifications;

public interface ISpecification<T> 
{
    bool IsSatisfiedBy(T entity);   
    Expression<Func<T, bool>> ToExpression();
}
