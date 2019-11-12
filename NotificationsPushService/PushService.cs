using NotificationsPushService.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationsPushService
{
    public class PushService
    {
        private static PushService instance;

        public static PushService Instance
        {
            get
            {
                return instance ?? (instance = new PushService());
            }
        }

        protected IPushNotifications pushNotification;

        public PushService()
        {
            IPushNotifications azureHubNotification = new PushSharperNotification();
            this.pushNotification = azureHubNotification;
        }

        public void SendNotification(string tag, string platform, string message, Dictionary<string, string> parameters)
        {
            pushNotification.SendNotification(tag, platform, message, parameters);
        }
    }
}
