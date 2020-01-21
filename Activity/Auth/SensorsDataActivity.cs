using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using GeoGeometry.Container;

namespace GeoGeometry.Activity.Auth
{
    [Activity(Label = "SensorsDataActivity")]
    public class SensorsDataActivity: AppCompatActivity
    {
        private TableLayout tablelayout;

        private TextView s_weight_1;

        private TextView s_temperature_1;

        private TextView s_humidity_1;

        private TextView s_light_1;

        private TextView s_battery_1;

        private TextView s_signal_strength_2;

        private TextView s_situation_1;

        private TextView s_open_close_container_1;

        private TextView s_lock_unlock_door_1;

        private TextView s_longitude_1;

        private TextView s_latitude_1;

        private TextView s_date_time_1;

        private ImageView photobox;
        //public List<Box> mItems;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_sensors);

            //List<UserCartModel> mItems = await GetData();
            tablelayout = FindViewById<TableLayout>(Resource.Id.sensors_data);
            s_weight_1 = FindViewById<TextView>(Resource.Id.s_weight_1);
            s_temperature_1 = FindViewById<TextView>(Resource.Id.s_temperature_1);
            s_humidity_1 = FindViewById<TextView>(Resource.Id.s_humidity_1);
            s_light_1 = FindViewById<TextView>(Resource.Id.s_light_1);
            s_battery_1 = FindViewById<TextView>(Resource.Id.s_battery_1);
            s_signal_strength_2 = FindViewById<TextView>(Resource.Id.s_signal_strength_2);
            s_situation_1 = FindViewById<TextView>(Resource.Id.s_situation_1);
            s_open_close_container_1 = FindViewById<TextView>(Resource.Id.s_open_close_container_1);
            s_lock_unlock_door_1 = FindViewById<TextView>(Resource.Id.s_lock_unlock_door_1);
            s_longitude_1 = FindViewById<TextView>(Resource.Id.s_longitude_1);
            s_latitude_1 = FindViewById<TextView>(Resource.Id.s_latitude_1);
            s_date_time_1 = FindViewById<TextView>(Resource.Id.s_date_time_1);
            photobox = FindViewById<ImageView>(Resource.Id.photobox);

            s_weight_1.Text = StaticBox.Sensors["Вес груза"] + " кг";
            s_temperature_1.Text = StaticBox.Sensors["Температура"] + " °C";
            s_humidity_1.Text = StaticBox.Sensors["Влажность"] + " %";
            s_light_1.Text = StaticBox.Sensors["Освещенность"] + " лм";
            s_battery_1.Text = StaticBox.Sensors["Уровень заряда аккумулятора"] + " В";
            s_signal_strength_2.Text = StaticBox.Sensors["Уровень сигнала"];
            s_situation_1.Text = StaticBox.Sensors["Местоположение контейнера"];
            s_open_close_container_1.Text = (StaticBox.Sensors["Состояние контейнера"] == "0")?"сложен":"разложен";
            s_lock_unlock_door_1.Text = (StaticBox.Sensors["Состояние дверей"] == "0")?"закрыта":"открыта";
            s_longitude_1.Text = StaticBox.Longitude.ToString();
            s_latitude_1.Text = StaticBox.Latitude.ToString();
            s_date_time_1.Text = StaticBox.CreatedAtSensors.ToString();
            //MediaStore.Images.Media.InsertImage(ContentResolver, bitmap, "screen", "shot");
            
            photobox.SetImageBitmap(StaticBox.ImageData);
            //BindData();
        }


        //public async Task<List<UserCartModel>> GetData()
        //{
        //    HttpClient client = new HttpClient();
        //    string ac = Common.Url;
        //    string url = ac + "RestaurentOderList?id=" + sessionid;

        //    var request = HttpWebRequest.Create(url);
        //    request.Method = "GET";

        //    var response = await client.GetAsync(url);
        //    if (response.IsSuccessStatusCode)
        //    {
        //        var content = await response.Content.ReadAsStringAsync();
        //        mItems = JsonConvert.DeserializeObject<List<UserCartModel>>(content);
        //    }
        //    return mItems;
        //}

        //public void BindData()
        //{
        //    try
        //    {
        //        TableRow tableRow = new TableRow(this);
        //        TableRow.LayoutParams layoutParams = new TableRow.LayoutParams(
        //        ViewGroup.LayoutParams.MatchParent,
        //        ViewGroup.LayoutParams.MatchParent);

        //        foreach (var r in mItems)
        //        {

        //            productname = new TextView(this);
        //            productname.Text = r.ProductName;
        //            productname.LayoutParameters = layoutParams;

        //            price = new TextView(this);
        //            price.Text = Convert.ToString(r.Price);
        //            price.LayoutParameters = layoutParams;

        //            tableRow.AddView(price, 0);
        //            tableRow.AddView(price, 1);

        //            tablelayout.AddView(tableRow, 0);
        //        }
        //    }
        //    catch (Exception exx)
        //    {
        //        Console.WriteLine("Error During Bind Table Layout All Order Restaurent" + exx.Message);
        //    }
        //}
    }
}