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
using Microsoft.AspNet.Identity;
using System.Data.Entity.Core;
using DataAccessLayer.Enums;

namespace BussinesLayer.Facades
{
    public class IssueFacade : AITBaseFacade
    {
        public IssueRepository IssueRepository { get; set; }

        public ProjectRepository ProjectRepository { get; set; }
        
        public Func<AITUserManager> UserManagerFactory { get; set; }

        public EmployeeRepository EmployeeRepository { get; set; }

        public IssueListQuery IssueListQuery { get; set; }

        protected IQuery<IssueDTO> CreateQuery(IssueFilter filter)
        {
            var query = IssueListQuery;
            IssueListQuery.Filter = filter;
            return query;
        }

        public int CreateIssue(IssueDTO issue, int projectId, int creatorId, int employeeId)
        {
            if (issue == null)
                throw new ArgumentNullException("issue");

            using (var uow = UnitOfWorkProvider.Create())
            {
                using (var userManager = UserManagerFactory.Invoke())
                {
                    if (ProjectRepository.GetById(projectId) == null)
                        throw new ObjectNotFoundException("Project wasn't found");

                    if (userManager.FindById(creatorId) == null)
                        throw new ObjectNotFoundException("Creator wasn't found");
                    
                    if (EmployeeRepository.GetById(employeeId) == null)
                        throw new ObjectNotFoundException("AssignedEmployee wasn't found");

                    var created = Mapper.Map<Issue>(issue);

                    created.ProjectId = projectId;
                    created.Project = null;
                    created.CreatorId = creatorId;
                    created.Creator = null;
                    created.AssignedEmployeeId = employeeId;
                    created.AssignedEmployee = null;

                    IssueRepository.Insert(created);
                    uow.Commit();
                    return created.Id;
                }
            }
        }

        public IssueDTO GetIssueById(int issueId)
        {
            using (UnitOfWorkProvider.Create())
            {
                var issue = IssueRepository.GetById(issueId);
                if (issue == null)
                    return null;

                return Mapper.Map<IssueDTO>(issue);
            }
        }

        public void UpdateIssue(IssueDTO issue, string userName)
        {
            if (issue == null)
                throw new ArgumentNullException("issue");

            using (var uow = UnitOfWorkProvider.Create())
            {
                using (var userManager = UserManagerFactory.Invoke())
                {
                    var retrieved = IssueRepository.GetById(issue.Id);
                    if (retrieved == null)
                        throw new ObjectNotFoundException("Issue not found");

                    Mapper.Map(issue, retrieved);

                    retrieved.ChangeTime = DateTime.Now;
                    retrieved.ChangeType = IssueChangeType.Updated;
                    retrieved.NameOfChanger = userName;

                    IssueRepository.Update(retrieved);
                    uow.Commit();
                }
            }
        }

        public void AcceptIssue(int issueId, string userName)
        {
            using(var uow = UnitOfWorkProvider.Create())
            {
                var issue = IssueRepository.GetById(issueId);
                if (issue == null)
                    throw new ObjectNotFoundException("Issue was not found.");

                if (issue.Status != IssueStatus.New)
                    return;

                issue.Status = IssueStatus.Accepted;

                issue.ChangeTime = DateTime.Now;
                issue.ChangeType = IssueChangeType.Accepted;
                issue.NameOfChanger = userName;

                IssueRepository.Update(issue);
                uow.Commit();
            }

        }

        public void RejectIssue(int issueId, string userName)
        {
            using(var uow = UnitOfWorkProvider.Create())
            {
                var issue = IssueRepository.GetById(issueId);
                if (issue == null)
                    throw new ObjectNotFoundException("Issue was not found.");

                if (issue.Status != IssueStatus.New)
                    return;

                issue.Status = IssueStatus.Rejected;
                issue.Finished = DateTime.Now;

                issue.ChangeTime = DateTime.Now;
                issue.ChangeType = IssueChangeType.Rejected;
                issue.NameOfChanger = userName;

                IssueRepository.Update(issue);
                uow.Commit();
            }
        }

        public void CloseIssue(int issueId, string userName)
        {
            using(var uow = UnitOfWorkProvider.Create())
            {
                var issue = IssueRepository.GetById(issueId);
                if (issue == null)
                    throw new ObjectNotFoundException("Issue was not found.");

                if (issue.Status != IssueStatus.Accepted)
                    return;

                issue.Status = IssueStatus.Closed;
                issue.Finished = DateTime.Now;

                issue.ChangeTime = DateTime.Now;
                issue.ChangeType = IssueChangeType.Closed;
                issue.NameOfChanger = userName;

                IssueRepository.Update(issue);
                uow.Commit();
            }
        }

        public void ReopenIssue(int issueId, string userName)
        {
            using(var uow = UnitOfWorkProvider.Create())
            {
                var issue = IssueRepository.GetById(issueId);
                if (issue == null)
                    throw new ObjectNotFoundException("Issue was not found.");

                if (issue.Status != IssueStatus.Closed && issue.Status != IssueStatus.Rejected)
                    return;

                issue.Status = IssueStatus.Accepted;
                issue.Finished = null;

                issue.ChangeTime = DateTime.Now;
                issue.ChangeType = IssueChangeType.Reopened;
                issue.NameOfChanger = userName;

                IssueRepository.Update(issue);
                uow.Commit();
            }
        }

        public void DeleteIssue(IssueDTO issue)
        {
            if (issue == null)
                throw new ArgumentNullException("issue");

            using (var uow = UnitOfWorkProvider.Create())
            {
                var deleted = IssueRepository.GetById(issue.Id);
                if (deleted == null)
                    throw new ObjectNotFoundException("Issue wasn't found");

                IssueRepository.Delete(deleted);
                uow.Commit();
            }
        }

        public List<IssueDTO> GetAllIssues()
        {
            using (UnitOfWorkProvider.Create())
            {
                return CreateQuery(new IssueFilter())
                    .Execute()
                    .ToList();
            }
        }

        public List<IssueDTO> GetIssuesByProject(int projectId)
        {
            using (UnitOfWorkProvider.Create())
            {
                return CreateQuery(new IssueFilter() { ProjectId = projectId })
                    .Execute()
                    .ToList();
            }
        }

        public List<IssueDTO> GetIssuesByCreator(int creatorId)
        {
            using (UnitOfWorkProvider.Create())
            {
                return CreateQuery(new IssueFilter() { CreatorId = creatorId })
                    .Execute()
                    .ToList();
            }
        }

        public List<IssueDTO> GetIssuesByAssignedEmployee(int assignedEmployeeId)
        {
            using (UnitOfWorkProvider.Create())
            {
                return CreateQuery(new IssueFilter() { AssignedEmployeeId = assignedEmployeeId })
                    .Execute()
                    .ToList();
            }
        }

        public List<IssueDTO> GetIssuesByTitle(string title)
        {
            using (UnitOfWorkProvider.Create())
            {
                return CreateQuery(new IssueFilter() { Title = title })
                    .Execute()
                    .ToList();
            }
        }
    }
}
