using BeDoHave.Data.Core.Entities;
using System;
using System.Linq.Expressions;

namespace BeDoHave.Application.Document.Specifications
{
    public class OrganisationSpecification : BaseSpecification<Organisation>
    {
        public OrganisationSpecification(Expression<Func<Organisation, bool>> criteria, int? start = null, int? take = null, string orderBy = null, string direction = "ASC")
            : base(criteria, start, take, direction)
        {
            AddOrderBy(orderBy);
        }
    }
}
