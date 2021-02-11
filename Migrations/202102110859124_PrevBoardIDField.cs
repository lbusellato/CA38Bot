namespace Ca38Bot.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PrevBoardIDField : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Games", "PrevBoardID", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Games", "PrevBoardID");
        }
    }
}
