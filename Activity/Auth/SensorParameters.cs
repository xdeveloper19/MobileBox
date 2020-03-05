using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GeoGeometry.Model.Auth;
using Android.App;
using Android.Content;
using Android.OS;
using System.Text.Json;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using System.Net.Http;
using Newtonsoft.Json;
using System.Net;
using GeoGeometry.Container;
using System.IO;
using GeoGeometry.Model.User;
using GeoGeometry.Model.Box;
using GeoGeometry.Model;
using Android.Telephony;
using Android.Gms.Location;
using GeoGeometry.Model.GPSLocation;
using Android.Gms.Maps;
using static GeoGeometry.Model.Box.SmartBox;
using Android.Gms.Maps.Model;
using System.Globalization;
using GeoGeometry.Activity.Menu;
using Plugin.Settings;
using Android.Support.V4.App;

namespace GeoGeometry.Activity.Auth
{

    [Activity(Label = "SensorParameters")]


    public class SensorParameters : AppCompatActivity
    {       

        private TextView TextNameBox;

        private TextView SmullWeight;

        private TextView SmullTemperature;

        private TextView SmullLight;

        private TextView SmullHumidity;

        private TextView SmullBattery;

        private TextView SmullNetworkSignal;

        private SeekBar s_weight;

        private SeekBar s_temperature;

        private SeekBar s_light;

        private SeekBar s_humidity;

        private SeekBar s_battery;

        private SeekBar s_signal_strength_1;

        private Button btn_save_parameters;

        //GoogleMap _googleMap;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.activity_box);
            StaticMenu.id_page = 1;

            btn_save_parameters = FindViewById<Button>(Resource.Id.btn_save_parameters);
            SmullWeight = FindViewById<TextView>(Resource.Id.SmullWeight);
            SmullTemperature = FindViewById<TextView>(Resource.Id.SmullTemperature);
            SmullLight = FindViewById<TextView>(Resource.Id.SmullLight);
            SmullHumidity = FindViewById<TextView>(Resource.Id.SmullHumidity);
            TextNameBox = FindViewById<TextView>(Resource.Id.TextNameBox);
            SmullBattery = FindViewById<TextView>(Resource.Id.SmullBattery);
            SmullNetworkSignal = FindViewById<TextView>(Resource.Id.SmullNetworkSignal);
            s_weight = FindViewById<SeekBar>(Resource.Id.s_weight);
            s_temperature = FindViewById<SeekBar>(Resource.Id.TemperatureEdit);
            s_light = FindViewById<SeekBar>(Resource.Id.s_light);
            s_humidity = FindViewById<SeekBar>(Resource.Id.s_humidity);
            s_battery = FindViewById<SeekBar>(Resource.Id.s_battery);
            s_signal_strength_1 = FindViewById<SeekBar>(Resource.Id.s_signal_strength_1);

            GetInfoAboutBox();

            
            s_weight.ProgressChanged += (object sender, SeekBar.ProgressChangedEventArgs e) =>
            {
                if (StaticBox.Sensors["Состояние контейнера"] == "0")
                {
                    s_weight.Progress = 0;
                }
                else
                {
                    if (e.FromUser)
                    {
                        SmullWeight.Text = string.Format("{0}", e.Progress);
                    }
                }
            };
            
            
            s_temperature.ProgressChanged += (object sender, SeekBar.ProgressChangedEventArgs e) =>
            {
                if (e.FromUser)
                {                  
                    SmullTemperature.Text = (e.Progress - 40).ToString();
                }
            };
            s_light.ProgressChanged += (object sender, SeekBar.ProgressChangedEventArgs e) =>
            {
                if (e.FromUser)
                {
                    SmullLight.Text = string.Format("{0}", e.Progress);
                }
            };
            s_humidity.ProgressChanged += (object sender, SeekBar.ProgressChangedEventArgs e) =>
            {
                if (e.FromUser)
                {
                    SmullHumidity.Text = string.Format("{0}", e.Progress);
                }
            };
            s_battery.ProgressChanged += (object sender, SeekBar.ProgressChangedEventArgs e) =>
            {
                if (e.FromUser)
                {
                    SmullBattery.Text = string.Format("{0}", e.Progress);
                }
            };
            
            s_signal_strength_1.ProgressChanged += (object sender, SeekBar.ProgressChangedEventArgs e) =>
            {
                if (e.FromUser)
                {
                    SmullNetworkSignal.Text = (e.Progress - 110).ToString();
                }
            };

           
           
