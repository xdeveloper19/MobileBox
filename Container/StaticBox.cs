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
using GeoGeometry.Model;
using GeoGeometry.Model.Box;
using GeoGeometry.Model.User;
using static GeoGeometry.Model.Box.SmartBox;

namespace GeoGeometry.Container
{
    class StaticBox
    {
        /// <summary>
        /// Id контейнера.
        /// </summary>
        //static public string SmartBoxId;

        // static public Dictionary<string,string> key { get; set; }

        // /// <summary>
        // /// Разложен ли контейнер.
        // /// </summary>
        // static public string IsOpenedBox { get; set; }

        // /// <summary>
        // /// Открыта ли дверь.
        // /// </summary>
        // static public string IsOpenedDoor { get; set; }

        // /// <summary>
        // /// Освещённость.
        // /// </summary>
        // static public string Light { get; set; }

        // /// <summary>
        // /// Температура.
        // /// </summary>
        // static public string Temperature { get; set; }

        // /// <summary>
        // /// Влажность.
        // /// </summary>
        // static public string Wetness { get; set; }

        // /// <summary>
        // /// Заряд батареи.
        // /// </summary>
        // static public string BatteryPower { get; set; }

        // /// <summary>
        // /// Состояние контейнера.
        // /// </summary>
        // static public string BoxState { get; set; }

        // /// <summary>
        // /// Наименование контейнера.
        // /// </summary>
        // static public string Name { get; set; }

        // /// <summary>
        // /// Вес контейнера.
        // /// </summary>
        // static public string Weight { get; set; }

        // static public double Longitude { get; set; }

        // static public double Latitude { get; set; }

        // static public string Signal { get; set; }

        // static public DateTime Date { get; set; }
        public static Dictionary<string, string> Sensors { get; set; }
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
                {"Сосотояние контейнера","" },
                {"Местоположение контейнера","" }
            };
        }

        static public List<ContainerResponse> Objects { get; set; }

        /// <summary>
        /// Добавляю информацию о клиенте
        /// </summary>
        /// <param name="o_auth">Объект авторизации/регистрации</param>
        //public static void AddInfoAuth(ContainerResponse o_auth)
        //{
        //    SmartBoxId = o_auth.SmartBoxId;
        //    Name = o_auth.Name;
        //}

        public static void AddInfoObjects(ListResponse<ContainerResponse> boxes)
        {
            Objects = new List<ContainerResponse>();
            foreach (var box in boxes.Objects)
            {
                Objects.Add(box);
            }
        }

        //крч здесь заполнишь все значения сам
        //public static void AddInfoBox(BoxDataResponse boxData)
        //{
        //    //Name = boxData.Name;
        //    SmartBoxId = boxData.Id;
        //    IsOpenedDoor = boxData.IsOpenedBox;
        //    Weight = boxData.Weight;
        //    Light = boxData.Light;
        //    Temperature = boxData.Temperature;
        //    Wetness = boxData.Wetness;
        //    BatteryPower = boxData.BatteryPower;
        //    BoxState = boxData.BoxState;
        //}
    }
}




        /// <summary>
        /// Добавляю информацию о клиенте
        /// </summary>
        /// <param name="o_auth">Объект настройки</param>
        //public static void AddInfoUserSettings(UserSettingsResponseData o_user_settings)
        //{
        //    PhoneNumber = o_user_settings.PhoneNumber;
        //    Email = o_user_settings.Email;
        //    IsEmailConfirmed = o_user_settings.IsEmailConfirmed;
        //    IsLeader = o_user_settings.IsLeader;
        //}

        /// <summary>
        /// Добавляю информацию о клиенте
        /// </summary>
        /// <param name="o_auth">Объект клиент</param>
        //public static void AddInfoUser(UserResponseData o_user)
        //{
        //    MiddleName = o_user.MiddleName;
        //    Team = o_user.Team;
        //    TeamId = o_user.TeamId;
        //    Section = o_user.Section;
        //    Rang = o_user.Rang;
        //    Imagines = o_user.Imagines;
        //}

