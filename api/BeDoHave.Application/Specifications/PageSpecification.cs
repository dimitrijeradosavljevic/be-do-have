using BeDoHave.Data.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace BeDoHave.Application.Specifications
{
    public class PageSpecification : BaseSpecification<Page>
    {
        public PageSpecification(Expression<Func<Page, bool>> criteria, 
                                ICollection<Expression<Func<Page, object>>> includes = null,
                                bool includeAncestors = false, int? start = null, int? take = null, string orderBy = null, string direction = "ASC")
            : base(criteria, start, take, direction)
        {
            if (includes is not null)
            {
                foreach (var expression in includes)
                {
                    AddInclude(expression);
                }
            }
        }
        
    }
}
