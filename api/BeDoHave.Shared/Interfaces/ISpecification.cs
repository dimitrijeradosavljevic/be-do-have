using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace BeDoHave.Shared.Interfaces
{
    public interface ISpecification<T>
    {
        Expression<Func<T, bool>> Criteria { get; }
        IReadOnlyList<Expression<Func<T, object>>> Includes { get; }
        IReadOnlyList<string> IncludeStrings { get; }
        IReadOnlyList<string> OrderByStrings { get; }
        int? Skip { get; }
        int? Take { get; }
        string Direction { get; }
    }
}
