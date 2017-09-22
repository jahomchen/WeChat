namespace JahomWeChat.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class jahom1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Records", "IsSpecial", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Records", "IsSpecial");
        }
    }
}
