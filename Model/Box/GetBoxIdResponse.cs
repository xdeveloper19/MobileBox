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
using GeoGeometry.Model.Auth;

namespace GeoGeometry.Model.Box
{
    class GetBoxIdResponse : BaseResponseObject
    {
        public string BoxId { get; set; }

        public string Name { get; set; }
    }
}