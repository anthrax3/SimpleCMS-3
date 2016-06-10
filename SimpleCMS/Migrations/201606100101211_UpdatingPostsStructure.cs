namespace SimpleCMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatingPostsStructure : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Posts", "Visible", c => c.Boolean(nullable: false));
            DropColumn("dbo.Posts", "Category");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Posts", "Category", c => c.String());
            DropColumn("dbo.Posts", "Visible");
        }
    }
}
