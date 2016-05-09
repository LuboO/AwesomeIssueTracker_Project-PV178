using DataAccessLayer;
using DataAccessLayer.Entities;
using Riganti.Utils.Infrastructure.Core;
using Riganti.Utils.Infrastructure.EntityFramework;
using System.Linq;

namespace BussinesLayer.Repositories
{
    public class CommentRepository : EntityFrameworkRepository<Comment, int>
    {
        public CommentRepository(IUnitOfWorkProvider provider) : base(provider)
        {
        }

        public override void Delete(Comment entity)
        {
            ((AITDbContext)Context).Comments.Remove(entity);
        }

        public override void Delete(int id)
        {
            var entity = GetById(id);
            if (entity != null)
                Delete(entity);
        }
    }
}
