﻿using System.Linq;
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
            if (Filter.CreatorId != null)
                query = query
                    .Where(i => i.CreatorId == Filter.CreatorId);
            if (Filter.AssignedEmployeeId != null)
                query = query
                    .Where(i => i.AssignedEmployeeId == Filter.AssignedEmployeeId);
            if (!string.IsNullOrEmpty(Filter.Title))
                query = query
                    .Where(i => i.Title.Equals(Filter.Title));
            if (Filter.Types.Count > 0)
                query = query
                    .Where(i => Filter.Types.Contains(i.Type));
            if (Filter.Statuses.Count > 0)
                query = query
                    .Where(i => Filter.Statuses.Contains(i.Status));

            return query.Project().To<IssueDTO>();
        }
    }
}