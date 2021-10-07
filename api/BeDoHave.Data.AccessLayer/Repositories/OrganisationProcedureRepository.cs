using BeDoHave.Data.AccessLayer.Context;
using BeDoHave.Data.AccessLayer.Interfaces;
using BeDoHave.Data.AccessLayer.UserDefinedTables;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeDoHave.Data.AccessLayer.Repositories
{
    public class OrganisationProcedureRepository : IOrganisationProcedureRepository
    {
        private DocumentDbContext _dbContext;

        public OrganisationProcedureRepository(DocumentDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IList<PageTree>> GetOrganisationTreeAsync(int organisationId)
        {
            return await _dbContext.PageTrees.FromSqlRaw("EXEC dbo.SelectOrganisationTree {0}", organisationId).ToListAsync();
        }
    }
}
