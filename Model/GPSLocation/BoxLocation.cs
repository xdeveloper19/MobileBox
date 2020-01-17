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

namespace GeoGeometry.Model.GPSLocation
{
    class BoxLocation
    {
        /// <summary>
        /// ID клиента.
        /// </summary>
        public string id { get; set; }

        /// <summary>
        /// Долгота
        /// </summary>
        public double lon1 { get; set; }

        /// <summary>
        /// Широта
        /// </summary>
        public double lat1 { get; set; }

        public DateTime date { get; set; }
    }
}