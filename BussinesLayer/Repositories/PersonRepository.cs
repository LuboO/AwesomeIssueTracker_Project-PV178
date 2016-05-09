using DataAccessLayer;
using DataAccessLayer.Entities;
using Riganti.Utils.Infrastructure.Core;
using Riganti.Utils.Infrastructure.EntityFramework;
using System.Collections.Generic;
using System.Linq;

namespace BussinesLayer.Repositories
{
    public class PersonRepository : EntityFrameworkRepository<Person, int>
    {
        public IssueRepository IssueRepository { get; set; }

        public CommentRepository CommentRepository { get; set; }

        public NotificationRepository NotificationRepository { get; set; }

        public EmployeeRepository EmployeeRepository { get; set; }

        public CustomerRepository CustomerRepository { get; set; }

        public PersonRepository(IUnitOfWorkProvider provider) : base(provider)
        {
        }

        public override void Delete(Person entity)
        {
            var comments = ((AITDbContext)Context).Comments
                .Where(c => c.AuthorId == entity.Id)
                .ToList();
            CommentRepository.Delete(comments);

            var notifications = ((AITDbContext)Context).Notifications
                .Where(n => n.PersonId == entity.Id)
                .ToList();
            NotificationRepository.Delete(notifications);

            var issues = ((AITDbContext)Context).Issues
                .Where(i => i.CreatorId == entity.Id)
                .ToList();
            IssueRepository.Delete(issues);

            var employees = ((AITDbContext)Context).Employees
                .Where(e => e.Id == entity.Id)
                .ToList();
            EmployeeRepository.Delete(employees);

            var customers = ((AITDbContext)Context).Customers
                .Where(c => c.Id == entity.Id)
                .ToList();
            CustomerRepository.Delete(customers);

            ((AITDbContext)Context).People.Remove(entity);
        }

        public override void Delete(int id)
        {
            var entity = GetById(id);
            if (entity != null)
                Delete(entity);
        }
    }
}
