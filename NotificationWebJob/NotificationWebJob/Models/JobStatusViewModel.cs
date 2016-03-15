using System;
using System.ComponentModel.DataAnnotations;

namespace NotificationWebJob.Models
{
	public class JobStatusViewModel 
	{
		
		public Guid JobId { get; set; }
		public string OrgIdFromClient { get; set; }

	}
}