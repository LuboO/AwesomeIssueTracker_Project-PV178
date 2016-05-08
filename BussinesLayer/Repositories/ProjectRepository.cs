using DataAccessLayer.Entities;
using Riganti.Utils.Infrastructure.Core;
using Riganti.Utils.Infrastructure.EntityFramework;

namespace BussinesLayer.Repositories
{
    public class ProjectRepository :EntityFrameworkRepository<Project, int>
    {
        public ProjectRepository(IUnitOfWorkProvider provider) : base(provider)
        {
        }
    }
}
