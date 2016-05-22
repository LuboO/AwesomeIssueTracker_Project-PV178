using DataAccessLayer;
using DataAccessLayer.Entities;
using Microsoft.AspNet.Identity.EntityFramework;

namespace BussinesLayer
{
    public class AITUserStore : UserStore<AITUser, AITRole, int, AITUserLogin, AITUserRole, AITUserClaim>
    {
        public AITUserStore(IAITUnitOfWorkProvider unitOfWorkProvider)
            : base((unitOfWorkProvider.GetCurrent() as AITUnitOfWork)?.Context)
        {
        }

        public AITUserStore(AITDbContext context)
            : base(context)
        {
        }
    }
}
