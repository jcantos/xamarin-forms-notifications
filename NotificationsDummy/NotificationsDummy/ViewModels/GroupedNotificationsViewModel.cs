using System;
using System.Windows.Input;
using NotificationsDummy.Interfaces;
using NotificationsDummy.Models;
using Xamarin.Forms;

namespace NotificationsDummy.ViewModels
{
    public class GroupedNotificationsViewModel : BaseViewModel
    {
        public Item Item { get; set; }
        public ICommand AddNotificationCommand { private set; get; }

        public GroupedNotificationsViewModel(Item item = null)
        {
            Title = item?.Text;
            Item = item;

            AddNotificationCommand = new Command(
                execute: () =>
                {
                    DependencyService.Get<INotificationService>().AddNotification();
                });

        }

    }
}
