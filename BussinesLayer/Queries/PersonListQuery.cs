using System.Linq;
using BussinesLayer.DTOs;
using DataAccessLayer.Entities;
using Riganti.Utils.Infrastructure.Core;
using System.Collections.Generic;
using AutoMapper;
using BussinesLayer.Filters;

namespace BussinesLayer.Queries
{
    public class PersonListQuery : AITQuery<PersonDTO>
    {
        public PersonFilter Filter { get; set; }

        public PersonListQuery(IUnitOfWorkProvider provider) : base(provider)
        {
        }

        protected override IQueryable<PersonDTO> GetQueryable()
        {
            IQueryable<Person> query = Context.People;

            if (Filter.PersonId != null)
                query = query
                    .Where(p => p.Id == Filter.PersonId);
            if (!string.IsNullOrEmpty(Filter.Name))
                query = query
                    .Where(p => p.Name.Equals(Filter.Name));
            if (!string.IsNullOrEmpty(Filter.Email))
                query = query
                    .Where(p => p.Email.Equals(Filter.Email));

            return (Mapper.Map<List<PersonDTO>>(query)).AsQueryable();
        }
    }
}