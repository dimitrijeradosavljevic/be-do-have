using BeDoHave.Data.Core.Entities;
using System;
using System.Linq.Expressions;

namespace BeDoHave.Application.Specifications
{
    public class UserSpecification : BaseSpecification<User>
    {
        public UserSpecification(Expression<Func<User, bool>> criteria, int? start = null, int? take = null, string orderBy = null, string direction = "ASC")
            : base(criteria, start, take, direction)
        {
            AddOrderBy(orderBy);
        }
    }
}
