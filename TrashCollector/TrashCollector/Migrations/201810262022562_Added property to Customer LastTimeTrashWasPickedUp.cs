namespace TrashCollector.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedpropertytoCustomerLastTimeTrashWasPickedUp : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Customers", "LastTimeTrashWasPickedUp", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Customers", "LastTimeTrashWasPickedUp");
        }
    }
}
