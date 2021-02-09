namespace Ca38Bot.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class GameHistoryField : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Games", "GameHistory", c => c.String());
            DropColumn("dbo.Games", "MMInit");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Games", "MMInit", c => c.Boolean(nullable: false));
            DropColumn("dbo.Games", "GameHistory");
        }
    }
}
