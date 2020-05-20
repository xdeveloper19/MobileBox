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
                        Toast.MakeText(this, o_data.Message, ToastLength.Long).Show();
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
            try
            {
                CreateBoxModel model = new CreateBoxModel
                {
                    id = StaticBox.DeviceId
                };

                var myHttpClient = new HttpClient();
                var id1 = CrossSettings.Current.GetValueOrDefault("id", "");
                var uri = new Uri("http://iot.tmc-centert.ru/api/container/getbox?id=" + id1);
                var uri2 = new Uri("http://smartboxcity.ru:8003/imitator/status?id=" + StaticBox.DeviceId);
                HttpResponseMessage response = await myHttpClient.GetAsync(uri2.ToString());
                //var myHttpClient = new HttpClient();
                //var id1 = CrossSettings.Current.GetValueOrDefault("id", "");
                //var uri = new Uri("http://iot.tmc-centert.ru/api/container/getbox?id=" + id1);
                // HttpResponseMessage response = await myHttpClient.GetAsync(uri.ToString());

                string s_result;
                using (HttpContent responseContent = response.Content)
                {
                    s_result = await responseContent.ReadAsStringAsync();
                }

                if (response.IsSuccessStatusCode)
                {
                    Status o_data = new Status();
                    
                    o_data = JsonConvert.DeserializeObject<Status>(s_result);

                    //В статик бокс закомментируй 9 свойств
                    StaticBox.Sensors["Температура"] = o_data.status.Sensors["Температура"].Replace(".",",");
                    StaticBox.Sensors["Влажность"] = o_data.status.Sensors["Влажность"].Replace(".", ",");
                    StaticBox.Sensors["Освещенность"] = o_data.status.Sensors["Освещенность"].Replace(".", ",");
                    StaticBox.Sensors["Уровень заряда аккумулятора"] = o_data.status.Sensors["Уровень заряда аккумулятора"].Replace(".", ",");
                    StaticBox.Sensors["Уровень сигнала"] = o_data.status.Sensors["Уровень сигнала"].Replace(".", ",");
                    StaticBox.Sensors["Состояние дверей"] = o_data.status.Sensors["Состояние дверей"].Replace(".", ",");
                    StaticBox.Sensors["Состояние контейнера"] = o_data.status.Sensors["Состояние контейнера"].Replace(".", ",");
                    StaticBox.Sensors["Местоположение контейнера"] = o_data.status.Sensors["Местоположение контейнера"].Replace(".", ",");

                    if (StaticBox.Sensors["Состояние контейнера"] == "0")
                        StaticBox.Sensors["Вес груза"] = "0";
                    else
                        StaticBox.Sensors["Вес груза"] = o_data.status.Sensors["Вес груза"];

                    //StaticBox.CreatedAtSensors = (DateTime)o_data.ResponseData.Objects[0].CreatedAt;
                    //Заполняй остальные параметры как в этом примере
                    int a = 0, b = 0;

                    decimal weight, temp, signal, light, humi, akk;

                    weight = Convert.ToDecimal(StaticBox.Sensors["Вес груза"]);
                    temp = Convert.ToDecimal(StaticBox.Sensors["Температура"]);
                    signal = Convert.ToDecimal(StaticBox.Sensors["Уровень сигнала"]);
                    light = Convert.ToDecimal(StaticBox.Sensors["Освещенность"]);
                    humi = Convert.ToDecimal(StaticBox.Sensors["Влажность"]);
                    akk = Convert.ToDecimal(StaticBox.Sensors["Уровень заряда аккумулятора"]);

                    s_weight.Progress = Convert.ToInt32(weight); 
                    a = Convert.ToInt32(temp);
                    b = Convert.ToInt32(signal);
                    s_light.Progress = Convert.ToInt32(light);
                    s_humidity.Progress = Convert.ToInt32(humi);
                    s_battery.Progress = Convert.ToInt32(akk);

                    s_temperature.Progress = a + 40;
                    s_signal_strength_1.Progress = b + 110;



                    SmullWeight.Text = StaticBox.Sensors["Вес груза"];
                    SmullTemperature.Text = StaticBox.Sensors["Температура"];
                    SmullLight.Text = StaticBox.Sensors["Освещенность"];
                    SmullHumidity.Text = StaticBox.Sensors["Влажность"];
                    SmullBattery.Text = StaticBox.Sensors["Уровень заряда аккумулятора"];
                    SmullNetworkSignal.Text = StaticBox.Sensors["Уровень сигнала"];
                    TextNameBox.Text = "(" + StaticBox.DeviceId + ")";

                    Toast.MakeText(this, "Успешно!", ToastLength.Long).Show();

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
                else
                {
                    ErrorResponseObject error = new ErrorResponseObject();
                    error = JsonConvert.DeserializeObject<ErrorResponseObject>(s_result);
                    Toast.MakeText(this, error.Errors[0], ToastLength.Long).Show();
                }
                
            }
            catch (Exception ex)
            {
                Toast.MakeText(this, ex.Message, ToastLength.Long).Show();
            }
        }
    }
}
            


