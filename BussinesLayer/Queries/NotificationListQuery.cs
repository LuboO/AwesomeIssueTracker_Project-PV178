﻿using System.Linq;
using BussinesLayer.DTOs;
using DataAccessLayer.Entities;
using Riganti.Utils.Infrastructure.Core;
using AutoMapper;
using System.Collections.Generic;

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
            if (Filter.NotificationId > 0)
            {
                query = query
                    .Where(c => c.Id == Filter.NotificationId);
            }
            return (Mapper.Map<List<NotificationDTO>>(query)).AsQueryable();
        }
    }
}