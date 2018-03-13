namespace angrybirds_iago.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class m2m : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MapsTable",
                c => new
                    {
                        MapId = c.Int(nullable: false, identity: true),
                        Birds = c.Int(nullable: false),
                        MapName = c.String(maxLength: 32),
                    })
                .PrimaryKey(t => t.MapId);
            
            CreateTable(
                "dbo.PlayersTable",
                c => new
                    {
                        PlayerId = c.Int(nullable: false, identity: true),
                        PlayerName = c.String(maxLength: 32),
                    })
                .PrimaryKey(t => t.PlayerId);
            
            CreateTable(
                "dbo.PlayerMaps",
                c => new
                    {
                        Player_PlayerId = c.Int(nullable: false),
                        Map_MapId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Player_PlayerId, t.Map_MapId })
                .ForeignKey("dbo.PlayersTable", t => t.Player_PlayerId, cascadeDelete: true)
                .ForeignKey("dbo.MapsTable", t => t.Map_MapId, cascadeDelete: true)
                .Index(t => t.Player_PlayerId)
                .Index(t => t.Map_MapId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PlayerMaps", "Map_MapId", "dbo.MapsTable");
            DropForeignKey("dbo.PlayerMaps", "Player_PlayerId", "dbo.PlayersTable");
            DropIndex("dbo.PlayerMaps", new[] { "Map_MapId" });
            DropIndex("dbo.PlayerMaps", new[] { "Player_PlayerId" });
            DropTable("dbo.PlayerMaps");
            DropTable("dbo.PlayersTable");
            DropTable("dbo.MapsTable");
        }
    }
}
