namespace MekDB.Migration.StoreConfiguration
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addordersantal : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BasketLines", "AntalInd", c => c.Int(nullable: false));
            AddColumn("dbo.BasketLines", "AntalUd", c => c.Int(nullable: false));
           
        }
        
        public override void Down()
        {
           
            DropColumn("dbo.BasketLines", "AntalUd");
            DropColumn("dbo.BasketLines", "AntalInd");
        }
    }
}
