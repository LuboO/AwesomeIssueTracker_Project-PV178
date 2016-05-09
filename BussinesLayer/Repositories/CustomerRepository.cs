using DataAccessLayer.Entities;
using DataAccessLayer;
using Riganti.Utils.Infrastructure.Core;
using Riganti.Utils.Infrastructure.EntityFramework;
using System.Linq;

namespace BussinesLayer.Repositories
{
    public class CustomerRepository : EntityFrameworkRepository<Customer, int>
    {
        public ProjectRepository ProjectRepository { get; set; }

        public CustomerRepository(IUnitOfWorkProvider provider) : base(provider)
        {
        }

        public override void Delete(Customer entity)
        {
            var projects = ((AITDbContext)Context).Projects
                .Where(p => p.CustomerId == entity.Id)
                .ToList();
            ProjectRepository.Delete(projects);

            ((AITDbContext)Context).Customers.Remove(entity);
        }

        public override void Delete(int id)
        {
            var entity = GetById(id);
            if (entity != null)
                Delete(entity);
        }
    }
}
