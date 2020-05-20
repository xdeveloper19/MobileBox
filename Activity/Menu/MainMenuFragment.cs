using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Support.V7.Widget;
using Android.Widget;
using System.IO;
using GeoGeometry.Model.Auth;
using Newtonsoft.Json;
using GeoGeometry.Model.User;
using System.Net.Http;
using Android;
using Android.Content.PM;   // для Permission.Granted
using Android.Support.Design.Widget;  // для Snackbar
using Plugin.Media;

namespace GeoGeometry.Activity.Menu
{
    public class MainMenuFragment : Android.App.Fragment
    {
        /// <summary>
        /// Конпка "Страницы Отряд".
        /// </summary>
        private AppCompatImageButton btn_squad;

        /// <summary>
        /// Конпка "Страницы Главная".
        /// </summary>
        private AppCompatImageButton btn_map;

        /// <summary>
        /// Конпка "Страницы Камера".
        /// </summary>
        private AppCompatImageButton btn_camera;

        /// <summary>
        /// Конпка "Страницы Клиент".
        /// </summary>
        private AppCompatImageButton btn_user;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View main_menu = inflater.Inflate(Resource.Layout.fragment_menu_main, container, false);

            if (StaticMenu.id_page != 2)
            {
                StaticMenu.id_page = 2;
                // Переход к Отряды.
                btn_squad = main_menu.FindViewById<AppCompatImageButton>(Resource.Id.btn_squad);
                btn_squad.Click += (s, e) =>
                {
                    Intent squadActivity = new Intent(Activity, typeof(Auth.ContainerSelection));
                    StartActivity(squadActivity);
                };
            }

            if (StaticMenu.id_page != 1)
            {
                StaticMenu.id_page = 1;
                // Переход к Главная.
                btn_map = main_menu.FindViewById<AppCompatImageButton>(Resource.Id.btn_map);
                btn_map.Click += (s, e) =>
                {
                    Intent mapActivity = new Intent(Activity, typeof(Home.HomeActivity));
                    StartActivity(mapActivity);
                };
            }

            if (StaticMenu.id_page != 3)
            {
                StaticMenu.id_page = 3;
                // Переход к Камерe.
                btn_camera = main_menu.FindViewById<AppCompatImageButton>(Resource.Id.btn_camera);
                btn_camera.Click += (s, e) =>
                {
                    Intent cameraActivity = new Intent(Activity, typeof(Camera.CameraActivity));
                    StartActivity(cameraActivity);
                };

                if (StaticMenu.id_page != 4)
                {
                    StaticMenu.id_page = 4;
                    // Переход к Клиент.
                    btn_user = main_menu.FindViewById<AppCompatImageButton>(Resource.Id.btn_user);
                    btn_user.Click += (s, e) =>
                    {
                        Intent userActivity = new Intent(Activity, typeof(Auth.RegisterBoxActivity));
                        StartActivity(userActivity);
                    };

                }
            }
                return main_menu;
            }
    }
}
