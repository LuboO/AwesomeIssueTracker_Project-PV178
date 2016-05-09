using DataAccessLayer;
using DataAccessLayer.Entities;
using Riganti.Utils.Infrastructure.Core;
using Riganti.Utils.Infrastructure.EntityFramework;

namespace BussinesLayer.Repositories
{
    public class NotificationRepository : EntityFrameworkRepository<Notification, int>
    {
        public NotificationRepository(IUnitOfWorkProvider provider) : base(provider)
        {
        }

        public override void Delete(Notification entity)
        {
            ((AITDbContext)Context).Notifications.Remove(entity);
        }

        public override void Delete(int id)
        {
            var entity = GetById(id);
            if (entity != null)
                Delete(entity);
        }
    }
}
