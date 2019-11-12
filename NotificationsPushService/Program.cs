using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationsPushService
{
    class Program
    {
        static void Main(string[] args)
        {
            string deviceToken = "D72195D320BECAC66EE4536CD763DCD6AC70F03F23CB8741096280EFE1FA0EF4";

            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("typePush", "newOrder");
            parameters.Add("orderID", "954788");

            PushService.Instance.SendNotification(
                tag: deviceToken,
                platform: "ios",
                message: "test for remote notifications",
                parameters: parameters);
        }
    }
}
