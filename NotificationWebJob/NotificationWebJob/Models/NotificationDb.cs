using System.Data.Entity;

namespace NotificationWebJob.Models
{
	public class NotificationDb : DbContext
	{

		public DbSet<LogEvents> LogEventses { get; set; }
		public DbSet<LogEventSubscriptions> LogEventSubscriptionses { get; set; }
		public DbSet<NotificationSignalR> NotificationSignalRs { get; set; }

	}
}