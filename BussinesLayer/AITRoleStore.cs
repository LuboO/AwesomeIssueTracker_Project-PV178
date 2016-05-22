using DataAccessLayer.Entities;
using Microsoft.AspNet.Identity.EntityFramework;

namespace BussinesLayer
{
    public class AITRoleStore : RoleStore<AITRole, int, AITUserRole>
    {
        public AITRoleStore(IAITUnitOfWorkProvider unitOfWorkProvider)
            : base((unitOfWorkProvider.GetCurrent() as AITUnitOfWork)?.Context)
        {
        }
    }
}
