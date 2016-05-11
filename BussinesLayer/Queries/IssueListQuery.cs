using System.Linq;
using BussinesLayer.DTOs;
using DataAccessLayer.Entities;
using Riganti.Utils.Infrastructure.Core;
using BussinesLayer.Filters;
using AutoMapper.QueryableExtensions;

namespace BussinesLayer.Queries
{
    public class IssueListQuery : AITQuery<IssueDTO>
    {
        public IssueFilter Filter { get; set; }

        public IssueListQuery(IUnitOfWorkProvider provider) : base(provider)
        {
        }

        protected override IQueryable<IssueDTO> GetQueryable()
        {
            IQueryable<Issue> query = Context.Issues;

            if (Filter.IssueId != null)
                query = query
                    .Where(i => i.Id == Filter.IssueId);
            if (Filter.ProjectId != null)
                query = query
                    .Where(i => i.ProjectId == Filter.ProjectId);
            if (Filter.AssignedEmployeeId != null)
                query = query
                    .Where(i => i.AssignedEmployeeId == Filter.AssignedEmployeeId);
            if (!string.IsNullOrEmpty(Filter.Title))
                query = query
                    .Where(i => i.Title.Equals(Filter.Title));

            return query.Project().To<IssueDTO>();
        }
    }
}