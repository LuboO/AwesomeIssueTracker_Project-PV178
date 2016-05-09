using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using BussinesLayer.DTOs;
using BussinesLayer.Repositories;
using BussinesLayer.Queries;
using Riganti.Utils.Infrastructure.Core;
using DataAccessLayer.Entities;
using BussinesLayer.Filters;

namespace BussinesLayer.Facades
{
    public class IssueFacade : AITBaseFacade
    {
        public NotificationFacade NotificationFacade { get; set; }

        public CommentFacade CommentFacade { get; set; }

        public IssueRepository IssueRepository { get; set; }

        public IssueListQuery IssueListQuery { get; set; }

        protected IQuery<IssueDTO> CreateQuery(IssueFilter filter)
        {
            var query = IssueListQuery;
            IssueListQuery.Filter = filter;
            return query;
        }

        public void CreateIssue(IssueDTO issue)
        {
            using (var uow = UnitOfWorkProvider.Create())
            {
                var created = Mapper.Map<Issue>(issue);
                IssueRepository.Insert(created);
                uow.Commit();
            }
        }

        public IssueDTO GetIssueById(int issueId)
        {
            using (UnitOfWorkProvider.Create())
            {
                var issue = IssueRepository
                    .GetById(issueId);
                return Mapper.Map<IssueDTO>(issue);
            }
        }

        public void UpdateIssue(IssueDTO issue)
        {
            using (var uow = UnitOfWorkProvider.Create())
            {
                var retrieved = IssueRepository.GetById(issue.Id);
                Mapper.Map(issue, retrieved);
                IssueRepository.Update(retrieved);
                uow.Commit();
            }
        }

        public void DeleteIssue(IssueDTO issue)
        {
            using (var uow = UnitOfWorkProvider.Create())
            {
                var deleted = IssueRepository.GetById(issue.Id);
                IssueRepository.Delete(deleted);
                uow.Commit();
            }
        }

        public void DeleteIssue(IEnumerable<IssueDTO> issues)
        {
            using (var uow = UnitOfWorkProvider.Create())
            {
                foreach (var i in issues)
                {
                    var deleted = IssueRepository.GetById(i.Id);
                    IssueRepository.Delete(deleted);
                }
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
