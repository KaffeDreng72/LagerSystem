namespace MekDB.Migration.StoreConfiguration
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RenameBasketTable : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.BasketLines", newName: "BasketLogs");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.BasketLogs", newName: "BasketLines");
        }
    }
}
