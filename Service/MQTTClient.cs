using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace GeoGeometry.Service
{
    public class MQTTClient
    {
        public string Server { get; set; }
        public string ClientId { get; set; }
        public string UserName { get; set; }
        public int Port { get; set; }
        public string Password { get; set; }
    }
}