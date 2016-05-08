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
    public class ProjectFacade : AITBaseFacade
    {
        public ProjectRepository ProjectRepository { get; set; }

        public ProjectListQuery ProjectListQuery { get; set; }

        protected IQuery<ProjectDTO> CreateQuery(ProjectFilter filter)
        {
            var query = ProjectListQuery;
            ProjectListQuery.Filter = filter;
            return query;
        }

        public void CreateProject(ProjectDTO Project)
        {
            using (var uow = UnitOfWorkProvider.Create())
            {
                var created = Mapper.Map<Project>(Project);
                ProjectRepository.Insert(created);
                uow.Commit();
            }
        }

        public ProjectDTO GetProjectById(int ProjectId)
        {
            using (UnitOfWorkProvider.Create())
            {
                var Project = ProjectRepository
                    .GetById(ProjectId);
                return Mapper.Map<ProjectDTO>(Project);
            }
        }

        public void UpdateProject(ProjectDTO Project)
        {
            using (var uow = UnitOfWorkProvider.Create())
            {
                var retrieved = ProjectRepository.GetById(Project.Id);
                Mapper.Map(Project, retrieved);
                ProjectRepository.Update(retrieved);
                uow.Commit();
            }
        }

        public void DeleteProject(ProjectDTO Project)
        {
            using (var uow = UnitOfWorkProvider.Create())
            {
                var deleted = ProjectRepository.GetById(Project.Id);
                ProjectRepository.Delete(deleted);
                uow.Commit();
            }
        }

        public List<ProjectDTO> GetAllProjects()
        {
            using (UnitOfWorkProvider.Create())
            {
                return CreateQuery(new ProjectFilter())
                    .Execute()
                    .ToList();
            }
        }
    }
}
