using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using Microsoft.Azure.WebJobs;
using Newtonsoft.Json;
using NotificationWebJob.Models;
using Enum = NotificationWebJob.Models.Enum;

namespace SignalRPassDataWebJob
{
	public class Functions
	{
		// This function will get triggered/executed when a new message is written 
		// on an Azure Queue called queue.
		public static async Task ProcessQueueMessage([QueueTrigger("jobqueue")] string jobId, TextWriter log)
		{

			desirialize sDesirialize = new desirialize();
			var s = sDesirialize.jsondeserializer(jobId);

			Console.WriteLine(s.JobId);

			//char[] delimiterChars = { '}', ',', '.', ':', '\t','{','[',']' };
			//string text = jobId;

			//Console.WriteLine(jobId);

			//string[] words = text.Split(delimiterChars);
			//foreach (string s in words)
			//{
			//	System.Console.WriteLine(s);
			//}



			//for (int i = 10; i <= 100; i += 10)
			//{
			//	Thread.Sleep(400);

			//await CommunicateProgress(jobId.JobId, jobId.OrgIdFromClient);
			//Console.WriteLine(msg);
			//}
		}


		private static async Task CommunicateProgress(Guid jobId, string percentage)
		{
			var httpClient = new HttpClient();

			var queryString = String.Format("?jobId={0}&progress={1}", jobId, percentage);
			var request = ConfigurationManager.AppSettings["ProgressNotificationEndpoint"] + queryString; //appsetting

			await httpClient.GetAsync(request);
		}
	}
	public class desirialize
	{
		public JobStatusViewModel jsondeserializer(string x)
		{
			return JsonConvert.DeserializeObject<JobStatusViewModel>(x);
		}

	}
}