            //редактирование данных контейнера
            btn_save_parameters.Click += async delegate
            {
                try
                {

                    StaticBox.Sensors["Вес груза"] = s_weight.Progress.ToString();
                    StaticBox.Sensors["Температура"] = SmullTemperature.Text;
                    StaticBox.Sensors["Влажность"] = s_humidity.Progress.ToString();
                    StaticBox.Sensors["Освещенность"] = s_light.Progress.ToString();
                    StaticBox.Sensors["Уровень сигнала"] = SmullNetworkSignal.Text;
                    StaticBox.Sensors["Уровень заряда аккумулятора"] = s_battery.Progress.ToString();
                    var o_data = await ContainerService.EditBox();

                    if (o_data.Status == "1")
                    {
                        Toast.MakeText(this, o_data.Message, ToastLength.Long).Show();
                        GetInfoAboutBox();
                    }
                    else
                    {
                        StaticBox.CameraOpenOrNo = 1;
                        Intent authActivity = new Intent(this, typeof(Auth.SensorsDataActivity));
                        StartActivity(authActivity);
                    }
                }
                catch (Exception ex)
                {
                    Toast.MakeText(this, "" + ex.Message, ToastLength.Long).Show();
                }
            };

        }   
        /// <summary>
        /// сбор информации о контейнере
        /// </summary>
        /// <param name="dir_path"></param>
        private async void GetInfoAboutBox()
        {
            var myHttpClient = new HttpClient();
            var id1 = CrossSettings.Current.GetValueOrDefault("id", "");
            var uri = new Uri("http://iot.tmc-centert.ru/api/container/getbox?id=" + id1);
             HttpResponseMessage response = await myHttpClient.GetAsync(uri.ToString());

            string s_result;
            using (HttpContent responseContent = response.Content)
            {
                s_result = await responseContent.ReadAsStringAsync();
            }
            AuthApiData<ListResponse<BoxDataResponse>> o_data = new AuthApiData<ListResponse<BoxDataResponse>>();

            o_data.ResponseData = new ListResponse<BoxDataResponse>();
            o_data = JsonConvert.DeserializeObject<AuthApiData<ListResponse<BoxDataResponse>>>(s_result);
            if (o_data.Status == "0")
            {
                ListResponse<BoxDataResponse> o_boxes_data = new ListResponse<BoxDataResponse>();
                o_boxes_data.Objects = o_data.ResponseData.Objects;// !!!


                //В статик бокс закомментируй 9 свойств
                StaticBox.Sensors["Температура"] = o_data.ResponseData.Objects.Where(f => f.SensorName == "Температура").Select(s => s.Value).FirstOrDefault();
                StaticBox.Sensors["Влажность"] = o_data.ResponseData.Objects.Where(f => f.SensorName == "Влажность").Select(s => s.Value).FirstOrDefault();
                StaticBox.Sensors["Освещенность"] = o_data.ResponseData.Objects.Where(f => f.SensorName == "Освещенность").Select(s => s.Value).FirstOrDefault();                
                StaticBox.Sensors["Уровень заряда аккумулятора"] = o_data.ResponseData.Objects.Where(f => f.SensorName == "Уровень заряда аккумулятора").Select(s => s.Value).FirstOrDefault();
                StaticBox.Sensors["Уровень сигнала"] = o_data.ResponseData.Objects.Where(f => f.SensorName == "Уровень сигнала").Select(s => s.Value).FirstOrDefault();
                StaticBox.Sensors["Состояние дверей"] = o_data.ResponseData.Objects.Where(f => f.SensorName == "Состояние дверей").Select(s => s.Value).FirstOrDefault();
                StaticBox.Sensors["Состояние контейнера"] = o_data.ResponseData.Objects.Where(f => f.SensorName == "Состояние контейнера").Select(s => s.Value).FirstOrDefault();
                StaticBox.Sensors["Местоположение контейнера"] = o_data.ResponseData.Objects.Where(f => f.SensorName == "Местоположение контейнера").Select(s => s.Value).FirstOrDefault();

                if(StaticBox.Sensors["Состояние контейнера"] == "0")
                    StaticBox.Sensors["Вес груза"] = "0";                    
                else
                    StaticBox.Sensors["Вес груза"] = o_data.ResponseData.Objects.Where(f => f.SensorName == "Вес груза").Select(s => s.Value).FirstOrDefault();

                StaticBox.CreatedAtSensors = (DateTime)o_data.ResponseData.Objects[0].CreatedAt;
            }
            //Заполняй остальные параметры как в этом примере
            int a = 0, b = 0;
            s_weight.Progress = Convert.ToInt32(StaticBox.Sensors["Вес груза"]);
            a = Convert.ToInt32(StaticBox.Sensors["Температура"]);
            b = Convert.ToInt32(StaticBox.Sensors["Уровень сигнала"]);
            s_light.Progress = Convert.ToInt32(StaticBox.Sensors["Освещенность"]);
            s_humidity.Progress = Convert.ToInt32(StaticBox.Sensors["Влажность"]);
            s_battery.Progress = Convert.ToInt32(StaticBox.Sensors["Уровень заряда аккумулятора"]);

            s_temperature.Progress = a + 40;
            s_signal_strength_1.Progress = b + 110;



            SmullWeight.Text = StaticBox.Sensors["Вес груза"];
            SmullTemperature.Text = StaticBox.Sensors["Температура"];
            SmullLight.Text = StaticBox.Sensors["Освещенность"];
            SmullHumidity.Text = StaticBox.Sensors["Влажность"];
            SmullBattery.Text = StaticBox.Sensors["Уровень заряда аккумулятора"];
            SmullNetworkSignal.Text = StaticBox.Sensors["Уровень сигнала"];
            TextNameBox.Text = "(" + CrossSettings.Current.GetValueOrDefault("namebox", "") + ")";

            if (StaticBox.Sensors["Состояние контейнера"] == "0")
            {
                StaticBox.Sensors["Вес груза"] = "0";
                Android.App.AlertDialog.Builder alert = new Android.App.AlertDialog.Builder(this);
                alert.SetTitle("Внимание !");
                alert.SetMessage("Невозможно изменить вес контейнера.(Состояние контейнера: сложен) ");
                alert.SetPositiveButton("Закрыть", (senderAlert, args) => {
                    Toast.MakeText(this, "Предупреждение было закрыто!", ToastLength.Short).Show();
                });
                Dialog dialog = alert.Create();
                dialog.Show();
            }
        }
    }
}
            


