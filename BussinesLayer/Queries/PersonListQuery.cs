using System.Linq;
using AutoMapper.QueryableExtensions;
using BussinesLayer.DTOs;
using DataAccessLayer.Entities;
using Riganti.Utils.Infrastructure.Core;
using System.Collections.Generic;
using AutoMapper;

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
            if (Filter.PersonId > 0)
            {
                query = query
                    .Where(c => c.Id == Filter.PersonId);
            }
            return (Mapper.Map<List<PersonDTO>>(query)).AsQueryable();
        }
    }
}