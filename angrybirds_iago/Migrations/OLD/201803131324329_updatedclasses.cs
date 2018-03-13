namespace angrybirds_iago.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatedclasses : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Scores",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PScore = c.Int(nullable: false),
                        Map_MapId = c.Int(),
                        Player_PlayerId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.MapsTable", t => t.Map_MapId)
                .ForeignKey("dbo.PlayersTable", t => t.Player_PlayerId)
                .Index(t => t.Map_MapId)
                .Index(t => t.Player_PlayerId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Scores", "Player_PlayerId", "dbo.PlayersTable");
            DropForeignKey("dbo.Scores", "Map_MapId", "dbo.MapsTable");
            DropIndex("dbo.Scores", new[] { "Player_PlayerId" });
            DropIndex("dbo.Scores", new[] { "Map_MapId" });
            DropTable("dbo.Scores");
        }
    }
}
