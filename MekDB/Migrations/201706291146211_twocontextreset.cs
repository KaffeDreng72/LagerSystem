namespace MekDB.Migration.StoreConfiguration
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class twocontextreset : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        LokationName = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Products",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        VareNr = c.String(),
                        ProduktNavn = c.String(),
                        DanskNavn = c.String(),
                        Leverandør = c.String(),
                        LagerStatus = c.Int(nullable: false),
                        BestilVed = c.Int(nullable: false),
                        Bemærkning = c.String(),
                        CategoryID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Categories", t => t.CategoryID)
                .Index(t => t.CategoryID);
            
            CreateTable(
                "dbo.ProductImageMappings",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ImageNumber = c.Int(nullable: false),
                        ProductID = c.Int(nullable: false),
                        productImageID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Products", t => t.ProductID, cascadeDelete: true)
                .ForeignKey("dbo.ProductImages", t => t.productImageID, cascadeDelete: true)
                .Index(t => t.ProductID)
                .Index(t => t.productImageID);
            
            CreateTable(
                "dbo.ProductImages",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        FileName = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => t.ID)
                .Index(t => t.FileName, unique: true);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ProductImageMappings", "productImageID", "dbo.ProductImages");
            DropForeignKey("dbo.ProductImageMappings", "ProductID", "dbo.Products");
            DropForeignKey("dbo.Products", "CategoryID", "dbo.Categories");
            DropIndex("dbo.ProductImages", new[] { "FileName" });
            DropIndex("dbo.ProductImageMappings", new[] { "productImageID" });
            DropIndex("dbo.ProductImageMappings", new[] { "ProductID" });
            DropIndex("dbo.Products", new[] { "CategoryID" });
            DropTable("dbo.ProductImages");
            DropTable("dbo.ProductImageMappings");
            DropTable("dbo.Products");
            DropTable("dbo.Categories");
        }
    }
}
