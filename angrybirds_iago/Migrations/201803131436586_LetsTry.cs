namespace angrybirds_iago.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LetsTry : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MapsTable",
                c => new
                    {
                        MapId = c.Int(nullable: false, identity: true),
                        BirdsLeft = c.Int(nullable: false),
                        MapName = c.String(maxLength: 32),
                        Player_PlayerId = c.Int(),
                    })
                .PrimaryKey(t => t.MapId)
                .ForeignKey("dbo.PlayersTable", t => t.Player_PlayerId)
                .Index(t => t.Player_PlayerId);
            
            CreateTable(
                "dbo.ScoresTable",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PlayerScore = c.Int(nullable: false),
                        Map_MapId = c.Int(nullable: false),
                        Player_PlayerId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.MapsTable", t => t.Map_MapId, cascadeDelete: true)
                .ForeignKey("dbo.PlayersTable", t => t.Player_PlayerId, cascadeDelete: true)
                .Index(t => t.Map_MapId)
                .Index(t => t.Player_PlayerId);
            
            CreateTable(
                "dbo.PlayersTable",
                c => new
                    {
                        PlayerId = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 32),
                    })
                .PrimaryKey(t => t.PlayerId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ScoresTable", "Player_PlayerId", "dbo.PlayersTable");
            DropForeignKey("dbo.MapsTable", "Player_PlayerId", "dbo.PlayersTable");
            DropForeignKey("dbo.ScoresTable", "Map_MapId", "dbo.MapsTable");
            DropIndex("dbo.ScoresTable", new[] { "Player_PlayerId" });
            DropIndex("dbo.ScoresTable", new[] { "Map_MapId" });
            DropIndex("dbo.MapsTable", new[] { "Player_PlayerId" });
            DropTable("dbo.PlayersTable");
            DropTable("dbo.ScoresTable");
            DropTable("dbo.MapsTable");
        }
    }
}
