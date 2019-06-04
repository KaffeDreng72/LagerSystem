namespace MekDB.Migration.StoreConfiguration
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addorders : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Orders",
                c => new
                    {
                        OrderID = c.Int(nullable: false, identity: true),
                        VareNr = c.String(),
                        ProduktNavn = c.String(),
                        AntalInd = c.Int(nullable: false),
                        AntalUd = c.Int(nullable: false),
                        UserID = c.String(),
                        DeliveryName = c.String(),
                        DateOgTid = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.OrderID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Orders");
        }
    }
}
