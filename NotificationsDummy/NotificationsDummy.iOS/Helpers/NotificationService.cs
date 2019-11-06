using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Foundation;
using NotificationsDummy.Interfaces;
using NotificationsDummy.iOS.Helpers;
using UIKit;
using UserNotifications;
using Xamarin.Forms;

[assembly: Dependency(typeof(NotificationService))]
namespace NotificationsDummy.iOS.Helpers
{
    public class NotificationService : INotificationService
    {
        private const string GROUP_NAME = "my_group_name";

        public void AddNotification()
        {
            createNotification();
        }

        private async void createNotification()
        {
            try
            {
                var center = UNUserNotificationCenter.Current;

                UNNotificationSettings settings = await center.GetNotificationSettingsAsync();
                if (settings.AuthorizationStatus != UNAuthorizationStatus.Authorized)
                {
                    return;
                }

                var content = new UNMutableNotificationContent()
                {
                    ThreadIdentifier = GROUP_NAME,
                    Title = "Javier Cantos",
                    Body = "Mensaje de prueba",
                    SummaryArgument = "Javier Cantos"
                };

                var request = UNNotificationRequest.FromIdentifier(
                    Guid.NewGuid().ToString(),
                    content,
                    null
                );

                center.AddNotificationRequest(request, null);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
    }
}