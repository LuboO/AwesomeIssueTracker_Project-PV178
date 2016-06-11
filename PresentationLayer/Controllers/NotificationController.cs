using BussinesLayer.DTOs;
using BussinesLayer.Facades;
using Microsoft.AspNet.Identity;
using PresentationLayer.Filters.Authorization;
using PresentationLayer.Models.Notification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PresentationLayer.Controllers
{
    [CustomAuthorize]
    public class NotificationController : Controller
    {
        private readonly NotificationFacade notificationFacade;

        public NotificationController(NotificationFacade notificationFacade)
        {
            this.notificationFacade = notificationFacade;
        }

        public ActionResult SubscribeToIssue(int? issueId)
        {
            if (!issueId.HasValue)
                return View("BadInput");

            int userId = User.Identity.GetUserId<int>();

            if (notificationFacade.IsUserSubbedToIssue(userId, issueId.Value))
                return RedirectToAction("IssueDetail", "Issue", new { issueId = issueId.Value });

            var notification = new NotificationDTO()
            {
                NotifyByEmail = false
            };
            notificationFacade.CreateNotification(notification, issueId.Value, userId);
            return RedirectToAction("IssueDetail", "Issue", new { issueId = issueId.Value });
        }

        public ActionResult UnsubscribeFromIssue(int? issueId)
        {
            if (!issueId.HasValue)
                return View("BadInput");

            int userId = User.Identity.GetUserId<int>();

            if (!notificationFacade.IsUserSubbedToIssue(userId, issueId.Value))
                return RedirectToAction("IssueDetail", "Issue", new { issueId = issueId.Value });

            var notification = notificationFacade.GetNotificationByIssueUser(issueId.Value, User.Identity.GetUserId<int>());
            if(notification != null)
                notificationFacade.DeleteNotification(notification);

            return RedirectToAction("IssueDetail", "Issue", new { issueId = issueId.Value });
        }

        public ActionResult ViewNotificationHistory()
        {
            int userId = User.Identity.GetUserId<int>();
            var model = new ViewNotificationHistoryModel()
            {
                ChangedIssues = notificationFacade.GetChangedIssuesByUser(userId),
            };
            return View("ViewNotificationHistory", model);
        }
    }
}