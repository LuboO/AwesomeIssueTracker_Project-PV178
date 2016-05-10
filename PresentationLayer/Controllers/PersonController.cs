using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using BussinesLayer.Facades;
using PresentationLayer.Models;

namespace PresentationLayer.Controllers
{
    public class PersonController : Controller
    {
        private readonly PersonFacade personFacade;

        public PersonController(PersonFacade personFacade)
        {
            this.personFacade = personFacade;
        }

        public ActionResult People()
        {
            var personViewModel = new PersonViewModel()
            {
                People = personFacade.GetAllPeople()
            };
            return View("People", personViewModel);
        }
    }
}