namespace DataAccessLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserExtended : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Comments", "AITUser_Id", c => c.Int());
            AddColumn("dbo.Issues", "AITUser_Id", c => c.Int());
            AddColumn("dbo.Notifications", "AITUser_Id", c => c.Int());
            AddColumn("dbo.AspNetUsers", "Adress", c => c.String(maxLength: 256));
            AddColumn("dbo.AspNetUsers", "DateOfBirth", c => c.DateTime());
            AddColumn("dbo.AspNetUsers", "Customer_Id", c => c.Int());
            AddColumn("dbo.AspNetUsers", "Employee_Id", c => c.Int());
            AlterColumn("dbo.AspNetUsers", "Email", c => c.String(nullable: false, maxLength: 256));
            AlterColumn("dbo.AspNetUsers", "PhoneNumber", c => c.String(maxLength: 64));
            CreateIndex("dbo.Comments", "AITUser_Id");
            CreateIndex("dbo.Issues", "AITUser_Id");
            CreateIndex("dbo.Notifications", "AITUser_Id");
            CreateIndex("dbo.AspNetUsers", "Email", unique: true);
            CreateIndex("dbo.AspNetUsers", "Customer_Id");
            CreateIndex("dbo.AspNetUsers", "Employee_Id");
            AddForeignKey("dbo.Comments", "AITUser_Id", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.AspNetUsers", "Customer_Id", "dbo.Customers", "Id");
            AddForeignKey("dbo.AspNetUsers", "Employee_Id", "dbo.Employees", "Id");
            AddForeignKey("dbo.Issues", "AITUser_Id", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Notifications", "AITUser_Id", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Notifications", "AITUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Issues", "AITUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUsers", "Employee_Id", "dbo.Employees");
            DropForeignKey("dbo.AspNetUsers", "Customer_Id", "dbo.Customers");
            DropForeignKey("dbo.Comments", "AITUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.AspNetUsers", new[] { "Employee_Id" });
            DropIndex("dbo.AspNetUsers", new[] { "Customer_Id" });
            DropIndex("dbo.AspNetUsers", new[] { "Email" });
            DropIndex("dbo.Notifications", new[] { "AITUser_Id" });
            DropIndex("dbo.Issues", new[] { "AITUser_Id" });
            DropIndex("dbo.Comments", new[] { "AITUser_Id" });
            AlterColumn("dbo.AspNetUsers", "PhoneNumber", c => c.String());
            AlterColumn("dbo.AspNetUsers", "Email", c => c.String(maxLength: 256));
            DropColumn("dbo.AspNetUsers", "Employee_Id");
            DropColumn("dbo.AspNetUsers", "Customer_Id");
            DropColumn("dbo.AspNetUsers", "DateOfBirth");
            DropColumn("dbo.AspNetUsers", "Adress");
            DropColumn("dbo.Notifications", "AITUser_Id");
            DropColumn("dbo.Issues", "AITUser_Id");
            DropColumn("dbo.Comments", "AITUser_Id");
        }
    }
}
