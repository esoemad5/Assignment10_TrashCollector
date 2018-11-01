namespace TrashCollector.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedLatitudeLongitudeupdateLatitudeAndLongitudeAsyncandsomeprivatethingstoCustomercsmodel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Customers", "Latitude", c => c.Single());
            AddColumn("dbo.Customers", "Longitude", c => c.Single());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Customers", "Longitude");
            DropColumn("dbo.Customers", "Latitude");
        }
    }
}
