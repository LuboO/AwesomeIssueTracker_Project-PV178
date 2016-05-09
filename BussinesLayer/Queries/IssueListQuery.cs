using System.Linq;
using BussinesLayer.DTOs;
using DataAccessLayer.Entities;
using Riganti.Utils.Infrastructure.Core;
using AutoMapper;
using System.Collections.Generic;

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
            return (Mapper.Map<List<IssueDTO>>(query)).AsQueryable();
        }
    }
}