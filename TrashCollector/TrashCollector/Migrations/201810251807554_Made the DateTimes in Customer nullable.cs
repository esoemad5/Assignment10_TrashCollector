namespace TrashCollector.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MadetheDateTimesinCustomernullable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Customers", "ExtraPickup", c => c.DateTime());
            AlterColumn("dbo.Customers", "SuspendServiceStart", c => c.DateTime());
            AlterColumn("dbo.Customers", "SuspendServiceEnd", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Customers", "SuspendServiceEnd", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Customers", "SuspendServiceStart", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Customers", "ExtraPickup", c => c.DateTime(nullable: false));
        }
    }
}
