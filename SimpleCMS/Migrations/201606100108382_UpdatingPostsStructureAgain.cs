namespace SimpleCMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatingPostsStructureAgain : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Posts", "Attachment", c => c.Boolean());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Posts", "Attachment", c => c.Boolean(nullable: false));
        }
    }
}
