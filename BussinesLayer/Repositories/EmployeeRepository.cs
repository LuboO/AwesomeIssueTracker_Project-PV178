using DataAccessLayer;
using DataAccessLayer.Entities;
using Riganti.Utils.Infrastructure.Core;
using Riganti.Utils.Infrastructure.EntityFramework;
using System.Linq;

namespace BussinesLayer.Repositories
{
    public class EmployeeRepository : EntityFrameworkRepository<Employee, int>
    {
        public IssueRepository IssueRepository { get; set; }

        public EmployeeRepository(IUnitOfWorkProvider provider) :base(provider)
        {
        }

        public override void Delete(Employee entity)
        {
            var issues = ((AITDbContext)Context).Issues
                .Where(i => i.AssignedEmployeeId == entity.Id)
                .ToList();
            IssueRepository.Delete(issues);

            ((AITDbContext)Context).Employees.Remove(entity);
        }

        public override void Delete(int id)
        {
            var entity = GetById(id);
            if (entity != null)
                Delete(entity);
        }
    }
}
