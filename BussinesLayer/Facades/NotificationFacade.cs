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
using Microsoft.AspNet.Identity;

namespace BussinesLayer.Facades
{
    public class NotificationFacade : AITBaseFacade
    {
        public NotificationRepository NotificationRepository { get; set; }

        public IssueRepository IssueRepository { get; set; }

        public Func<AITUserManager> UserManagerFactory { get; set; }

        public NotificationListQuery NotificationListQuery { get; set; }

        protected IQuery<NotificationDTO> CreateQuery(NotificationFilter filter)
        {
            var query = NotificationListQuery;
            NotificationListQuery.Filter = filter;
            return query;
        }

        public int CreateNotification(NotificationDTO notification, int issueId, int userId)
        {
            if (notification == null)
                throw new ArgumentNullException("notification");

            using (var uow = UnitOfWorkProvider.Create())
            {
                using (var userManager = UserManagerFactory.Invoke())
                {
                    var issue = IssueRepository.GetById(issueId);
                    if (issue == null)
                        throw new ObjectNotFoundException("Issue wasn't found");

                    var user = userManager.FindById(userId);
                    if (user == null)
                        throw new ObjectNotFoundException("Author wasn't found");

                    var created = Mapper.Map<Notification>(notification);

                    created.IssueId = issue.Id;
                    created.Issue = null;
                    created.UserId = user.Id;
                    created.User = null;

                    NotificationRepository.Insert(created);
                    uow.Commit();
                    return created.Id;

                }
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

        public List<NotificationDTO> GetNotificationsByUser(int userId)
        {
            using (UnitOfWorkProvider.Create())
            {
                return CreateQuery(new NotificationFilter() { UserId = userId })
                    .Execute()
                    .ToList();
            }
        }

        public NotificationDTO GetNotificationByIssueUser(int issueId, int userId)
        {
            using (UnitOfWorkProvider.Create())
            {
                var filter = new NotificationFilter()
                {
                    UserId = userId,
                    IssueId = issueId
                };
                return CreateQuery(filter)
                    .Execute()
                    .ToList()
                    .FirstOrDefault();
            }
        }

        public bool IsUserSubbedToIssue(int userId, int issueId)
        {
            using (UnitOfWorkProvider.Create())
            {
                var filter = new NotificationFilter()
                {
                    UserId = userId,
                    IssueId = issueId
                };
                if (CreateQuery(filter).Execute().ToList().Count > 0)
                    return true;

                return false;
            }
        }

        public List<IssueDTO> GetChangedIssuesByUser(int userId)
        {
            using(UnitOfWorkProvider.Create())
            {
                var filter = new NotificationFilter()
                {
                    UserId = userId,
                };

                var userNotifications = CreateQuery(filter).Execute().ToList();
                var changedIssues = userNotifications
                    .Select(n => n.Issue)
                    .Where(i => i.ChangeTime != null)
                    .OrderByDescending(i => i.ChangeTime)
                    .ToList();

                return changedIssues;
            }
        }
    }
}
