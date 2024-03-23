using System.Linq.Expressions;
using Server.Domain.Common.Specifications;
using Server.Domain.Entity.Content;

namespace Server.Domain.Specifications.FacultySpec;

public class ByFacultySpecification : ISpecification<Faculty>
{
    private readonly string _faculty;

    public ByFacultySpecification(string faculty)
    {           
        this._faculty = faculty;
    }

    public bool IsSatisfiedBy(Faculty entity)
    {
        return entity.Name.Contains(_faculty);
    }

    public Expression<Func<Faculty, bool>> ToExpression()
    {
        return faculty => IsSatisfiedBy(faculty);
    }
}