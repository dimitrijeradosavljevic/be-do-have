using BeDoHave.Data.AccessLayer.UserDefinedTables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeDoHave.Data.AccessLayer.Interfaces
{
    public interface IOrganisationProcedureRepository
    {
        Task<IList<PageTree>> GetOrganisationTreeAsync(int organisationId);
    }
}
