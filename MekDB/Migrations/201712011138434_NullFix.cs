namespace MekDB.Migration.StoreConfiguration
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NullFix : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Locations", "LokationName", c => c.String(nullable: false));
            AlterColumn("dbo.Suppliers", "Name", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Suppliers", "Name", c => c.String());
            AlterColumn("dbo.Locations", "LokationName", c => c.String());
        }
    }
}
