using System.Linq;
using BussinesLayer.DTOs;
using DataAccessLayer.Entities;
using Riganti.Utils.Infrastructure.Core;
using AutoMapper;
using System.Collections.Generic;
using BussinesLayer.Filters;

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
            if (Filter.PersonId != null)
                query = query
                    .Where(n => n.PersonId == Filter.PersonId);

            return (Mapper.Map<List<NotificationDTO>>(query)).AsQueryable();
        }
    }
}