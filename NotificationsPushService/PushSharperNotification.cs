using Newtonsoft.Json.Linq;
using NotificationsPushService.Interfaces;
using PushSharp.Apple;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationsPushService
{
    public class PushSharperNotification : IPushNotifications
    {
        public void SendNotification(string tag, string platform, string message, Dictionary<string, string> parameters)
        {
            if (platform == "ios")
                this.SendAppleNotification(tag, message, parameters);
        }

        public void SendAppleNotification(string deviceToken, string message, Dictionary<string, string> parameters)
        {
            var pathCertificate = @"<PATH_PROYECT>\Certificates\dummy-push-dev.p12";

            // Configuration (NOTE: .pfx can also be used here)
            var config = new ApnsConfiguration(ApnsConfiguration.ApnsServerEnvironment.Sandbox,
                pathCertificate, "12345678");

            // Create a new broker
            var apnsBroker = new ApnsServiceBroker(config);

            // Wire up events
            apnsBroker.OnNotificationFailed += (notification, aggregateEx) => {
                Debug.Write("Error to try send push");
            };

            apnsBroker.OnNotificationSucceeded += (notification) => {
                Debug.Write("Push sended ok");
            };

            // Start the broker
            apnsBroker.Start();

            // Queue a notification to send
            apnsBroker.QueueNotification(new ApnsNotification
            {
                DeviceToken = deviceToken,
                Payload = JObject.FromObject(new
                {
                    aps = new
                    {
                        alert = message,
                        sound = "default",
                        badge = 1,
                        parameters = parameters
                    }
                })
            });

            // Stop the broker, wait for it to finish   
            // This isn't done after every message, but after you're
            // done with the broker
            apnsBroker.Stop();
        }
    }
}
