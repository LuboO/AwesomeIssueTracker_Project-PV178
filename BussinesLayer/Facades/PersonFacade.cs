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
    public class PersonFacade : AITBaseFacade
    {
        public PersonRepository PersonRepository { get; set; }

        public PersonListQuery PersonListQuery { get; set; }

        protected IQuery<PersonDTO> CreateQuery(PersonFilter filter)
        {
            var query = PersonListQuery;
            PersonListQuery.Filter = filter;
            return query;
        }

        public void CreatePerson(PersonDTO Person)
        {
            using (var uow = UnitOfWorkProvider.Create())
            {
                var created = Mapper.Map<Person>(Person);
                PersonRepository.Insert(created);
                uow.Commit();
            }
        }

        public PersonDTO GetPersonById(int PersonId)
        {
            using (UnitOfWorkProvider.Create())
            {
                var Person = PersonRepository
                    .GetById(PersonId);
                return Mapper.Map<PersonDTO>(Person);
            }
        }

        public void UpdatePerson(PersonDTO Person)
        {
            using (var uow = UnitOfWorkProvider.Create())
            {
                var retrieved = PersonRepository.GetById(Person.Id);
                Mapper.Map(Person, retrieved);
                PersonRepository.Update(retrieved);
                uow.Commit();
            }
        }

        public void DeletePerson(PersonDTO Person)
        {
            using (var uow = UnitOfWorkProvider.Create())
            {
                var deleted = PersonRepository.GetById(Person.Id);
                PersonRepository.Delete(deleted);
                uow.Commit();
            }
        }

        public List<PersonDTO> GetAllPersons()
        {
            using (UnitOfWorkProvider.Create())
            {
                return CreateQuery(new PersonFilter())
                    .Execute()
                    .ToList();
            }
        }
    }
}
