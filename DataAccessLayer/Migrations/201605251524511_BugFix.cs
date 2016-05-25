namespace DataAccessLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BugFix : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.People", "Address", c => c.String(maxLength: 256));
            AddColumn("dbo.AspNetUsers", "Address", c => c.String(maxLength: 256));
            DropColumn("dbo.People", "Adress");
            DropColumn("dbo.AspNetUsers", "Adress");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "Adress", c => c.String(maxLength: 256));
            AddColumn("dbo.People", "Adress", c => c.String(maxLength: 256));
            DropColumn("dbo.AspNetUsers", "Address");
            DropColumn("dbo.People", "Address");
        }
    }
}
