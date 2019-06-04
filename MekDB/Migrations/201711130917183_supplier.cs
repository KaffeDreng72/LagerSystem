namespace MekDB.Migration.StoreConfiguration
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class supplier : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Suppliers",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            AddColumn("dbo.Products", "SupplierID", c => c.Int());
            AddColumn("dbo.Products", "Suppliers_ID", c => c.Int());
            CreateIndex("dbo.Products", "Suppliers_ID");
            AddForeignKey("dbo.Products", "Suppliers_ID", "dbo.Suppliers", "ID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Products", "Suppliers_ID", "dbo.Suppliers");
            DropIndex("dbo.Products", new[] { "Suppliers_ID" });
            DropColumn("dbo.Products", "Suppliers_ID");
            DropColumn("dbo.Products", "SupplierID");
            DropTable("dbo.Suppliers");
        }
    }
}
