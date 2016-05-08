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
    public class IssueFacade : AITBaseFacade
    {
        public IssueRepository IssueRepository { get; set; }

        public IssueListQuery IssueListQuery { get; set; }

        protected IQuery<IssueDTO> CreateQuery(IssueFilter filter)
        {
            var query = IssueListQuery;
            IssueListQuery.Filter = filter;
            return query;
        }

        public void CreateIssue(IssueDTO Issue)
        {
            using (var uow = UnitOfWorkProvider.Create())
            {
                var created = Mapper.Map<Issue>(Issue);
                IssueRepository.Insert(created);
                uow.Commit();
            }
        }

        public IssueDTO GetIssueById(int IssueId)
        {
            using (UnitOfWorkProvider.Create())
            {
                var Issue = IssueRepository
                    .GetById(IssueId);
                return Mapper.Map<IssueDTO>(Issue);
            }
        }

        public void UpdateIssue(IssueDTO Issue)
        {
            using (var uow = UnitOfWorkProvider.Create())
            {
                var retrieved = IssueRepository.GetById(Issue.Id);
                Mapper.Map(Issue, retrieved);
                IssueRepository.Update(retrieved);
                uow.Commit();
            }
        }

        public void DeleteIssue(IssueDTO Issue)
        {
            using (var uow = UnitOfWorkProvider.Create())
            {
                var deleted = IssueRepository.GetById(Issue.Id);
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
    }
}
