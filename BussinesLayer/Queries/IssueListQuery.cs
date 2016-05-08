using System.Linq;
using AutoMapper.QueryableExtensions;
using BussinesLayer.DTOs;
using DataAccessLayer.Entities;
using Riganti.Utils.Infrastructure.Core;

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
            if (Filter.IssueId > 0)
            {
                query = query
                    .Where(c => c.Id == Filter.IssueId);
            }
            return query.ProjectTo<IssueDTO>();
        }
    }
}