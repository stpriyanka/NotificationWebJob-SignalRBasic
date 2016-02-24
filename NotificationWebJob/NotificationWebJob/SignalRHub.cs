using System.Data.Entity;
using System.Web.Script.Serialization;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NotificationWebJob.Models;


namespace NotificationWebJob
{


	[Authorize]
	public class SignalRHub : Hub
	{
		readonly NotificationDb _myDbContext = new NotificationDb();

		private static int _connectionCounter = 0;


		public async Task SendMessage(string message)
		{
			//const int orgid = 4356;


			//var version = Context.QueryString["orgID"];
			//if (version != orgid.ToString())
			//{
			//	Clients.Caller.send(version);
			//}

			//var name = _myDbContext.LogEventSubscriptionses.FirstOrDefault
			//			 (r => r.UserWhoSubscribed == Context.User.Identity.Name);


			//var obj = new LogEvents()
			//{
			//	Id = name.Id,
			//	UserWhoCreatesEvent = name.UserWhoSubscribed,
			//	EventType = name.EventType,
			//	ObjectTypeOfEvent = name.ObjectTypeOfEvent

			//};

			//var json = new JavaScriptSerializer().Serialize(obj);
			//Clients.All.send(json);

			Clients.Caller.send(message);
		}



		public override Task OnConnected()
		{
			_connectionCounter++;

			Clients.All.send(_connectionCounter);
			
			//Clients.All.send("Current active number of connection " + _connectionCounter);


			return base.OnConnected();
		}


		public override Task OnDisconnected(bool stopCalled)
		{

			_connectionCounter--;

			return base.OnDisconnected(stopCalled);
		}
	}

}