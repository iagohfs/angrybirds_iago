namespace angrybirds_iago.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ContextChanges : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Scores", newName: "ScoresTable");
            DropForeignKey("dbo.PlayerMaps", "Player_PlayerId", "dbo.PlayersTable");
            DropForeignKey("dbo.PlayerMaps", "Map_MapId", "dbo.MapsTable");
            DropForeignKey("dbo.Scores", "Player_PlayerId", "dbo.PlayersTable");
            DropForeignKey("dbo.Scores", "Map_MapId", "dbo.MapsTable");
            DropIndex("dbo.ScoresTable", new[] { "Map_MapId" });
            DropIndex("dbo.ScoresTable", new[] { "Player_PlayerId" });
            DropIndex("dbo.PlayerMaps", new[] { "Player_PlayerId" });
            DropIndex("dbo.PlayerMaps", new[] { "Map_MapId" });
            RenameColumn(table: "dbo.MapsTable", name: "Birds", newName: "BirdsLeft");
            RenameColumn(table: "dbo.ScoresTable", name: "PScore", newName: "PlayerScore");
            AddColumn("dbo.MapsTable", "Player_PlayerId", c => c.Int());
            AlterColumn("dbo.ScoresTable", "Map_MapId", c => c.Int(nullable: false));
            AlterColumn("dbo.ScoresTable", "Player_PlayerId", c => c.Int(nullable: false));
            CreateIndex("dbo.MapsTable", "Player_PlayerId");
            CreateIndex("dbo.ScoresTable", "Map_MapId");
            CreateIndex("dbo.ScoresTable", "Player_PlayerId");
            AddForeignKey("dbo.MapsTable", "Player_PlayerId", "dbo.PlayersTable", "PlayerId");
            AddForeignKey("dbo.ScoresTable", "Player_PlayerId", "dbo.PlayersTable", "PlayerId", cascadeDelete: true);
            AddForeignKey("dbo.ScoresTable", "Map_MapId", "dbo.MapsTable", "MapId", cascadeDelete: true);
            DropTable("dbo.PlayerMaps");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.PlayerMaps",
                c => new
                    {
                        Player_PlayerId = c.Int(nullable: false),
                        Map_MapId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Player_PlayerId, t.Map_MapId });
            
            DropForeignKey("dbo.ScoresTable", "Map_MapId", "dbo.MapsTable");
            DropForeignKey("dbo.ScoresTable", "Player_PlayerId", "dbo.PlayersTable");
            DropForeignKey("dbo.MapsTable", "Player_PlayerId", "dbo.PlayersTable");
            DropIndex("dbo.ScoresTable", new[] { "Player_PlayerId" });
            DropIndex("dbo.ScoresTable", new[] { "Map_MapId" });
            DropIndex("dbo.MapsTable", new[] { "Player_PlayerId" });
            AlterColumn("dbo.ScoresTable", "Player_PlayerId", c => c.Int());
            AlterColumn("dbo.ScoresTable", "Map_MapId", c => c.Int());
            DropColumn("dbo.MapsTable", "Player_PlayerId");
            RenameColumn(table: "dbo.ScoresTable", name: "PlayerScore", newName: "PScore");
            RenameColumn(table: "dbo.MapsTable", name: "BirdsLeft", newName: "Birds");
            CreateIndex("dbo.PlayerMaps", "Map_MapId");
            CreateIndex("dbo.PlayerMaps", "Player_PlayerId");
            CreateIndex("dbo.ScoresTable", "Player_PlayerId");
            CreateIndex("dbo.ScoresTable", "Map_MapId");
            AddForeignKey("dbo.Scores", "Map_MapId", "dbo.MapsTable", "MapId");
            AddForeignKey("dbo.Scores", "Player_PlayerId", "dbo.PlayersTable", "PlayerId");
            AddForeignKey("dbo.PlayerMaps", "Map_MapId", "dbo.MapsTable", "MapId", cascadeDelete: true);
            AddForeignKey("dbo.PlayerMaps", "Player_PlayerId", "dbo.PlayersTable", "PlayerId", cascadeDelete: true);
            RenameTable(name: "dbo.ScoresTable", newName: "Scores");
        }
    }
}
