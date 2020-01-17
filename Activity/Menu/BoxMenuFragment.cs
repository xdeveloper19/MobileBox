using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;

namespace GeoGeometry.Activity.Menu
{
    public class BoxMenuFragment: Fragment
    {
        /// <summary>
        /// Конпка "Страницы Главная".
        /// </summary>
        private AppCompatImageButton btn_box;

        /// <summary>
        /// Конпка "Страницы Карта".
        /// </summary>
        private AppCompatImageButton btn_map32;

        /// <summary>
        /// Конпка "Страницы Состояние".
        /// </summary>
        private AppCompatImageButton btn_box_state;

        /// <summary>
        /// Конпка "Страницы Фотофиксация".
        /// </summary>
        private AppCompatImageButton btn_photo;

        /// <summary>
        /// Конпка "Страницы Выход".
        /// </summary>
        private AppCompatImageButton btn_exit1;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View main_menu = inflater.Inflate(Resource.Layout.fragment_menu_box, container, false);
            string dir_path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);

            if (StaticMenu.id_page != 2)
            {
                StaticMenu.id_page = 2;
                // Переход к Отряды.
                btn_box = main_menu.FindViewById<AppCompatImageButton>(Resource.Id.btn_box);
                btn_box.Click += (s, e) =>
                {
                    Intent squadActivity = new Intent(Activity, typeof(Auth.DriverActivity));
                    StartActivity(squadActivity);
                };
            }

            if (StaticMenu.id_page != 1)
            {
                StaticMenu.id_page = 1;
                // Переход к Главная.
                btn_map32 = main_menu.FindViewById<AppCompatImageButton>(Resource.Id.btn_map32);
                btn_map32.Click += (s, e) =>
                {
                    Intent squadActivity = new Intent(Activity, typeof(Auth.ActivityGPSBox));
                    StartActivity(squadActivity);
                };
            }

            if (StaticMenu.id_page != 3)
            {
                StaticMenu.id_page = 3;
                // Переход к Камерe.
                btn_box_state = main_menu.FindViewById<AppCompatImageButton>(Resource.Id.btn_box_state);
                btn_box_state.Click += (s, e) =>
                {
                    Intent userActivity = new Intent(Activity, typeof(Auth.ActivityContainerCondition));
                    StartActivity(userActivity);
                }; 

                if (StaticMenu.id_page != 4)
                {
                    StaticMenu.id_page = 4;
                    // Переход к Клиент.
                    btn_photo = main_menu.FindViewById<AppCompatImageButton>(Resource.Id.btn_photo);
                    btn_photo.Click += (s, e) =>
                    {
                        Intent userActivity = new Intent(Activity, typeof(Auth.SensorsDataActivity));
                        StartActivity(userActivity);
                    };

                }

                if (StaticMenu.id_page != 5)
                {
                    StaticMenu.id_page = 5;
                    // Переход к Клиент.
                    btn_exit1 = main_menu.FindViewById<AppCompatImageButton>(Resource.Id.btn_exit_box);
                    btn_exit1.Click += (s, e) =>
                    {
                        //File.Delete(dir_path + "user_data.txt");
                        //ClearField();
                        Intent ActivityMain = new Intent(Activity, typeof(MainActivity));
                        StartActivity(ActivityMain);
                    };

                }
            }
            return main_menu;
        }
    }
}