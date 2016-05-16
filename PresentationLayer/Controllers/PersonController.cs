using System.Web.Mvc;
using BussinesLayer.Facades;
using PresentationLayer.Models.Person;
using PresentationLayer.Models.Employee;
using PresentationLayer.Models.Customer;
using PresentationLayer.Models.Issue;
using PresentationLayer.Models.Comment;
using PresentationLayer.Models.Project;
using BussinesLayer.DTOs;

namespace PresentationLayer.Controllers
{
    public class PersonController : Controller
    {
        private readonly PersonFacade personFacade;
        private readonly EmployeeFacade employeeFacade;
        private readonly CustomerFacade customerFacade;
        private readonly ProjectFacade projectFacade;
        private readonly IssueFacade issueFacade;
        private readonly CommentFacade commentFacade;

        public PersonController(
            PersonFacade personFacade, EmployeeFacade employeeFacade, CustomerFacade customerFacade,
            ProjectFacade projectFacade, IssueFacade issueFacade, CommentFacade commentFacade)
        {
            this.personFacade = personFacade;
            this.employeeFacade = employeeFacade;
            this.customerFacade = customerFacade;
            this.projectFacade = projectFacade;
            this.issueFacade = issueFacade;
            this.commentFacade = commentFacade;
        }

        public ActionResult ViewAllPeople()
        {
            var viewAllPeopleModel = new ViewAllPeopleModel()
            {
                People = personFacade.GetAllPeople()
            };
            return View("ViewAllPeople", viewAllPeopleModel);
        }

        public ActionResult PersonDetail(int personId)
        {
            /* Get personal data */
            var personDetailModel = new PersonDetailModel()
            {
                Person = personFacade.GetPersonById(personId),
                ListIssuesModel = new ListIssuesModel()
                {
                    Issues = issueFacade.GetIssuesByCreator(personId)
                },
                ListCommentsModel = new ListCommentsModel()
                {
                    Comments = commentFacade.GetCommentsByAuthor(personId)
                }
            };
            /* Get employee data if any */
            var employee = employeeFacade.GetEmployeeById(personId);
            if(employee != null)
            {
                personDetailModel.EmployeeDetailModel = new EmployeeDetailModel()
                {
                    Employee = employee,
                    ListIssuesModel = new ListIssuesModel()
                    {
                        Issues = issueFacade.GetIssuesByAssignedEmployee(personId)
                    }
                };
            }
            /* Get customer data if any */
            var customer = customerFacade.GetCustomerById(personId);
            if(customer != null)
            {
                personDetailModel.CustomerDetailModel = new CustomerDetailModel()
                {
                    Customer = customer,
                    ListProjectsModel = new ListProjectsModel()
                    {
                        Projects = projectFacade.GetProjectsByCustomer(personId)
                    }
                };
            }
            return View("PersonDetail", personDetailModel);
        }

        public ActionResult CreatePerson()
        {
            var editPersonModel = new EditPersonModel()
            {
                Person = new PersonDTO()
            };
            return View("CreatePerson", editPersonModel);
        }

        [HttpPost]
        public ActionResult CreatePerson(EditPersonModel editPersonModel)
        {
            personFacade.CreatePerson(editPersonModel.Person);
            return RedirectToAction("ViewAllPeople");
        }

        public ActionResult EditPerson(int personId)
        {
            var editPersonModel = new EditPersonModel()
            {
                Person = personFacade.GetPersonById(personId)
            };
            return View("EditPerson", editPersonModel);
        }

        [HttpPost]
        public ActionResult EditPerson(EditPersonModel editPersonModel)
        {
            personFacade.UpdatePerson(editPersonModel.Person);
            return RedirectToAction("ViewAllPeople");
        }

        public ActionResult DeletePerson(int personId)
        {
            var person = personFacade.GetPersonById(personId);
            personFacade.DeletePerson(person);
            return RedirectToAction("ViewAllPeople");
        }
    }
}