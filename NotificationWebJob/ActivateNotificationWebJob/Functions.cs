using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mandrill;
using Mandrill.Models;
using Mandrill.Requests.Messages;
using Microsoft.Azure.WebJobs;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using NotificationWebJob.Models;

namespace ActivateNotificationWebJob
{
	public class Functions
	{
		public static void ProcessQueueMessage([QueueTrigger("logeventslog")] LogEvents logEvent, TextWriter log)
		{
			var storageAccount = CloudStorageAccount.Parse(ConfigurationManager.ConnectionStrings["AzureWebJobsStorage"].ToString());
			var blobClient = storageAccount.CreateCloudBlobClient();

			var dbContext = new NotificationDb();

			List<string> c = dbContext.LogEventSubscriptionses.Where(r => r.EventType == logEvent.EventType
							 && r.ObjectTypeOfEvent == logEvent.ObjectTypeOfEvent)
							 .Select(r => r.UserWhoSubscribed).ToList();
			
			
			var y = new LogEventSubscriptions()
			
			{
				UserWhoSubscribed = "priyanka_tasnia@yahoo.com",
				Id = Guid.NewGuid()
			};

			dbContext.LogEventSubscriptionses.Add(y);
			dbContext.SaveChanges();

			//var mandrill = new MandrillApi(ConfigurationManager.AppSettings["MandrillApiKey"]);

			//foreach (var row in c)
			//{
			//	var email = new EmailMessage
			//	{
			//		Text = "body",
			//		FromEmail = "priyanka@worldfavor.com",
			//		To = new List<EmailAddress> { new EmailAddress { Email = row } },
			//		Subject = "Sub"
			//	};

			//	 mandrill.SendMessage(new SendMessageRequest(email));
			//}
		}
	}
}
