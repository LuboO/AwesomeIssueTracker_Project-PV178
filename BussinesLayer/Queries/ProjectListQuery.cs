using System.Linq;
using BussinesLayer.DTOs;
using DataAccessLayer.Entities;
using Riganti.Utils.Infrastructure.Core;
using AutoMapper;
using System.Collections.Generic;

namespace BussinesLayer.Queries
{
    public class ProjectListQuery : AITQuery<ProjectDTO>
    {
        public ProjectFilter Filter { get; set; }

        public ProjectListQuery(IUnitOfWorkProvider provider) : base(provider)
        {
        }

        protected override IQueryable<ProjectDTO> GetQueryable()
        {
            IQueryable<Project> query = Context.Projects;

            if (Filter.ProjectId != null)
                query = query
                    .Where(p => p.Id == Filter.ProjectId);
            if (Filter.CustomerId != null)
                query = query
                    .Where(p => p.CustomerId == Filter.CustomerId);
            if (!string.IsNullOrEmpty(Filter.Name))
                query = query
                    .Where(p => p.Name.Equals(Filter.Name));

            return (Mapper.Map<List<ProjectDTO>>(query)).AsQueryable();
        }
    }
}