namespace ClothingShop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Secondary : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Products", "FileId", "dbo.Files");
            DropIndex("dbo.Products", new[] { "FileId" });
            DropColumn("dbo.Products", "FileId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Products", "FileId", c => c.Int(nullable: false));
            CreateIndex("dbo.Products", "FileId");
            AddForeignKey("dbo.Products", "FileId", "dbo.Files", "FileId", cascadeDelete: true);
        }
    }
}
