using DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataAccessLayer
{
    class Program
    {
        // This is just a demo code for safe removal of objects from database, later it will be probably somewhere entirely else.
        // This exist because cascade delete doesn't work with my glorious model.
        static void SafeRemovePeople(AwesomeIssueTrackerDbContext ctx , List<Person> people)
        {
            foreach (var person in people)
            {
                var childComments = ctx.Comments
                    .Where(c => c.AuthorId == person.Id)
                    .ToList();
                SafeRemoveComments(ctx, childComments);

                var childNotifications = ctx.Notifications
                    .Where(n => n.PersonId == person.Id)
                    .ToList();
                SafeRemoveNotifications(ctx, childNotifications);

                var childIssues = ctx.Issues
                    .Where(i => i.CreatorId == person.Id)
                    .ToList();
                SafeRemoveIssues(ctx, childIssues);

                var childEmployees = ctx.Employees
                    .Where(e => e.Id == person.Id)
                    .ToList();
                SafeRemoveEmployees(ctx, childEmployees);

                var childCustomers = ctx.Customers
                    .Where(c => c.Id == person.Id)
                    .ToList();
                SafeRemoveCustomers(ctx, childCustomers);
            }
            ctx.People.RemoveRange(people);
        }

        static void SafeRemoveEmployees(AwesomeIssueTrackerDbContext ctx , List<Employee> employees)
        {
            foreach(var employee in employees)
            {
                var childIssues = ctx.Issues
                    .Where(i => i.AssignedEmployeeId == employee.Id)
                    .ToList();
                SafeRemoveIssues(ctx, childIssues);
            }
            ctx.Employees.RemoveRange(employees);
        }

        static void SafeRemoveCustomers(AwesomeIssueTrackerDbContext ctx , List<Customer> customers)
        {
            foreach(var customer in customers)
            {
                var childProjects = ctx.Projects
                    .Where(p => p.CustomerId == customer.Id)
                    .ToList();
                SafeRemoveProjects(ctx, childProjects);
            }
            ctx.Customers.RemoveRange(customers);
        }

        static void SafeRemoveProjects(AwesomeIssueTrackerDbContext ctx , List<Project> projects)
        {
            foreach(var project in projects)
            {
                var childIssues = ctx.Issues
                    .Where(i => i.ProjectId == project.Id)
                    .ToList();
                SafeRemoveIssues(ctx, childIssues);
            }
            ctx.Projects.RemoveRange(projects);
        }

        static void SafeRemoveIssues(AwesomeIssueTrackerDbContext ctx , List<Issue> issues)
        {
            foreach(var issue in issues)
            {
                var childComments = ctx.Comments
                    .Where(c => c.IssueId == issue.Id)
                    .ToList();
                SafeRemoveComments(ctx , childComments);

                var childNotifications = ctx.Notifications
                    .Where(n => n.IssueId == issue.Id)
                    .ToList();
                SafeRemoveNotifications(ctx, childNotifications);
            }
            ctx.Issues.RemoveRange(issues);
        }

        static void SafeRemoveNotifications(AwesomeIssueTrackerDbContext ctx , List<Notification> notifications)
        {
            ctx.Notifications.RemoveRange(notifications);
        }

        static void SafeRemoveComments(AwesomeIssueTrackerDbContext ctx, List<Comment> comments)
        {
            ctx.Comments.RemoveRange(comments);
        }

        static void Main(string[] args)
        {
            using (var ctx = new AwesomeIssueTrackerDbContext())
            {
                try
                {
                    // Demonstration of CRUD operations (not so nice but works)
                    // List everyone
                    foreach (var p in ctx.People.ToList())
                        Console.WriteLine(p);
                    Console.WriteLine();
                    // Create
                    var person = new Person
                    {
                        Name = "John Doe",
                        Email = "e@mail.com",
                        Adress = "Some object in memory"
                    };
                    ctx.People.Add(person);
                    ctx.SaveChanges();
                    // List everyone
                    foreach (var p in ctx.People.ToList())
                        Console.WriteLine(p);
                    Console.WriteLine();
                    // Retrieve
                    person = ctx.People
                        .Where(p => p.Name.Equals("John Doe"))
                        .FirstOrDefault();
                    // Update
                    person.Email = "thisemailisbetter@mail.com";
                    ctx.SaveChanges();
                    // List everyone
                    foreach (var p in ctx.People.ToList())
                        Console.WriteLine(p);
                    Console.WriteLine();
                    // Delete (using my function because of disabled cascade delete and intertable dependencies)
                    SafeRemovePeople(ctx, new List<Person>() { person });
                    ctx.SaveChanges();
                    // List everyone
                    foreach (var p in ctx.People.ToList())
                        Console.WriteLine(p);
                    Console.WriteLine();
                }
                catch (Exception ex)
                {
                    // Just to know that something happened.
                    Console.WriteLine(ex.Message);
                }
            }
        }
    }
}