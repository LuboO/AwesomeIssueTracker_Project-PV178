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

        public void CreatePerson(PersonDTO person)
        {
            using (var uow = UnitOfWorkProvider.Create())
            {
                var created = Mapper.Map<Person>(person);
                PersonRepository.Insert(created);
                uow.Commit();
            }
        }

        public PersonDTO GetPersonById(int personId)
        {
            using (UnitOfWorkProvider.Create())
            {
                var person = PersonRepository
                    .GetById(personId);
                return Mapper.Map<PersonDTO>(person);
            }
        }

        public void UpdatePerson(PersonDTO person)
        {
            using (var uow = UnitOfWorkProvider.Create())
            {
                var retrieved = PersonRepository.GetById(person.Id);
                Mapper.Map(person, retrieved);
                PersonRepository.Update(retrieved);
                uow.Commit();
            }
        }

        public void DeletePerson(PersonDTO person)
        {
            using (var uow = UnitOfWorkProvider.Create())
            {
                var deleted = PersonRepository.GetById(person.Id);
                PersonRepository.Delete(deleted);
                uow.Commit();
            }
        }

        public void DeletePerson(IEnumerable<PersonDTO> people)
        {
            using (var uow = UnitOfWorkProvider.Create())
            {
                foreach(var p in people)
                {
                    var deleted = PersonRepository.GetById(p.Id);
                    PersonRepository.Delete(deleted);
                }
                uow.Commit();
            }
        }

        public List<PersonDTO> GetAllPeople()
        {
            using (UnitOfWorkProvider.Create())
            {
                return CreateQuery(new PersonFilter())
                    .Execute()
                    .ToList();
            }
        }

        public List<PersonDTO> GetPeopleByName(string name)
        {
            using (UnitOfWorkProvider.Create())
            {
                return CreateQuery(new PersonFilter() { Name = name })
                    .Execute()
                    .ToList();
            }
        }

        public List<PersonDTO> GetPeopleByEmail(string email)
        {
            using (UnitOfWorkProvider.Create())
            {
                return CreateQuery(new PersonFilter() { Email = email })
                    .Execute()
                    .ToList();
            }
        }
    }
}
