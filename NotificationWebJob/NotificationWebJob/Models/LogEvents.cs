using System;

namespace NotificationWebJob.Models
{
	public class LogEvents
	{
		public Guid Id { get; set; }
		public string UserWhoCreatesEvent { get; set; }

		public Enum.EventType EventType { get; set; }

		public Enum.ObjectTypeOfEvent ObjectTypeOfEvent { get; set; }
	}
}