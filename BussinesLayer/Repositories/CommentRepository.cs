using DataAccessLayer.Entities;
using Riganti.Utils.Infrastructure.Core;
using Riganti.Utils.Infrastructure.EntityFramework;

namespace BussinesLayer.Repositories
{
    public class CommentRepository : EntityFrameworkRepository<Comment, int>
    {
        public CommentRepository(IUnitOfWorkProvider provider) : base(provider)
        {
        }
    }
}
