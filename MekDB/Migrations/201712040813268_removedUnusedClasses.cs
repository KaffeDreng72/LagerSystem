namespace MekDB.Migration.StoreConfiguration
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removedUnusedClasses : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ProductImageMappings", "ProductID", "dbo.Products");
            DropForeignKey("dbo.ProductImageMappings", "productImageID", "dbo.ProductImages");
            DropIndex("dbo.ProductImageMappings", new[] { "ProductID" });
            DropIndex("dbo.ProductImageMappings", new[] { "productImageID" });
            DropIndex("dbo.ProductImages", new[] { "FileName" });
            //DropTable("dbo.ProductImageMappings");
            //DropTable("dbo.ProductImages");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.ProductImages",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        FileName = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.ProductImageMappings",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ImageNumber = c.Int(nullable: false),
                        ProductID = c.Int(nullable: false),
                        productImageID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateIndex("dbo.ProductImages", "FileName", unique: true);
            CreateIndex("dbo.ProductImageMappings", "productImageID");
            CreateIndex("dbo.ProductImageMappings", "ProductID");
            AddForeignKey("dbo.ProductImageMappings", "productImageID", "dbo.ProductImages", "ID", cascadeDelete: true);
            AddForeignKey("dbo.ProductImageMappings", "ProductID", "dbo.Products", "ID", cascadeDelete: true);
        }
    }
}
