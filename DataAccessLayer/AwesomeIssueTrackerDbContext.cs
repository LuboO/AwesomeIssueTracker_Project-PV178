using System.Data.Entity;
using DataAccessLayer.Entities;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace DataAccessLayer
{
    public class AwesomeIssueTrackerDbContext : DbContext
    {
        public AwesomeIssueTrackerDbContext() : base("AITAppDB")
        {
            Database.SetInitializer(new DataInitializer());
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Just to turn off default cascading delete that would create multiple delete paths
            // and therefore mess up SQL database...
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Comment> Comments { get; set; }

        public DbSet<Customer> Customers { get; set; }

        public DbSet<Employee> Employees { get; set; }

        public DbSet<Issue> Issues { get; set; }

        public DbSet<Notification> Notifications { get; set; }

        public DbSet<Person> People { get; set; }

        public DbSet<Project> Projects { get; set; }
    }
}
