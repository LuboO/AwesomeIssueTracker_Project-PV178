using AutoMapper.QueryableExtensions;
using BussinesLayer.DTOs;
using BussinesLayer.Filters;
using DataAccessLayer.Entities;
using Riganti.Utils.Infrastructure.Core;
using System.Linq;

namespace BussinesLayer.Queries
{
    public class UserListQuery : AITQuery<UserDTO>
    {
        public UserFilter Filter { get; set; }

        public UserListQuery(IUnitOfWorkProvider provider) : base(provider)
        {
        }

        protected override IQueryable<UserDTO> GetQueryable()
        {
            IQueryable<AITUser> query = Context.Users;

            if (Filter.UserId != null)
                query = query
                    .Where(u => u.Id == Filter.UserId);
            if (!string.IsNullOrEmpty(Filter.UserName))
                query = query
                    .Where(u => u.UserName == Filter.UserName);
            if (!string.IsNullOrEmpty(Filter.Name))
                query = query
                    .Where(u => u.Name == Filter.Name);
            if (!string.IsNullOrEmpty(Filter.Email))
                query = query
                    .Where(u => u.Email == Filter.Email);

            return query.Project().To<UserDTO>();
        }
    }
}
