using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using GeoGeometry.Container;
using GeoGeometry.Model;
using GeoGeometry.Model.Box;
using GeoGeometry.Service;
using Newtonsoft.Json;
using Plugin.Settings;
using Xamarin.Forms.Internals;

namespace GeoGeometry.Activity.Auth
{
    [Activity(Label = "TestContainerCondition")]
    class TestContainerCondition: AppCompatActivity
    {
        private RelativeLayout condition;

        private const string mqttServer = "farmer.cloudmqtt.com";
        private const int mqttPort = 10613; //Your port number
        private const string mqttUserName = "mashtgbr";
        private const string mqttPassword = "S9czqATnDuvL";
        private string origin_topic;


        MQTTService mQTT;

        private TextView TextConditionCenter;

        private Button ContainerFold;

        private Button ContainerOff;

        private Button ContainerUnfold;

        private Button ContainerOpen;

        private Button ContainerClose;

        private Button btn_save_status_container;

        private Spinner s_situation_container;

        private string a_situation;

        private ImageView box_lay_fold;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.activity_test_condition);

            condition = FindViewById<RelativeLayout>(Resource.Id.test_condition);
            TextConditionCenter = FindViewById<TextView>(Resource.Id.TextConditionCenter);

            ContainerFold = FindViewById<Button>(Resource.Id.ContainerFold);
            ContainerOff = FindViewById<Button>(Resource.Id.ContainerOff);
            ContainerUnfold = FindViewById<Button>(Resource.Id.ContainerUnfold);
            ContainerOpen = FindViewById<Button>(Resource.Id.ContainerOpen);
            ContainerClose = FindViewById<Button>(Resource.Id.ContainerClose);

            btn_save_status_container = FindViewById<Button>(Resource.Id.btn_save_parameters);
            box_lay_fold = FindViewById<ImageView>(Resource.Id.box_lay_fold);
            s_situation_container = FindViewById<Spinner>(Resource.Id.s_situation);

            origin_topic = StaticBox.Imitators[StaticBox.DeviceId];

            var client_data = new MQTTClient()
            {
                ClientId = "ESP32AndroidImitatorClient",
                Password = mqttPassword,
                Port = mqttPort,
                Server = mqttServer,
                UserName = mqttUserName
            };

            mQTT = new MQTTService(client_data, this);

            GetInfoAboutBox();
            //StaticBox.Sensors["Местоположение контейнера"] = "На автомобиле";
            s_situation_container.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(Spinner_ItemSelected);
            var adapter = ArrayAdapter.CreateFromResource(this, Resource.Array.a_situation_loaded_container, Android.Resource.Layout.SimpleSpinnerItem);

            adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            s_situation_container.Adapter = adapter;

