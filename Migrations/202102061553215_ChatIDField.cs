namespace Ca38Bot.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChatIDField : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Games", "ChatID", c => c.Long(nullable: false));
            DropColumn("dbo.Games", "TGID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Games", "TGID", c => c.String());
            DropColumn("dbo.Games", "ChatID");
        }
    }
}
