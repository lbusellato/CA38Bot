namespace Ca38Bot.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MMInitField : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Games", "MMInit", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Games", "MMInit");
        }
    }
}
