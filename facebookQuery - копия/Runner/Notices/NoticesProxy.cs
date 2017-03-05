using Microsoft.AspNet.SignalR.Client;
using Services.Interfaces.Notices;

namespace Runner.Notices
{
    public class NoticesProxy : INoticesProxy
    {
        public void AddNotice(dynamic accountId, dynamic message)
        {
            var hubConnection = new HubConnection("http://localhost:63711/");
            var hubProxy = hubConnection.CreateHubProxy("notificationHub");
            hubConnection.Start().Wait();

            hubProxy.Invoke("Add", accountId, message);
        }
    }
}
