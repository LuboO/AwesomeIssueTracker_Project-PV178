using DataAccessLayer.Entities;
using Riganti.Utils.Infrastructure.Core;
using Riganti.Utils.Infrastructure.EntityFramework;

namespace BussinesLayer.Repositories
{
    public class IssueRepository : EntityFrameworkRepository<Issue, int>
    {
        public IssueRepository(IUnitOfWorkProvider provider) : base(provider)
        {
        }
    }
}
