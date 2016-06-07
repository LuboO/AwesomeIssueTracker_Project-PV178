using AutoMapper;
using BussinesLayer.DTOs;
using DataAccessLayer.Entities;

namespace BussinesLayer
{
    public class Mapping
    {
        public static void Create()
        {
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

            Mapper.CreateMap<ProjectDTO, Project>();
            Mapper.CreateMap<Project, ProjectDTO>();

            Mapper.CreateMap<UserDTO, AITUser>();
            Mapper.CreateMap<AITUser, UserDTO>();
        }
    }
}
