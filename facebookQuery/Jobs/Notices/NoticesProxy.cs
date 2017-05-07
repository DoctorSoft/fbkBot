using System;
using System.IO;
using Microsoft.AspNet.SignalR.Client;
using Services.Interfaces.Notices;

namespace Jobs.Notices
{
    public class NoticesProxy : INoticesProxy
    {
        public void AddNotice(dynamic accountId, dynamic message)
        {
            var hosting = new HubConnection("http://face.2h.by/");
            var local = new HubConnection("http://localhost:63711");
            var local2 = new HubConnection("http://test.test");

            var hubConnection = hosting;
 
            var hubProxy = hubConnection.CreateHubProxy("notificationHub");
            
            try
            {
                hubConnection.Start().Wait();
                hubProxy.Invoke("Add", accountId, message).Wait();
            }
            catch (Exception ex)
            {
                //LogWriter.LogWriter.AddToLog(ex.Message);
            }
        }
    }
}
