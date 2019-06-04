namespace MekDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update_user_fields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "Fornavn", c => c.String(nullable: false, maxLength: 50));
            AddColumn("dbo.AspNetUsers", "Efternavn", c => c.String(nullable: false, maxLength: 50));
            AddColumn("dbo.AspNetUsers", "Hold", c => c.String(nullable: false, maxLength: 50));
            AddColumn("dbo.AspNetUsers", "KontaktListen", c => c.String(nullable: false, maxLength: 50));
            AddColumn("dbo.AspNetUsers", "Address_Vej", c => c.String());
            AddColumn("dbo.AspNetUsers", "Address_By", c => c.String());
            AddColumn("dbo.AspNetUsers", "Address_PostNr", c => c.Int(nullable: false));
            AddColumn("dbo.AspNetUsers", "Address_TelefonNr", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "Address_TelefonNr");
            DropColumn("dbo.AspNetUsers", "Address_PostNr");
            DropColumn("dbo.AspNetUsers", "Address_By");
            DropColumn("dbo.AspNetUsers", "Address_Vej");
            DropColumn("dbo.AspNetUsers", "KontaktListen");
            DropColumn("dbo.AspNetUsers", "Hold");
            DropColumn("dbo.AspNetUsers", "Efternavn");
            DropColumn("dbo.AspNetUsers", "Fornavn");
        }
    }
}
