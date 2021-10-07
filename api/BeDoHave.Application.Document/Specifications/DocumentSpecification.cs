using BeDoHave.Data.Core.Entities;
using System;
using System.Linq.Expressions;

namespace BeDoHave.Application.Document.Specifications
{
    public class DocumentSpecification : BaseSpecification<Data.Core.Entities.Document>
    {
        public DocumentSpecification(Expression<Func<Data.Core.Entities.Document, bool>> criteria, int? start = null, int? take = null, string orderBy = null, string direction = "ASC")
            : base(criteria, start, take, direction)
        {
            AddOrderBy(orderBy);
        }
    }
}
