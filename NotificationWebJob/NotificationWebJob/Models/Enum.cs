using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NotificationWebJob.Models
{
	public class Enum
	{
		public enum EventType
		{
			Add = 1,
			Delete=2
		}

		public enum ObjectTypeOfEvent
		{
			WeeklyMeeting = 1,
			MonthlyMeeting = 2
		}
	}
}