using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using BussinesLayer.DTOs;
using BussinesLayer.Repositories;
using BussinesLayer.Queries;
using Riganti.Utils.Infrastructure.Core;
using DataAccessLayer.Entities;

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
            using (var uow = UnitOfWorkProvider.Create())
            {
                var created = Mapper.Map<Notification>(notification);
                NotificationRepository.Insert(created);
                uow.Commit();
            }
        }

        public NotificationDTO GetNotificationById(int notificationId)
        {
            using (UnitOfWorkProvider.Create())
            {
                var Notification = NotificationRepository
                    .GetById(notificationId);
                return Mapper.Map<NotificationDTO>(Notification);
            }
        }

        public void UpdateNotification(NotificationDTO notification)
        {
            using (var uow = UnitOfWorkProvider.Create())
            {
                var retrieved = NotificationRepository.GetById(notification.Id);
                Mapper.Map(notification, retrieved);
                NotificationRepository.Update(retrieved);
                uow.Commit();
            }
        }

        public void DeleteNotification(NotificationDTO notification)
        {
            using (var uow = UnitOfWorkProvider.Create())
            {
                var deleted = NotificationRepository.GetById(notification.Id);
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
    }
}
