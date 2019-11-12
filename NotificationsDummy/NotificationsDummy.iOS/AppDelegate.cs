using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using NotificationsDummy.Models;
using UIKit;
using UserNotifications;

namespace NotificationsDummy.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        private App crossApp;

        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            settingsForLocalNotifications();
            settingsForRemoteNotifications();

            global::Xamarin.Forms.Forms.Init();
            crossApp = new App();
            LoadApplication(crossApp);

            return base.FinishedLaunching(app, options);
        }

        public override void RegisteredForRemoteNotifications(UIApplication application, NSData deviceToken)
        {
            //byte[] dt = deviceToken.ToArray();
            //string newDeviceToken = Convert.ToBase64String(dt);

            // for push sharper format
            byte[] dt = deviceToken.ToArray();
            string dts = BitConverter.ToString(dt).Replace("-", "").ToUpperInvariant();

            //TODO
            //register deviceToken on backend
        }

        public override void OnActivated(UIApplication uiApplication)
        {
            UIApplication.SharedApplication.ApplicationIconBadgeNumber = 0;
            base.OnActivated(uiApplication);
        }

        public override void ReceivedRemoteNotification(UIApplication application, NSDictionary userInfo)
        {
            this.receivedPushNotification(userInfo, application);
        }

        public override void DidReceiveRemoteNotification(UIApplication application, NSDictionary userInfo, Action<UIBackgroundFetchResult> completionHandler)
        {
            this.receivedPushNotification(userInfo, application);
        }

        private void receivedPushNotification(NSDictionary userInfo, UIApplication application)
        {
            if (application.ApplicationState == UIApplicationState.Active)
            {
                return;
            }

            var aps = userInfo.ObjectForKey(new NSString("aps")) as NSDictionary;
            if (aps != null)
            {
                string message = aps.ObjectForKey(new NSString("alert")) as NSString;
                var parameters = aps.ObjectForKey(new NSString("parameters")) as NSDictionary;

                NSError error;
                var json = NSJsonSerialization.Serialize(parameters, NSJsonWritingOptions.PrettyPrinted, out error);
                Newtonsoft.Json.Linq.JObject jObject = Newtonsoft.Json.Linq.JObject.Parse(json.ToString(NSStringEncoding.UTF8));

                Notification _notification = new Notification()
                {
                    Message = message,
                    Parameters = jObject
                };

                crossApp.OnPushNotification(_notification);
            }
        }

        private void settingsForLocalNotifications()
        {
            UNUserNotificationCenter center = UNUserNotificationCenter.Current;
            center.RequestAuthorization(UNAuthorizationOptions.Alert, (bool success, NSError error) => {
                // Set the Delegate regardless of success; users can modify their notification
                // preferences at any time in the Settings app.
                center.Delegate = new NotificationCenterDelegate();
            });
        }
        private void settingsForRemoteNotifications()
        {
            if (UIDevice.CurrentDevice.CheckSystemVersion(8, 0))
            {
                var pushSettings = UIUserNotificationSettings.GetSettingsForTypes(
                       UIUserNotificationType.Alert | UIUserNotificationType.Badge | UIUserNotificationType.Sound,
                       new NSSet());

                UIApplication.SharedApplication.RegisterUserNotificationSettings(pushSettings);
                UIApplication.SharedApplication.RegisterForRemoteNotifications();
            }
            else
            {
                UIRemoteNotificationType notificationTypes = UIRemoteNotificationType.Alert | UIRemoteNotificationType.Badge | UIRemoteNotificationType.Sound;
                UIApplication.SharedApplication.RegisterForRemoteNotificationTypes(notificationTypes);
            }
        }
    }

    public class NotificationCenterDelegate : UNUserNotificationCenterDelegate
    {
        [Export("userNotificationCenter:willPresentotification:withCompletionHandler:")]
        public override void WillPresentNotification(UNUserNotificationCenter center, UNNotification notification, System.Action<UNNotificationPresentationOptions> completionHandler)
        {
            completionHandler(UNNotificationPresentationOptions.Alert);
        }
    }
}
