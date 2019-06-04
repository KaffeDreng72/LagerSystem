namespace MekDB.Migration.StoreConfiguration
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removedoldsupplier : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Products", "Leverandør");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Products", "Leverandør", c => c.String());
        }
    }
}
