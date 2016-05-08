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

        public void CreateNotification(NotificationDTO Notification)
        {
            using (var uow = UnitOfWorkProvider.Create())
            {
                var created = Mapper.Map<Notification>(Notification);
                NotificationRepository.Insert(created);
                uow.Commit();
            }
        }

        public NotificationDTO GetNotificationById(int NotificationId)
        {
            using (UnitOfWorkProvider.Create())
            {
                var Notification = NotificationRepository
                    .GetById(NotificationId);
                return Mapper.Map<NotificationDTO>(Notification);
            }
        }

        public void UpdateNotification(NotificationDTO Notification)
        {
            using (var uow = UnitOfWorkProvider.Create())
            {
                var retrieved = NotificationRepository.GetById(Notification.Id);
                Mapper.Map(Notification, retrieved);
                NotificationRepository.Update(retrieved);
                uow.Commit();
            }
        }

        public void DeleteNotification(NotificationDTO Notification)
        {
            using (var uow = UnitOfWorkProvider.Create())
            {
                var deleted = NotificationRepository.GetById(Notification.Id);
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
