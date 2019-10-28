using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Media;
using Android.OS;
using Android.Runtime;
using Android.Service.Notification;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;
using Java.Util.Concurrent.Atomic;
using NotificationsDummy.Droid.Helpers;
using NotificationsDummy.Interfaces;
using Xamarin.Forms;

[assembly: Dependency(typeof(NotificationService))]
namespace NotificationsDummy.Droid.Helpers
{
    public class NotificationService : INotificationService
    {
        private static AtomicInteger c = new AtomicInteger();
        private const string GROUP_NAME = "my_group_name";

        public void AddNotification()
        {
            this.createNotification();
        }

        private void createNotification()
        {
            var notificationManager = MainActivity.Instance.GetSystemService(Context.NotificationService) as NotificationManager;
            int id = getNextNotificationID();

            //Use Notification Builder
            NotificationCompat.Builder builder = new NotificationCompat.Builder(MainActivity.Instance, MainActivity.CHANNEL_ID);
            NotificationCompat.Builder builderMuted = new NotificationCompat.Builder(MainActivity.Instance, MainActivity.CHANNEL_ID_MUTED);

            // Set up an intent so that tapping the notifications returns to this app
            Intent activityIntent = new Intent(MainActivity.Instance, typeof(MainActivity));
            activityIntent.AddFlags(ActivityFlags.SingleTop);
            PendingIntent contentIntent = PendingIntent.GetActivity(MainActivity.Instance,
                id,
                activityIntent,
                PendingIntentFlags.UpdateCurrent);

            string title = "Javier Cantos";
            string text = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Quisque metus nisl, feugiat non vulputate et, dictum et odio. Morbi malesuada ultrices semper";

            //create the notification
            var notification = builder
                        .SetContentIntent(contentIntent)
                        .SetSmallIcon(Android.Resource.Drawable.SymActionChat)
                        .SetTicker(title)
                        .SetContentTitle(title)
                        .SetContentText(text)
                        .SetSound(RingtoneManager.GetDefaultUri(RingtoneType.Notification))
                        .SetPriority(NotificationCompat.PriorityHigh)
                        .SetGroup(GROUP_NAME)
                        .SetAutoCancel(true)
                        .Build();

            //show the notification
            notificationManager.Notify(id, notification);

            int numberNotifications = getNumberNotifications(GROUP_NAME);
            if (numberNotifications > 1)
            {
                text = GROUP_NAME + " (" + numberNotifications + ")";

                //create the summary notification
                var notificationSummary = builderMuted
                        .SetContentIntent(contentIntent)
                        .SetSmallIcon(Android.Resource.Drawable.SymActionChat)
                        .SetStyle(new NotificationCompat.BigTextStyle().SetSummaryText(text))
                        .SetSound(null)
                        .SetPriority(NotificationCompat.PriorityLow)
                        .SetGroupSummary(true)
                        .SetGroup(GROUP_NAME)
                        .SetShortcutId("summary_" + GROUP_NAME)
                        .SetAutoCancel(true)
                        .Build();

                //show the notification
                int idSumm = getIdSummaryNotification(GROUP_NAME);
                notificationManager.Notify(idSumm, notificationSummary);
            }

        }

        private int getNextNotificationID()
        {
            return c.IncrementAndGet();
        }

        private int getNumberNotifications(string groupName)
        {
            int reply = 0;

            var notificationManager = MainActivity.Instance.GetSystemService(Context.NotificationService) as NotificationManager;
            StatusBarNotification[] activeNotifications = notificationManager.GetActiveNotifications();

            foreach (StatusBarNotification notification in activeNotifications)
            {
                if (notification.IsGroup && notification.GroupKey.Contains(groupName) && notification.Notification.ShortcutId != "summary_" + GROUP_NAME)
                    reply++;
            }

            return reply;
        }

        private int getIdSummaryNotification(string groupName)
        {
            int reply = 0;

            var notificationManager = MainActivity.Instance.GetSystemService(Context.NotificationService) as NotificationManager;
            StatusBarNotification[] activeNotifications = notificationManager.GetActiveNotifications();

            foreach (StatusBarNotification notification in activeNotifications)
            {
                if (notification.Notification.ShortcutId == "summary_" + GROUP_NAME)
                {
                    reply = notification.Id;
                    break;
                }
            }

            if (reply == 0)
                reply = getNextNotificationID();

            return reply;
        }
    }
}