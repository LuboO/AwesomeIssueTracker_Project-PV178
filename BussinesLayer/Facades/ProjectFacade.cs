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
    public class ProjectFacade : AITBaseFacade
    {
        public CustomerRepository CustomerRepository { get; set; }

        public ProjectRepository ProjectRepository { get; set; }

        public ProjectListQuery ProjectListQuery { get; set; }

        protected IQuery<ProjectDTO> CreateQuery(ProjectFilter filter)
        {
            var query = ProjectListQuery;
            ProjectListQuery.Filter = filter;
            return query;
        }

        public void CreateProject(ProjectDTO project, int customerId)
        {
            using (var uow = UnitOfWorkProvider.Create())
            {
                var created = Mapper.Map<Project>(project);
                created.Customer = CustomerRepository.GetById(customerId);
                ProjectRepository.Insert(created);
                uow.Commit();
            }
        }

        public ProjectDTO GetProjectById(int projectId)
        {
            using (UnitOfWorkProvider.Create())
            {
                var Project = ProjectRepository
                    .GetById(projectId);
                return Mapper.Map<ProjectDTO>(Project);
            }
        }

        public void UpdateProject(ProjectDTO project, int customerId)
        {
            using (var uow = UnitOfWorkProvider.Create())
            {
                var retrieved = ProjectRepository.GetById(project.Id);
                Mapper.Map(project, retrieved);
                retrieved.Customer = CustomerRepository.GetById(customerId);
                ProjectRepository.Update(retrieved);
                uow.Commit();
            }
        }

        public void DeleteProject(ProjectDTO project)
        {
            using (var uow = UnitOfWorkProvider.Create())
            {
                var deleted = ProjectRepository.GetById(project.Id);
                ProjectRepository.Delete(deleted);
                uow.Commit();
            }
        }

        public void DeleteProject(IEnumerable<ProjectDTO> projects)
        {
            using(var uow = UnitOfWorkProvider.Create())
            {
                foreach (var p in projects) {
                    var deleted = ProjectRepository.GetById(p.Id);
                    ProjectRepository.Delete(deleted);
                }
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

        public List<ProjectDTO> GetProjectsByCustomer(int customerId)
        {
            using (UnitOfWorkProvider.Create())
            {
                return CreateQuery(new ProjectFilter() { CustomerId = customerId })
                    .Execute()
                    .ToList();
            }
        }

        public List<ProjectDTO> GetProjectsByName(string name)
        {
            using (UnitOfWorkProvider.Create())
            {
                return CreateQuery(new ProjectFilter() { Name = name })
                    .Execute()
                    .ToList();
            }
        }
    }
}
