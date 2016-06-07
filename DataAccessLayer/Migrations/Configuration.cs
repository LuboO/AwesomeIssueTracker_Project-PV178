namespace DataAccessLayer.Migrations
{
    using Entities;
    using Enums;
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
            context.Roles.Add(new AITRole() { Name = UserRole.Administrator.ToString() });
            context.Roles.Add(new AITRole() { Name = UserRole.Employee.ToString() });
            context.Roles.Add(new AITRole() { Name = UserRole.Customer.ToString() });

            context.SaveChanges();
        }
    }
}
