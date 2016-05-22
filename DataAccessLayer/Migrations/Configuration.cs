namespace DataAccessLayer.Migrations
{
    using Enums;
    using Entities;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<DataAccessLayer.AITDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(DataAccessLayer.AITDbContext context)
        {
            // Created some sample data for debugging.
            var persNightWatch = new Person
            {
                Name = "Night Watch",
                Email = "nightwatch@thewall.we"
            };

            var persJonSnow = new Person
            {
                Name = "Jon Snow",
                Email = "iknownothing@thewall.we"
            };

            var persOlly = new Person
            {
                Name = "Olly",
                Email = "stupidkid@thewall.we"
            };

            var persAlliser = new Person
            {
                Name = "Alliser Thorne",
                Email = "slimybastard@thewall.we"
            };

            var persStannis = new Person
            {
                Name = "Stannis Baratheon",
                Email = "stannisthemannis@dragonstone.we",
                Adress = "Dragonstone, Blackwater bay"
            };

            var custNightWatch = new Customer
            {
                Type = CustomerType.LegalEntity,
                Person = persNightWatch
            };

            var emplJonSnow = new Employee
            {
                Person = persJonSnow
            };

            var emplAlliser = new Employee
            {
                Person = persAlliser
            };

            var projCastleBlackDef = new Project
            {
                Name = "Castle Black defense",
                Description = "Wildlings are about to attack Castle Black. We have to do our best to protect it.",
                Customer = custNightWatch
            };

            var issueNotEnoughMen = new Issue
            {
                Title = "Not enough men.",
                Description = "We don't have enough soldiers to protect The Wall. Maybe we should ask Stannis for help?",
                Status = IssueStatus.New,
                Type = IssueType.Requirement,
                Project = projCastleBlackDef,
                Creator = persJonSnow,
                AssignedEmployee = emplJonSnow,
                Created = DateTime.Now
            };

            var issueJonSnowSucks = new Issue
            {
                Title = "Our Lord Commander is horrible.",
                Description = "He likes wildlings and everything and I am spoiled brat and I don't like him.",
                Status = IssueStatus.New,
                Type = IssueType.Error,
                Project = projCastleBlackDef,
                Creator = persOlly,
                AssignedEmployee = emplAlliser,
                Created = DateTime.Now
            };

            var notifJonSnow = new Notification
            {
                NotifyByEmail = true,
                Person = persJonSnow,
                Issue = issueNotEnoughMen

            };

            var notifStannis = new Notification
            {
                NotifyByEmail = false,
                Person = persStannis,
                Issue = issueNotEnoughMen
            };

            var notifAlliser = new Notification
            {
                NotifyByEmail = true,
                Person = persAlliser,
                Issue = issueJonSnowSucks
            };

            var commStannis = new Comment
            {
                Subject = "I am coming.",
                Message = "I am the one true king and I will come to your aid. Also I'll take my weird priestress I hope it's okay.",
                Created = DateTime.Now,
                Issue = issueNotEnoughMen,
                Author = persStannis
            };

            var commJon = new Comment
            {
                Subject = "RE: I am coming.",
                Message = "Thank you very much, I look forward to meet your priestress.",
                Created = DateTime.Now,
                Issue = issueNotEnoughMen,
                Author = persJonSnow
            };

            var commStannis2 = new Comment
            {
                Subject = "RE: RE: I am coming.",
                Message = "Okay great, have dinner ready.",
                Created = DateTime.Now,
                Issue = issueNotEnoughMen,
                Author = persStannis
            };

            var commAlliser = new Comment
            {
                Subject = "Nothing important.",
                Message = "Olly meet me after sunset, we need to talk.",
                Created = DateTime.Now,
                Issue = issueJonSnowSucks,
                Author = persAlliser
            };

            context.People.Add(persNightWatch);
            context.People.Add(persJonSnow);
            context.People.Add(persAlliser);
            context.People.Add(persOlly);
            context.People.Add(persStannis);
            context.Customers.Add(custNightWatch);
            context.Employees.Add(emplJonSnow);
            context.Employees.Add(emplAlliser);
            context.Projects.Add(projCastleBlackDef);
            context.Issues.Add(issueNotEnoughMen);
            context.Issues.Add(issueJonSnowSucks);
            context.Notifications.Add(notifStannis);
            context.Notifications.Add(notifJonSnow);
            context.Notifications.Add(notifAlliser);
            context.Comments.Add(commStannis);
            context.Comments.Add(commJon);
            context.Comments.Add(commStannis2);
            context.Comments.Add(commAlliser);

            context.Roles.Add(new AITRole { Name = "Administrator" });

            context.SaveChanges();
        }
    }
}
