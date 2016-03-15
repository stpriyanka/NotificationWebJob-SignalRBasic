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
using Website;


namespace NotificationWebJob
{


	[Authorize]
	public class SignalRHub : Hub
	{
		readonly NotificationDb _myDbContext = new NotificationDb();

		private readonly static ConnectionMapping<Guid> Connections = new ConnectionMapping<Guid>();


		private static int _connectionCounter = 0;


		public async Task SendMessage(string message)
		{
			const int orgid = 4356;

			////receive data from client state
			//string myid = clients.caller.id;

			var version = Context.QueryString["orgid"];
			if (version != orgid.ToString())
			{
				Clients.Caller.send(version);
			}

			var name = _myDbContext.LogEventSubscriptionses.FirstOrDefault
						 (r => r.UserWhoSubscribed == Context.User.Identity.Name);


			var obj = new LogEvents()
			{
				Id = name.Id,
				//userwhocreatesevent = name.userwhosubscribed,
				//eventtype = name.eventtype,
				//objecttypeofevent = name.objecttypeofevent

			};

			var json = new JavaScriptSerializer().Serialize(obj);
			Clients.Caller.send(json);

			//var version = Context.QueryString["orgID"];
			//Guid orgId = GetOrgId();


			Clients.All.send(version);
		}



		public override Task OnConnected()
		{
			_connectionCounter++;

			//Clients.All.send(_connectionCounter);

			Clients.All.send("Current active number of connection " + _connectionCounter);

			return base.OnConnected();
		}


		public override Task OnDisconnected(bool stopCalled)
		{

			_connectionCounter--;

			
			return base.OnDisconnected(stopCalled);
		}

	
	}
}