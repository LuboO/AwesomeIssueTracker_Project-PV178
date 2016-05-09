using DataAccessLayer;
using DataAccessLayer.Entities;
using Riganti.Utils.Infrastructure.Core;
using Riganti.Utils.Infrastructure.EntityFramework;
using System.Linq;

namespace BussinesLayer.Repositories
{
    public class ProjectRepository : EntityFrameworkRepository<Project, int>
    {
        public IssueRepository IssueRepository { get; set; }

        public ProjectRepository(IUnitOfWorkProvider provider) : base(provider)
        {
        }

        public override void Delete(Project entity)
        {
            var issues = ((AITDbContext)Context).Issues
                .Where(i => i.ProjectId == entity.Id)
                .ToList();
            IssueRepository.Delete(issues);

            ((AITDbContext)Context).Projects.Remove(entity);
        }

        public override void Delete(int id)
        {
            var entity = GetById(id);
            if (entity != null)
                Delete(entity);
        }
    }
}
