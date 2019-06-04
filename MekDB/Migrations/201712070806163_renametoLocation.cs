namespace MekDB.Migration.StoreConfiguration
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class renametoLocation : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Products", name: "CategoryID", newName: "LocationID");
            RenameIndex(table: "dbo.Products", name: "IX_CategoryID", newName: "IX_LocationID");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Products", name: "IX_LocationID", newName: "IX_CategoryID");
            RenameColumn(table: "dbo.Products", name: "LocationID", newName: "CategoryID");
        }
    }
}
