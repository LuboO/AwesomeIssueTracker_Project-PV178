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
        // Facades removed as they should be, I forgot to do that before
        // turning in BussinesLayer.
        public IssueRepository IssueRepository { get; set; }

        public ProjectRepository ProjectRepository { get; set; }

        public PersonRepository PersonRepository { get; set; }

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
            using (var uow = UnitOfWorkProvider.Create())
            {
                var created = Mapper.Map<Issue>(issue);
                created.Project = ProjectRepository.GetById(projectId);
                created.Creator = PersonRepository.GetById(creatorId);
                created.AssignedEmployee = EmployeeRepository.GetById(employeeId);
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

        public void UpdateIssue(IssueDTO issue, int projectId, int creatorId, int employeeId)
        {
            using (var uow = UnitOfWorkProvider.Create())
            {
                var retrieved = IssueRepository.GetById(issue.Id);
                Mapper.Map(issue, retrieved);
                retrieved.Project = ProjectRepository.GetById(projectId);
                retrieved.Creator = PersonRepository.GetById(creatorId);
                retrieved.AssignedEmployee = EmployeeRepository.GetById(employeeId);
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
