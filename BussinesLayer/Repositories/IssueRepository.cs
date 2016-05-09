using DataAccessLayer;
using DataAccessLayer.Entities;
using Riganti.Utils.Infrastructure.Core;
using Riganti.Utils.Infrastructure.EntityFramework;
using System.Linq;

namespace BussinesLayer.Repositories
{
    public class IssueRepository : EntityFrameworkRepository<Issue, int>
    {
        public NotificationRepository NotificationRepository { get; set; }

        public CommentRepository CommentRepository { get; set; }

        public IssueRepository(IUnitOfWorkProvider provider) : base(provider)
        {
        }

        public override void Delete(Issue entity)
        {
            var comments = ((AITDbContext)Context).Comments
                .Where(c => c.IssueId == entity.Id)
                .ToList();
            CommentRepository.Delete(comments);

            var notifications = ((AITDbContext)Context).Notifications
                .Where(c => c.IssueId == entity.Id)
                .ToList();
            NotificationRepository.Delete(notifications);

            ((AITDbContext)Context).Issues.Remove(entity);
        }

        public override void Delete(int id)
        {
            var entity = GetById(id);
            if (entity != null)
                Delete(entity);
        }
    }
}
