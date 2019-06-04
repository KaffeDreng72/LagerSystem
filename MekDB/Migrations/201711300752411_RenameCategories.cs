namespace MekDB.Migration.StoreConfiguration
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RenameCategories : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Categories", newName: "Locations");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.Locations", newName: "Categories");
        }
    }
}
