namespace MekDB.Migration.StoreConfiguration
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addImageToTable : DbMigration
    {
        public override void Up()
        {
           
            AddColumn("dbo.Products", "Image", c => c.Binary());
           
        }
        
        public override void Down()
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
            DropColumn("dbo.Products", "Image");
            CreateIndex("dbo.Products", "ImagesID");
            AddForeignKey("dbo.Products", "ImagesID", "dbo.Images", "ID");
        }
    }
}
