using System;
using System.Collections.Generic;
using System.Text;

namespace NotificationsDummy.Models
{
    public class Notification
    {
        public string Message { get; set; }
        public Newtonsoft.Json.Linq.JObject Parameters { get; set; }
    }
}
