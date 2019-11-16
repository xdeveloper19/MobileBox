﻿using System;
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
    public class BoxDataResponse: BaseResponseObject
    {
        public string Id { get; set; }
        public bool IsOpenedBox { get; set; }
        public bool IsOpenedDoor { get; set; }
        public double Weight { get; set; }
        public int Light { get; set; }
        public string Code { get; set; }
        public double Temperature { get; set; }
        public double Wetness { get; set; }
        public double BatteryPower { get; set; }
        //public ContainerState Situation { get; set; }
        //public enum ContainerState
        //{
        //    //сложенный, то есть контейнер закрыт

        //    onBase = 1, //на складе
        //    onCar = 2,//на автомобиле
        //    onShipper = 3, //выгружен у грузоотправителя
        //    onConsignee = 4//разгружен у грузополучателя
        //}
    }
}