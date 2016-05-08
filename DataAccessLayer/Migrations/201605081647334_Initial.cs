namespace DataAccessLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Comments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Subject = c.String(maxLength: 256),
                        Message = c.String(nullable: false),
                        Created = c.DateTime(nullable: false),
                        IssueId = c.Int(nullable: false),
                        AuthorId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.People", t => t.AuthorId)
                .ForeignKey("dbo.Issues", t => t.IssueId)
                .Index(t => t.IssueId)
                .Index(t => t.AuthorId);
            
            CreateTable(
                "dbo.People",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 64),
                        Email = c.String(nullable: false, maxLength: 64),
                        Adress = c.String(maxLength: 256),
                        Phone = c.String(maxLength: 64),
                        DateOfBirth = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Customers",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Type = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.People", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.Projects",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 256),
                        Description = c.String(),
                        CustomerId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Customers", t => t.CustomerId)
                .Index(t => t.CustomerId);
            
            CreateTable(
                "dbo.Issues",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 256),
                        Description = c.String(),
                        Type = c.Int(nullable: false),
                        Status = c.Int(nullable: false),
                        Created = c.DateTime(nullable: false),
                        Finished = c.DateTime(),
                        ProjectId = c.Int(nullable: false),
                        AssignedEmployeeId = c.Int(nullable: false),
                        CreatorId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Employees", t => t.AssignedEmployeeId)
                .ForeignKey("dbo.People", t => t.CreatorId)
                .ForeignKey("dbo.Projects", t => t.ProjectId)
                .Index(t => t.ProjectId)
                .Index(t => t.AssignedEmployeeId)
                .Index(t => t.CreatorId);
            
            CreateTable(
                "dbo.Employees",
                c => new
                    {
                        Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.People", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.Notifications",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        NotifyByEmail = c.Boolean(nullable: false),
                        IssueId = c.Int(nullable: false),
                        PersonId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Issues", t => t.IssueId)
                .ForeignKey("dbo.People", t => t.PersonId)
                .Index(t => t.IssueId)
                .Index(t => t.PersonId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Comments", "IssueId", "dbo.Issues");
            DropForeignKey("dbo.Comments", "AuthorId", "dbo.People");
            DropForeignKey("dbo.Issues", "ProjectId", "dbo.Projects");
            DropForeignKey("dbo.Notifications", "PersonId", "dbo.People");
            DropForeignKey("dbo.Notifications", "IssueId", "dbo.Issues");
            DropForeignKey("dbo.Issues", "CreatorId", "dbo.People");
            DropForeignKey("dbo.Issues", "AssignedEmployeeId", "dbo.Employees");
            DropForeignKey("dbo.Employees", "Id", "dbo.People");
            DropForeignKey("dbo.Projects", "CustomerId", "dbo.Customers");
            DropForeignKey("dbo.Customers", "Id", "dbo.People");
            DropIndex("dbo.Notifications", new[] { "PersonId" });
            DropIndex("dbo.Notifications", new[] { "IssueId" });
            DropIndex("dbo.Employees", new[] { "Id" });
            DropIndex("dbo.Issues", new[] { "CreatorId" });
            DropIndex("dbo.Issues", new[] { "AssignedEmployeeId" });
            DropIndex("dbo.Issues", new[] { "ProjectId" });
            DropIndex("dbo.Projects", new[] { "CustomerId" });
            DropIndex("dbo.Customers", new[] { "Id" });
            DropIndex("dbo.Comments", new[] { "AuthorId" });
            DropIndex("dbo.Comments", new[] { "IssueId" });
            DropTable("dbo.Notifications");
            DropTable("dbo.Employees");
            DropTable("dbo.Issues");
            DropTable("dbo.Projects");
            DropTable("dbo.Customers");
            DropTable("dbo.People");
            DropTable("dbo.Comments");
        }
    }
}
