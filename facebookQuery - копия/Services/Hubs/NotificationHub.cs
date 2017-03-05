using System;
using System.Threading;
using Microsoft.AspNet.SignalR;

namespace Services.Hubs
{
    public class NotificationHub : Hub
    {
        public void Add(dynamic acc, dynamic msg)
        {
            var date = DateTime.Now;
            var message = string.Format("{0} {1}", date, msg);

            var hubContext = GlobalHost.ConnectionManager.GetHubContext<NotificationHub>();
            hubContext.Clients.All.addNotice(acc, message);

            Thread.Sleep(2000);
        }
    }
}