            btn_save_status_container.Click += async delegate
            {
                try
                {

                    //StaticBox.Sensors["Состояние контейнера"] = (s_open_close_container.Text == "сложен") ? "0" : "1";
                    //StaticBox.Sensors["Состояние дверей"] = (s_lock_unlock_door.Text == "закрыта") ? "0" : "1";
                    

                    var o_data = await ContainerService.EditBox();

                    if (o_data.Status == "1")
                    {
                        Toast.MakeText(this, o_data.Message, ToastLength.Long).Show();
                        GetInfoAboutBox();
                    }
                    else
                    {
                        GetInfoAboutBox();
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

            ContainerFold.Click += delegate (object select, EventArgs eventArgs)
            {
                SetBtnBackground("fold");
                StaticBox.Sensors["Состояние контейнера"] = "0";
                mQTT.PublishBoxState(origin_topic, "2");
            };
            //остановка
            ContainerOff.Click += delegate (object select, EventArgs eventArgs)
            {
                SetBtnBackground("off");
                StaticBox.Sensors["Состояние контейнера"] = "2";
                mQTT.PublishBoxState(origin_topic, "0");
            };
            //разложить
            ContainerUnfold.Click += delegate (object select, EventArgs eventArgs)
            {
                SetBtnBackground("unfold");
                StaticBox.Sensors["Состояние контейнера"] = "1";
                mQTT.PublishBoxState(origin_topic, "1");
            };

            //изменение состояния дверей
            ContainerOpen.Click += async delegate
            {
                try
                {
                    StaticBox.Sensors["Состояние дверей"] = "1";
                    SetBtnBackground("open");
                }
                catch (Exception ex)
                {
                    Toast.MakeText(this, "" + ex.Message, ToastLength.Long).Show();
                }
            };

            //изменение состояния дверей
            ContainerClose.Click += async delegate
            {
                try
                {
                    StaticBox.Sensors["Состояние дверей"] = "0";
                    SetBtnBackground("close");
                }
                catch (Exception ex)
                {
                    Toast.MakeText(this, "" + ex.Message, ToastLength.Long).Show();
                }
            };

            //изменение состояния дверей
            //btn_lock_unlock_door.Click += async delegate
            //{
            //    try
            //    {
            //        if (s_lock_unlock_door.Text == "закрыта")
            //        {
            //            s_lock_unlock_door.Text = "открыта";
            //            box_lay_fold.SetImageResource(Resource.Drawable.open_door);
            //        }
            //        else if (s_lock_unlock_door.Text == "открыта" && s_open_close_container.Text == "разложен")
            //        {
            //            s_lock_unlock_door.Text = "закрыта";
            //            box_lay_fold.SetImageResource(Resource.Drawable.close_door);
            //        }
            //        else if (s_open_close_container.Text == "сложен" && s_lock_unlock_door.Text == "открыта")
            //        {
            //            Android.App.AlertDialog.Builder alert = new Android.App.AlertDialog.Builder(this);
            //            alert.SetTitle("Внимание !");
            //            alert.SetMessage("Невозможно изменить состояние дверей.");
            //            alert.SetPositiveButton("Закрыть", (senderAlert, args) => {
            //                Toast.MakeText(this, "Предупреждение было закрыто!", ToastLength.Short).Show();
            //            });
            //            Dialog dialog = alert.Create();
            //            dialog.Show();
            //        }
            //        else if (s_open_close_container.Text == null && s_lock_unlock_door.Text == null)
            //        {
            //            Android.App.AlertDialog.Builder alert = new Android.App.AlertDialog.Builder(this);
            //            alert.SetTitle("Внимание !");
            //            alert.SetMessage("Невозможно изменить состояние дверей и контейнера.");
            //            alert.SetPositiveButton("Закрыть", (senderAlert, args) => {
            //                Toast.MakeText(this, "Предупреждение было закрыто!", ToastLength.Short).Show();
            //            });
            //            Dialog dialog = alert.Create();
            //            dialog.Show();
            //        }
            //        else
            //        {
            //            Android.App.AlertDialog.Builder alert = new Android.App.AlertDialog.Builder(this);
            //            alert.SetTitle("Внимание !");
            //            alert.SetMessage("Невозможно изменить состояние дверей.");
            //            alert.SetPositiveButton("Закрыть", (senderAlert, args) => {
            //                Toast.MakeText(this, "Предупреждение было закрыто!", ToastLength.Short).Show();
            //            });
            //            Dialog dialog = alert.Create();
            //            dialog.Show();
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        Toast.MakeText(this, "" + ex.Message, ToastLength.Long).Show();
            //    }
            //};
        }

        //сбор информации о состоянии модели
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

                string s_result;
                using (HttpContent responseContent = response.Content)
                {
                    s_result = await responseContent.ReadAsStringAsync();
                }

                if (response.IsSuccessStatusCode)
                {
                    Status o_data = new Status();

                    o_data = JsonConvert.DeserializeObject<Status>(s_result);

                    //string s_result;
                    //using (HttpContent responseContent = response.Content)
                    //{
                    //    s_result = await responseContent.ReadAsStringAsync();
                    //}
                    //StaticBox.AddInfoObjects(o_boxes_data);
                    //В статик бокс закомментируй 9 свойств
                    StaticBox.Sensors["Температура"] = o_data.status.Sensors["Температура"];
                    StaticBox.Sensors["Влажность"] = o_data.status.Sensors["Влажность"];
                    StaticBox.Sensors["Освещенность"] = o_data.status.Sensors["Освещенность"];
                    StaticBox.Sensors["Уровень заряда аккумулятора"] = o_data.status.Sensors["Уровень заряда аккумулятора"];
                    StaticBox.Sensors["Уровень сигнала"] = o_data.status.Sensors["Уровень сигнала"];
                    StaticBox.Sensors["Состояние дверей"] = o_data.status.Sensors["Состояние дверей"];
                    StaticBox.Sensors["Состояние контейнера"] = o_data.status.Sensors["Состояние контейнера"];
                    StaticBox.Sensors["Местоположение контейнера"] = o_data.status.Sensors["Местоположение контейнера"];
                    //StaticBox.CreatedAtSensors = o_data.ResponseData.Objects[0].CreatedAt;
                    if (StaticBox.Sensors["Состояние контейнера"] == "0")
                        StaticBox.Sensors["Вес груза"] = "0";
                    else
                        StaticBox.Sensors["Вес груза"] = o_data.status.Sensors["Вес груза"];

                    string box_state = (StaticBox.Sensors["Состояние контейнера"] == "0") ? "fold" : (StaticBox.Sensors["Состояние контейнера"] == "1")? "unfold" : "off";
                    string rollete_state = (StaticBox.Sensors["Состояние дверей"] == "0") ? "close" : "open";

                    SetBtnBackground(box_state);
                    SetBtnBackground(rollete_state);
                    //if (s_open_close_container.Text == "сложен")
                    //{
                    //    s_lock_unlock_door.Text = "открыта";
                    //    box_lay_fold.SetImageResource(Resource.Drawable.close_box);
                    //}
                    //else
                    //{
                    //    s_lock_unlock_door.Text = (StaticBox.Sensors["Состояние дверей"] == "0") ? "закрыта" : "открыта";
                    //    if (s_lock_unlock_door.Text == "закрыта" && s_open_close_container.Text == "разложен")
                    //    {
                    //        box_lay_fold.SetImageResource(Resource.Drawable.close_door);
                    //    }
                    //    else if (s_lock_unlock_door.Text == "открыта" && s_open_close_container.Text == "разложен")
                    //    {
                    //        box_lay_fold.SetImageResource(Resource.Drawable.open_door);
                    //    }
                    //}
                    a_situation = StaticBox.Sensors["Местоположение контейнера"];

                    s_situation_container.SetSelection(Resources.GetStringArray(Resource.Array.a_situation_loaded_container).IndexOf(x => x == a_situation));

                    Toast.MakeText(this, "Успешно!", ToastLength.Long).Show();
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
        private void Spinner_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            var spinner = sender as Spinner;
            a_situation = spinner.GetItemAtPosition(e.Position).ToString();
            StaticBox.Sensors["Местоположение контейнера"] = a_situation;
        }

        private void SetBtnBackground(string v)
        {
            switch (v)
            {
                case "fold":
                    ContainerFold.SetBackgroundResource(Resource.Drawable.Left_Button_Press);
                    ContainerUnfold.SetBackgroundResource(Resource.Drawable.Right_Button_NoPress);
                    ContainerOff.SetBackgroundResource(Resource.Drawable.Central_Button_NoPress);
                    box_lay_fold.SetImageResource(Resource.Drawable.close_box);
                    break;
                case "unfold":
                    ContainerFold.SetBackgroundResource(Resource.Drawable.Left_Button_NoPress);
                    ContainerUnfold.SetBackgroundResource(Resource.Drawable.Right_Button_Press);
                    ContainerOff.SetBackgroundResource(Resource.Drawable.Central_Button_NoPress);
                    break;
                case "off":
                    ContainerFold.SetBackgroundResource(Resource.Drawable.Left_Button_NoPress);
                    ContainerUnfold.SetBackgroundResource(Resource.Drawable.Right_Button_NoPress);
                    ContainerOff.SetBackgroundResource(Resource.Drawable.Central_Button_Press);
                    break;
                case "open":
                    ContainerOpen.SetBackgroundResource(Resource.Drawable.Left_Button_Press);
                    ContainerClose.SetBackgroundResource(Resource.Drawable.Right_Button_NoPress);
                    box_lay_fold.SetImageResource(Resource.Drawable.open_door);
                    break;
                case "close":
                    ContainerOpen.SetBackgroundResource(Resource.Drawable.Left_Button_NoPress);
                    ContainerClose.SetBackgroundResource(Resource.Drawable.Right_Button_Press);
                    box_lay_fold.SetImageResource(Resource.Drawable.close_door);
                    break;
            }
        }

        protected override void OnDestroy()
        {
            mQTT.Stop();
            base.OnDestroy();
        }
    }
}