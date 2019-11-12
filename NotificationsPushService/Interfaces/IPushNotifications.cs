using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationsPushService.Interfaces
{
    public interface IPushNotifications
    {
        void SendNotification(string tag, string platform, string message, Dictionary<string, string> parameters);
    }
}
