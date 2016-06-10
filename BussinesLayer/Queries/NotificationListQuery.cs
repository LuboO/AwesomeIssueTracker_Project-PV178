using System.Linq;
using BussinesLayer.DTOs;
using DataAccessLayer.Entities;
using Riganti.Utils.Infrastructure.Core;
using BussinesLayer.Filters;
using AutoMapper.QueryableExtensions;

namespace BussinesLayer.Queries
{
    public class NotificationListQuery : AITQuery<NotificationDTO>
    {
        public NotificationFilter Filter { get; set; }

        public NotificationListQuery(IUnitOfWorkProvider provider) : base(provider)
        {
        }

        protected override IQueryable<NotificationDTO> GetQueryable()
        {
            IQueryable<Notification> query = Context.Notifications;

            if (Filter.NotificationId != null)
                query = query
                    .Where(n => n.Id == Filter.NotificationId);
            if (Filter.IssueId != null)
                query = query
                    .Where(n => n.IssueId == Filter.IssueId);
            if (Filter.UserId != null)
                query = query
                    .Where(n => n.UserId == Filter.UserId);

            return query.Project().To<NotificationDTO>();
        }
    }
}