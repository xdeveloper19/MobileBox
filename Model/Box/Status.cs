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
    public class Status: BaseResponseObject
    {
        public BoxResponse status { get; set; }
        public Status()
        {
            this.status = new BoxResponse();
        }
    }
}