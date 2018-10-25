namespace TrashCollector.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedaForeignKeyreferenceinEmployeestoUsers : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Employees", "UserID", c => c.String(maxLength: 128));
            CreateIndex("dbo.Employees", "UserID");
            AddForeignKey("dbo.Employees", "UserID", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Employees", "UserID", "dbo.AspNetUsers");
            DropIndex("dbo.Employees", new[] { "UserID" });
            DropColumn("dbo.Employees", "UserID");
        }
    }
}
