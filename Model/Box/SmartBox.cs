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

namespace GeoGeometry.Model.Box
{
    public class SmartBox
    {
        public enum ContainerState
        {
            //сложенный, то есть контейнер закрыт

            onBase = 1, //на складе
            onCar,//на автомобиле
            onShipper, //выгружен у грузоотправителя
            onConsignee//разгружен у грузополучателя
        }

        public string Id { get; set; }


        //public string Weight { get; set; }


        //public string Light { get; set; }


        //public string Temperature { get; set; }


        //public string Wetness { get; set; }
        //public string BatteryPower { get; set; }
        //public string SignalLevel { get; set; }
        public DateTime Date { get; set; }

        public Dictionary<string,string> Sensors { get; set; }
    }
}