using System;
using System.Collections.Generic;
using Android.Graphics;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using GeoGeometry.Model;
using GeoGeometry.Model.Box;
using GeoGeometry.Model.User;
using static GeoGeometry.Model.Box.SmartBox;

namespace GeoGeometry.Container
{
    class StaticBox
    {
        public static Bitmap ImageData { get; set; } 
        public static Dictionary<string, string> Sensors { get; set; }
        public static string IsAvalableRequest { get; set; }
        public static double Longitude { get; set; }
        public static string DeviceId { get; set; }
        public static double Latitude { get; set; }
        public static DateTime CreatedAtSensors { get; set; }

        public static int CameraOpenOrNo { get; set; }
        
        static StaticBox()
        {
            Sensors = new Dictionary<string, string>()
            {

                {"Температура","" },
                {"Влажность","" },
                {"Освещенность","" },
                {"Вес груза","" },
                {"Уровень заряда аккумулятора","" },
                {"Уровень сигнала","" },
                {"Состояние дверей","" },
                {"Состояние контейнера","" },
                {"Местоположение контейнера","" }
            };
        }

        static public List<ContainerResponse> Objects { get; set; }

  
    }
}




