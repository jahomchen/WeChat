namespace JahomWeChat.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class jahom : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Records",
                c => new
                    {
                        ID = c.Guid(nullable: false, identity: true),
                        CreateTime = c.DateTime(nullable: false),
                        ModifyTime = c.DateTime(nullable: false),
                        UserId = c.Guid(nullable: false),
                        UserName = c.String(),
                        Title = c.String(),
                        Content = c.String(),
                        Summary = c.String(),
                        Tips = c.String(),
                        IsCompleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Replies",
                c => new
                    {
                        ID = c.Guid(nullable: false, identity: true),
                        CreateTime = c.DateTime(nullable: false),
                        ModifyTime = c.DateTime(nullable: false),
                        PId = c.Guid(nullable: false),
                        RecordId = c.Guid(nullable: false),
                        FromUserId = c.Guid(nullable: false),
                        FromUserName = c.String(),
                        ToUserId = c.Guid(nullable: false),
                        ToUserName = c.String(),
                        Message = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        ID = c.Guid(nullable: false, identity: true),
                        CreateTime = c.DateTime(nullable: false),
                        ModifyTime = c.DateTime(nullable: false),
                        OpenId = c.String(),
                        UserName = c.String(),
                        Email = c.String(),
                        Sign = c.String(),
                        Gender = c.Int(nullable: false),
                        Tips = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Users");
            DropTable("dbo.Replies");
            DropTable("dbo.Records");
        }
    }
}
