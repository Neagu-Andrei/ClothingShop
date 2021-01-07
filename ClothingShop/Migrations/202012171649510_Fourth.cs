namespace ClothingShop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Fourth : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "FileId", c => c.Int(nullable: false));
            CreateIndex("dbo.Products", "FileId");
            AddForeignKey("dbo.Products", "FileId", "dbo.Files", "FileId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Products", "FileId", "dbo.Files");
            DropIndex("dbo.Products", new[] { "FileId" });
            DropColumn("dbo.Products", "FileId");
        }
    }
}
