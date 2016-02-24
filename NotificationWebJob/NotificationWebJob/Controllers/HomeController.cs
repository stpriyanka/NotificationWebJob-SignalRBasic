using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Mandrill;
using Mandrill.Models;
using Mandrill.Requests.Messages;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using Newtonsoft.Json;
using NotificationWebJob.Models;
using Enum = NotificationWebJob.Models.Enum;

namespace NotificationWebJob.Controllers
{
	public class HomeController : Controller
	{

		public ActionResult Index()
		{
			//var db = new NotificationDb();
			//foreach (var x in db.LogEventSubscriptionses)
			//{
			//	db.LogEventSubscriptionses.Remove(x);
			//}

			//foreach (var x in db.LogEventses)
			//{
			//	db.LogEventses.Remove(x);
			//}



			//db.SaveChanges();
			return View();
		}

		public ActionResult About()
		{
			ViewBag.Message = "Your application description page.";

			return View();
		}

		public ActionResult Contact()
		{
			ViewBag.Message = "Your contact page.";

			return View();
		}


		public async Task<ActionResult> LogEventSubscriptions()
		{
			var dbContext = new NotificationDb();
			var logEventSub = new LogEventSubscriptions
			{
				Id = Guid.NewGuid(),
				ObjectTypeOfEvent = Enum.ObjectTypeOfEvent.MonthlyMeeting,
				EventType = Enum.EventType.Delete,
				UserWhoSubscribed = "stpriyanka2011@gmail.com"
			};

			dbContext.LogEventSubscriptionses.Add(logEventSub);
			await dbContext.SaveChangesAsync();
			return Content("Created new row");
		}


		public async Task<ActionResult> LogEvents()
		{
			var dbContext = new NotificationDb();
			var logEvent = new LogEvents()
			{
				EventType = Enum.EventType.Delete,
				Id = Guid.NewGuid(),
				UserWhoCreatesEvent = "stpriyanka2011@gmail.com",
				ObjectTypeOfEvent = Enum.ObjectTypeOfEvent.MonthlyMeeting
			};
			dbContext.LogEventses.Add(logEvent);
			await dbContext.SaveChangesAsync();


			var storageAccount = CloudStorageAccount.Parse(ConfigurationManager.ConnectionStrings["AzureWebJobsStorage"].ToString());
			CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();

			CloudQueue queue = queueClient.GetQueueReference("logeventslog");
			queue.CreateIfNotExists();
			var queueMessage = new CloudQueueMessage(JsonConvert.SerializeObject(logEvent));
			queue.AddMessage(queueMessage);


			//List<string> c = dbContext.LogEventSubscriptionses.Where(r => r.EventType == logEvent.EventType
			//	&& r.ObjectTypeOfEvent == logEvent.ObjectTypeOfEvent).Select(r => r.UserWhoSubscribed).ToList();

			//var mandrill = new MandrillApi(ConfigurationManager.AppSettings["MandrillApiKey"]);

			//foreach (var row in c)
			//{
			//	var email = new EmailMessage
			//	{
			//		Text = "body",
			//		FromEmail = "priyanka@worldfavor.com",
			//		To = new List<EmailAddress> {new EmailAddress {Email = row}},
			//		Subject = "Sub"
			//	};

			//	await mandrill.SendMessage(new SendMessageRequest(email));
			//}

			return Content("new row created");


		}


		public async Task<ActionResult> NotificationUserID()
		{
			var notificationContext = new NotificationDb();
			var notificationSignaldbSet = new NotificationSignalR()
			{
				Id = 1,
				RecipientOrganizationId = 4536,
				RecipientUserId = 2,
				Seen = false
			};

			notificationContext.NotificationSignalRs.Add(notificationSignaldbSet);
			await notificationContext.SaveChangesAsync();

			return Content("new row has been created in notificationDBSET");
		}



	}
}