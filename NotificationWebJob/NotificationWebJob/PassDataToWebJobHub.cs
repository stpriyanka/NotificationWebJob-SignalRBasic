using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.SignalR;
using Website;
using Microsoft.Ajax.Utilities;

namespace NotificationWebJob
{
	public class PassDataToWebJobHub : Hub
	{
		private readonly static ConnectionMapping<Guid> Connections = new ConnectionMapping<Guid>();

		public override Task OnConnected()
		{
			Guid jobid = GetJobId();

			//var version = Context.QueryString["orgid"];

			Connections.Add(jobid, Context.ConnectionId);

			return base.OnConnected();
		}

		public override Task OnDisconnected(bool stopCalled)
		{
			Guid jobid = GetJobId();
			var version = Context.QueryString["orgid"];


			Connections.Remove(jobid, Context.ConnectionId);

			return base.OnDisconnected(stopCalled);
		}

		public override Task OnReconnected()
		{
			Guid jobId = GetJobId();

			if (!Connections.GetConnections(jobId).Contains(Context.ConnectionId))
			{
				Connections.Add(jobId, Context.ConnectionId);
			}

			return base.OnReconnected();
		}

		private Guid GetJobId()
		{
			//var version = Context.QueryString["orgid"];

			return new Guid(Context.QueryString["jobId"]);

		}
		private string GetQs(string qs)
		{
			var version = Context.QueryString["orgid"];

			return version;

		}
	
		public static IEnumerable<string> GetUserConnections(Guid jobId)
		{
			return Connections.GetConnections(jobId);
		}
	}
}