namespace angrybirds_iago.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class smallchanges : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MapsTable", "BirdsAvailable", c => c.Int(nullable: false));
            DropColumn("dbo.MapsTable", "BirdsLeft");
        }
        
        public override void Down()
        {
            AddColumn("dbo.MapsTable", "BirdsLeft", c => c.Int(nullable: false));
            DropColumn("dbo.MapsTable", "BirdsAvailable");
        }
    }
}
