namespace Bath.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddBathContext : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BathAreas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.BathDatas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        GuestId = c.String(),
                        BathAreaId = c.Int(nullable: false),
                        GuestSate = c.Int(nullable: false),
                        CheckingTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.BathAreas", t => t.BathAreaId, cascadeDelete: true)
                .Index(t => t.BathAreaId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.BathDatas", "BathAreaId", "dbo.BathAreas");
            DropIndex("dbo.BathDatas", new[] { "BathAreaId" });
            DropTable("dbo.BathDatas");
            DropTable("dbo.BathAreas");
        }
    }
}
