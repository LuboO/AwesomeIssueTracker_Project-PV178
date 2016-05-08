using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BussinesLayer.DTOs;
using DataAccessLayer.Entities;

namespace BussinesLayer
{
    public class Mapping
    {
        public static void Create()
        {
#pragma warning disable CS0618 // Type or member is obsolete
            Mapper.CreateMap<Comment, CommentDTO>();
            Mapper.CreateMap<CommentDTO, Comment>();

            Mapper.CreateMap<CustomerDTO, Customer>();
            Mapper.CreateMap<Customer, CustomerDTO>();

            Mapper.CreateMap<EmployeeDTO, Employee>();
            Mapper.CreateMap<Employee, EmployeeDTO>();

            Mapper.CreateMap<IssueDTO, Issue>();
            Mapper.CreateMap<Issue, IssueDTO>();

            Mapper.CreateMap<NotificationDTO, Notification>();
            Mapper.CreateMap<Notification, NotificationDTO>();

            Mapper.CreateMap<PersonDTO, Person>();
            Mapper.CreateMap<Person, PersonDTO>();

            Mapper.CreateMap<ProjectDTO, Project>();
            Mapper.CreateMap<Project, ProjectDTO>();
#pragma warning restore CS0618 // Type or member is obsolete
        }
    }
}
