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
    }
}
