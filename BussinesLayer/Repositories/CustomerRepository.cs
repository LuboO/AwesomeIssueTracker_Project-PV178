using DataAccessLayer.Entities;
using Riganti.Utils.Infrastructure.Core;
using Riganti.Utils.Infrastructure.EntityFramework;

namespace BussinesLayer.Repositories
{
    public class CustomerRepository : EntityFrameworkRepository<Customer, int>
    {
        public CustomerRepository(IUnitOfWorkProvider provider) : base(provider)
        {
        }
    }
}
