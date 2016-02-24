namespace NotificationWebJob.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.LogEvents",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        UserWhoCreatesEvent = c.String(),
                        EventType = c.Int(nullable: false),
                        ObjectTypeOfEvent = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.LogEventSubscriptions",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        UserWhoSubscribed = c.String(),
                        EventType = c.Int(nullable: false),
                        ObjectTypeOfEvent = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.LogEventSubscriptions");
            DropTable("dbo.LogEvents");
        }
    }
}
