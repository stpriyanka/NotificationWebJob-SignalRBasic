using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Mandrill;
using Mandrill.Models;
using Mandrill.Requests.Messages;
using Microsoft.AspNet.SignalR;
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

		//public Task<ActionResult> Contact()
		//{

		//	//var orgID = "4356";
		//	var jobId = Guid.NewGuid();

		
		//	var storageAccount = CloudStorageAccount.Parse(ConfigurationManager.ConnectionStrings["AzureWebJobsStorage"].ConnectionString);
		//	var cloudQueueClient = storageAccount.CreateCloudQueueClient();

		//	var queue = cloudQueueClient.GetQueueReference("jobqueue");
		//	queue.CreateIfNotExists();

		//	CloudQueueMessage message = new CloudQueueMessage(JsonConvert.SerializeObject(jobId));
		//	 queue.AddMessageAsync(message);

		//	ViewBag.Message = "Your contact page.";

		//	return RedirectToAction("Contact", new { jobId });


		//	//return View("Contact");
		//}





		List<object>x =new List<object>(); 

		public async Task<ActionResult> CreateJob()
		{
			const string orgid = "4356";
		
			var jobId = Guid.NewGuid();
			
			var viewModel = new JobStatusViewModel
			{
				JobId = jobId,
				OrgIdFromClient = orgid
			};

			x.Add(viewModel);
			



			var storageAccount = CloudStorageAccount.Parse(ConfigurationManager.ConnectionStrings["AzureWebJobsStorage"].ConnectionString);
			var cloudQueueClient = storageAccount.CreateCloudQueueClient();

			var queue = cloudQueueClient.GetQueueReference("jobqueue");
			queue.CreateIfNotExists();

			CloudQueueMessage message = new CloudQueueMessage(JsonConvert.SerializeObject(x));
			await queue.AddMessageAsync(message);

			return RedirectToAction("JobStatus", new { jobId });
		}
	

		public ActionResult JobStatus(Guid jobId)
		{
			var viewModel = new JobStatusViewModel
			{
				JobId = jobId
				
			};

			return View(viewModel);
		}


		public ActionResult ProgressNotification(Guid jobId, string progress)
		{
			var connections = PassDataToWebJobHub.GetUserConnections(jobId);

			if (connections != null)
			{
				foreach (var connection in connections)
				{
					// Notify the client to refresh the list of connections
					var hubContext = GlobalHost.ConnectionManager.GetHubContext<PassDataToWebJobHub>();
					hubContext.Clients.Clients(new[] { connection }).updateProgress(progress);
				}
			}

			return new HttpStatusCodeResult(HttpStatusCode.OK);
		}




		public async Task<ActionResult> LogEventSubscriptions()
		{
			var dbContext = new NotificationDb();
			var logEventSub = new LogEventSubscriptions
			{
				Id = Guid.NewGuid(),
				ObjectTypeOfEvent = Enum.ObjectTypeOfEvent.MonthlyMeeting,
				EventType = Enum.EventType.Delete,
				UserWhoSubscribed = "priyanka_tasnia@yahoo.com"
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
				UserWhoCreatesEvent = "priyanka_tasnia@yahoo.com",
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