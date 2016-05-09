using Castle.Windsor;
using System;
using System.Linq;
using BussinesLayer.Facades;
using BussinesLayer.DTOs;

namespace BussinesLayer
{
    class Program
    {
        static void Main(string[] args)
        {
            // Installation
            IWindsorContainer container = new WindsorContainer();
            container.Install(new Installer());
            PersonFacade personFacade = container.Resolve<PersonFacade>();

            // Demonstration of CRUD operations
            // List everyone
            foreach (var p in personFacade.GetAllPeople())
                Console.WriteLine(p);
            Console.WriteLine();

            // Create person
            var person = new PersonDTO
            {
                Name = "John Doe",
                Email = "e@mail.com",
                Adress = "Some object in memory"
            };
            personFacade.CreatePerson(person);

            // List everyone
            foreach (var p in personFacade.GetAllPeople())
                Console.WriteLine(p);
            Console.WriteLine();

            // Retrieve person
            person = personFacade.GetPeopleByName("John Doe").FirstOrDefault();

            // Update person
            person.Email = "thisemailisbetter@mail.com";
            personFacade.UpdatePerson(person);

            // List everyone
            foreach (var p in personFacade.GetAllPeople())
                Console.WriteLine(p);
            Console.WriteLine();

            // Delete person
            personFacade.DeletePerson(person);
            
            // List everyone
            foreach (var p in personFacade.GetAllPeople())
                Console.WriteLine(p);
            Console.WriteLine();
        }
    }
}
