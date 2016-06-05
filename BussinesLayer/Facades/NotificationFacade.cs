using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using BussinesLayer.DTOs;
using BussinesLayer.Repositories;
using BussinesLayer.Queries;
using Riganti.Utils.Infrastructure.Core;
using DataAccessLayer.Entities;
using BussinesLayer.Filters;
using System;
using System.Data.Entity.Core;

namespace BussinesLayer.Facades
{
    public class NotificationFacade : AITBaseFacade
    {
        public NotificationRepository NotificationRepository { get; set; }

        public NotificationListQuery NotificationListQuery { get; set; }

        protected IQuery<NotificationDTO> CreateQuery(NotificationFilter filter)
        {
            var query = NotificationListQuery;
            NotificationListQuery.Filter = filter;
            return query;
        }

        public void CreateNotification(NotificationDTO notification)
        {
            if (notification == null)
                throw new ArgumentNullException("notification");

            using (var uow = UnitOfWorkProvider.Create())
            {
                var created = Mapper.Map<Notification>(notification);
                if (created == null)
                    return;

                NotificationRepository.Insert(created);
                uow.Commit();
            }
        }

        public NotificationDTO GetNotificationById(int notificationId)
        {
            using (UnitOfWorkProvider.Create())
            {
                var notification = NotificationRepository.GetById(notificationId);
                if (notification == null)
                    return null;

                return Mapper.Map<NotificationDTO>(notification);
            }
        }

        public void UpdateNotification(NotificationDTO notification)
        {
            if (notification == null)
                throw new ArgumentNullException("notification");

            using (var uow = UnitOfWorkProvider.Create())
            {
                var retrieved = NotificationRepository.GetById(notification.Id);
                if (retrieved == null)
                    throw new ObjectNotFoundException("Notification not found");

                Mapper.Map(notification, retrieved);
                NotificationRepository.Update(retrieved);
                uow.Commit();
            }
        }

        public void DeleteNotification(NotificationDTO notification)
        {
            if (notification == null)
                throw new ArgumentNullException("notification");

            using (var uow = UnitOfWorkProvider.Create())
            {
                var deleted = NotificationRepository.GetById(notification.Id);
                if (deleted == null)
                    throw new ObjectNotFoundException("Notification not found");

                NotificationRepository.Delete(deleted);
                uow.Commit();
            }
        }

        public List<NotificationDTO> GetAllNotifications()
        {
            using (UnitOfWorkProvider.Create())
            {
                return CreateQuery(new NotificationFilter())
                    .Execute()
                    .ToList();
            }
        }

        public List<NotificationDTO> GetNotificationsByIssue(int issueId)
        {
            using (UnitOfWorkProvider.Create())
            {
                return CreateQuery(new NotificationFilter() { IssueId = issueId })
                    .Execute()
                    .ToList();
            }
        }

        public List<NotificationDTO> GetNotificationsByPerson(int personId)
        {
            using (UnitOfWorkProvider.Create())
            {
                return CreateQuery(new NotificationFilter() { PersonId = personId })
                    .Execute()
                    .ToList();
            }
        }
    }
}
