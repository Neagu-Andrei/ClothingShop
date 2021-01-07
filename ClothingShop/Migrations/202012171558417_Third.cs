namespace ClothingShop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Third : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Reviews", "UserId", c => c.String(maxLength: 128));
            CreateIndex("dbo.Reviews", "UserId");
            AddForeignKey("dbo.Reviews", "UserId", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Reviews", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.Reviews", new[] { "UserId" });
            DropColumn("dbo.Reviews", "UserId");
        }
    }
}
