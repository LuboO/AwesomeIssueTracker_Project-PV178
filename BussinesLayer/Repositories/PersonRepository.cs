using DataAccessLayer.Entities;
using Riganti.Utils.Infrastructure.Core;
using Riganti.Utils.Infrastructure.EntityFramework;

namespace BussinesLayer.Repositories
{
    public class PersonRepository : EntityFrameworkRepository<Person, int>
    {
        public PersonRepository(IUnitOfWorkProvider provider) : base(provider)
        {
        }
    }
}
