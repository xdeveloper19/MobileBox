using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;

namespace GeoGeometry.Activity.Auth
{
    [Activity(Label = "ActivityContainerCondition")]
    class ActivityContainerCondition : AppCompatActivity
    {
        private RelativeLayout condition;

        private Button btn_open_close_container;

        private Button btn_lock_unlock_door;

        private EditText s_open_close_container;

        private EditText s_lock_unlock_door;

        private ImageView box_lay_fold;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.activity_container_condition);


            condition = FindViewById<RelativeLayout>(Resource.Id.condition);
            s_open_close_container = FindViewById<EditText>(Resource.Id.s_open_close_container);
            s_lock_unlock_door = FindViewById<EditText>(Resource.Id.s_lock_unlock_door);
            btn_open_close_container = FindViewById<Button>(Resource.Id.btn_open_close_container);
            btn_lock_unlock_door = FindViewById<Button>(Resource.Id.btn_lock_unlock_door);
            box_lay_fold = FindViewById<ImageView>(Resource.Id.box_lay_fold);

            s_open_close_container.Focusable = false;
            s_open_close_container.LongClickable = false;
            s_lock_unlock_door.Focusable = false;
            s_lock_unlock_door.LongClickable = false;


            //изменение состояния контейнера
            btn_open_close_container.Click += async delegate
            {
                try
                {
                    if (s_open_close_container.Text == "сложен")
                    {
                        s_open_close_container.Text = "разложен";
                        box_lay_fold.SetImageResource(Resource.Drawable.close_door);
                    }
                        
                    else
                    {
                        s_open_close_container.Text = "сложен";
                        s_lock_unlock_door.Text = "";
                        box_lay_fold.SetImageResource(Resource.Drawable.close_box);
                    }                      
                }
                catch (Exception ex)
                {
                    Toast.MakeText(this, "" + ex.Message, ToastLength.Long).Show();
                }
            };

            //изменение состояния дверей
            btn_lock_unlock_door.Click += async delegate
            {
                try
                {
                    if (s_lock_unlock_door.Text == "закрыта" && s_open_close_container.Text != "сложен")
                    {
                        s_lock_unlock_door.Text = "открыта";
                        box_lay_fold.SetImageResource(Resource.Drawable.open_door);
                    }
                        
                    else if (s_open_close_container.Text != "сложен")
                    {
                        s_lock_unlock_door.Text = "закрыта";
                        box_lay_fold.SetImageResource(Resource.Drawable.close_door);
                    }
                    else if(s_open_close_container.Text == null && s_lock_unlock_door.Text == null)
                    {
                        Toast.MakeText(this, "" + "Невозможно изменить состояния дверей.", ToastLength.Long).Show();
                    }
                    else
                        Toast.MakeText(this, "" + "Невозможно изменить состояния дверей.", ToastLength.Long).Show();
                }
                catch (Exception ex)
                {
                    Toast.MakeText(this, "" + ex.Message, ToastLength.Long).Show();
                }
            };
        }
    }
}