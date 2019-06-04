namespace MekDB.Migration.StoreConfiguration
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addImagesTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Images",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        FullFileName = c.String(),
                        Image = c.Binary(),
                    })
                .PrimaryKey(t => t.ID);
            
            AddColumn("dbo.Products", "ImagesID", c => c.Int());
            CreateIndex("dbo.Products", "ImagesID");
            AddForeignKey("dbo.Products", "ImagesID", "dbo.Images", "ID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Products", "ImagesID", "dbo.Images");
            DropIndex("dbo.Products", new[] { "ImagesID" });
            DropColumn("dbo.Products", "ImagesID");
            DropTable("dbo.Images");
        }
    }
}
