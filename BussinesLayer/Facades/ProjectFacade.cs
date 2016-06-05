using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using BussinesLayer.DTOs;
using BussinesLayer.Repositories;
using BussinesLayer.Queries;
using Riganti.Utils.Infrastructure.Core;
using DataAccessLayer.Entities;
using BussinesLayer.Filters;
using System;
using System.Data.Entity.Core;

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
            if (project == null)
                throw new ArgumentNullException("project");

            using (var uow = UnitOfWorkProvider.Create())
            {
                var created = Mapper.Map<Project>(project);
                if (created == null)
                    return;

                created.Customer = CustomerRepository.GetById(customerId);
                if (created.Customer == null)
                    throw new ObjectNotFoundException("Customer not found");

                ProjectRepository.Insert(created);
                uow.Commit();
            }
        }

        public ProjectDTO GetProjectById(int projectId)
        {
            using (UnitOfWorkProvider.Create())
            {
                var project = ProjectRepository.GetById(projectId);
                if (project == null)
                    return null;

                return Mapper.Map<ProjectDTO>(project);
            }
        }

        public void UpdateProject(ProjectDTO project, int customerId)
        {
            if (project == null)
                throw new ArgumentNullException("project");

            using (var uow = UnitOfWorkProvider.Create())
            {
                var retrieved = ProjectRepository.GetById(project.Id);
                if (retrieved == null)
                    throw new ObjectNotFoundException("Project not found");

                Mapper.Map(project, retrieved);
                retrieved.Customer = CustomerRepository.GetById(customerId);
                if (retrieved.Customer == null)
                    throw new ObjectNotFoundException("Customer not found");

                ProjectRepository.Update(retrieved);
                uow.Commit();
            }
        }

        public void DeleteProject(ProjectDTO project)
        {
            if (project == null)
                throw new ArgumentNullException("project");

            using (var uow = UnitOfWorkProvider.Create())
            {
                var deleted = ProjectRepository.GetById(project.Id);
                if (deleted == null)
                    throw new ObjectNotFoundException("Project not found");

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
