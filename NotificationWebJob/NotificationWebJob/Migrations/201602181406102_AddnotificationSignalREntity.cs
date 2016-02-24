namespace NotificationWebJob.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddnotificationSignalREntity : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.NotificationSignalRs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Seen = c.Boolean(nullable: false),
                        RecipientUserId = c.Int(),
                        RecipientOrganizationId = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.NotificationSignalRs");
        }
    }
}
