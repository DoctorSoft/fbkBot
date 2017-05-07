using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.AspNet.SignalR;
using Services.Models.SignalRModels;

namespace Services.Hubs
{
    public class NotificationHub : Hub
    {
        static List<UserModel> Users = new List<UserModel>();

        public void Add(dynamic acc, dynamic msg)
        {
            try
            {
                var date = DateTime.Now;
                var message = string.Format("{0} {1}", date, msg);

                var hubContext = GlobalHost.ConnectionManager.GetHubContext<NotificationHub>();
                hubContext.Clients.All.addNotice(acc, message);

                Thread.Sleep(2000);

            }
            catch (Exception ex)
            {
                LogWriter.LogWriter.AddToLog(ex.Message);
            }
        }

        // Подключение нового пользователя
        public void Connect(string userName)
        {
            var id = Context.ConnectionId;


            if (!Users.Any(x => x.ConnectionId == id))
            {
                Users.Add(new UserModel { ConnectionId = id, Name = userName });

                // Посылаем сообщение текущему пользователю
                Clients.Caller.onConnected(id, userName, Users);

                // Посылаем сообщение всем пользователям, кроме текущего
                //Clients.AllExcept(id).onNewUserConnected(id, userName);
            }
        }

        // Отключение пользователя
        public override System.Threading.Tasks.Task OnDisconnected(bool stopCalled)
        {
            var item = Users.FirstOrDefault(x => x.ConnectionId == Context.ConnectionId);
            if (item != null)
            {
                Users.Remove(item);
                var id = Context.ConnectionId;
                Clients.All.onUserDisconnected(id, item.Name);
            }

            return base.OnDisconnected(stopCalled);
        }
    }
}