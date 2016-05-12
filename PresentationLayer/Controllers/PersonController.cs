using System.Web.Mvc;
using BussinesLayer.Facades;
using PresentationLayer.Models.Person;

namespace PresentationLayer.Controllers
{
    public class PersonController : Controller
    {
        private readonly PersonFacade personFacade;

        public PersonController(PersonFacade personFacade)
        {
            this.personFacade = personFacade;
        }

        public ActionResult ListPeople()
        {
            var listPeopleModel = new ListPeopleModel()
            {
                People = personFacade.GetAllPeople()
            };
            return View("ListPeople", listPeopleModel);
        }

        public ActionResult PersonDetail(int personId)
        {
            var personDetailModel = new PersonDetailModel()
            {
                Person = personFacade.GetPersonById(personId)
            };
            return View("PersonDetail", personDetailModel);
        }
    }
}