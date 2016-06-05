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

        public void CreateIssue(IssueDTO issue, int projectId, int creatorId, int employeeId)
        {
            if (issue == null)
                throw new ArgumentNullException("issue");

            using (var uow = UnitOfWorkProvider.Create())
            {
                using (var userManager = UserManagerFactory.Invoke())
                {
                    var created = Mapper.Map<Issue>(issue);
                    if (created == null)
                        return;

                    created.Project = ProjectRepository.GetById(projectId);
                    if (created.Project == null)
                        throw new ObjectNotFoundException("Project hasn't been found");

                    created.Creator = userManager.FindById(creatorId);
                    if (created.Creator == null)
                        throw new ObjectNotFoundException("Creator hasn't been found");

                    created.AssignedEmployee = EmployeeRepository.GetById(employeeId);
                    if (created.AssignedEmployee == null)
                        throw new ObjectNotFoundException("AssignedEmployee hasn't been found");

                    IssueRepository.Insert(created);
                    uow.Commit();
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

        public void UpdateIssue(IssueDTO issue, int projectId, int creatorId, int employeeId)
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
                    retrieved.Project = ProjectRepository.GetById(projectId);
                    if (retrieved.Project == null)
                        throw new ObjectNotFoundException("Project hasn't been found");

                    retrieved.Creator = userManager.FindById(creatorId);
                    if (retrieved.Creator == null)
                        throw new ObjectNotFoundException("Creator hasn't been found");

                    retrieved.AssignedEmployee = EmployeeRepository.GetById(employeeId);
                    if (retrieved.AssignedEmployee == null)
                        throw new ObjectNotFoundException("AssignedEmployee hasn't been found");

                    IssueRepository.Update(retrieved);
                    uow.Commit();
                }
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
                    throw new ObjectNotFoundException("Issue hasn't been found");

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
