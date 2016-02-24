namespace NotificationWebJob.Models
{
	public class NotificationSignalR
	{
		public int? Id { get; set; }

		public bool Seen { get; set; }

		public int? RecipientUserId { get; set; }

		public int? RecipientOrganizationId { get; set; }

	}
}