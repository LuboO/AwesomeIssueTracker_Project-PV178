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
            if (Filter.ProjectId > 0)
            {
                query = query
                    .Where(c => c.Id == Filter.ProjectId);
            }
            return (Mapper.Map<List<ProjectDTO>>(query)).AsQueryable();
        }
    }
}