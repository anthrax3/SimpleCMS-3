namespace SimpleCMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Variousdbproperties : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Comments", "Posts_ID", "dbo.Posts");
            DropIndex("dbo.Comments", new[] { "Posts_ID" });
            CreateTable(
                "dbo.PostsComments",
                c => new
                    {
                        Posts_ID = c.Int(nullable: false),
                        Comments_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Posts_ID, t.Comments_ID })
                .ForeignKey("dbo.Posts", t => t.Posts_ID, cascadeDelete: true)
                .ForeignKey("dbo.Comments", t => t.Comments_ID, cascadeDelete: true)
                .Index(t => t.Posts_ID)
                .Index(t => t.Comments_ID);
            
            AddColumn("dbo.Comments", "childCommentID", c => c.Int(nullable: false));
            AddColumn("dbo.Posts", "AttachmentPath", c => c.String());
            AddColumn("dbo.Posts", "ApplicationUser_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.Posts", "ApplicationUser_Id");
            AddForeignKey("dbo.Posts", "ApplicationUser_Id", "dbo.AspNetUsers", "Id");
            DropColumn("dbo.Comments", "Posts_ID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Comments", "Posts_ID", c => c.Int());
            DropForeignKey("dbo.PostsComments", "Comments_ID", "dbo.Comments");
            DropForeignKey("dbo.PostsComments", "Posts_ID", "dbo.Posts");
            DropForeignKey("dbo.Posts", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.PostsComments", new[] { "Comments_ID" });
            DropIndex("dbo.PostsComments", new[] { "Posts_ID" });
            DropIndex("dbo.Posts", new[] { "ApplicationUser_Id" });
            DropColumn("dbo.Posts", "ApplicationUser_Id");
            DropColumn("dbo.Posts", "AttachmentPath");
            DropColumn("dbo.Comments", "childCommentID");
            DropTable("dbo.PostsComments");
            CreateIndex("dbo.Comments", "Posts_ID");
            AddForeignKey("dbo.Comments", "Posts_ID", "dbo.Posts", "ID");
        }
    }
}
