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
namespace GeoGeometry.Activity.Auth
{

    [Activity(Label = "DriverActivity")]

    public class DriverActivity : AppCompatActivity, IOnMapReadyCallback
    {

        private Button btn_exit_;

        private Button btn_free_for_order;

        private Button btn_change_container;

        private Button btn_open_close_container;

        private Button btn_lock_unlock_door;

        private Button btn_change_parameters;

        private Button btn_transfer_access;

        private EditText container_name;

        private Spinner s_situation;

        private EditText s_open_close_container;

        private EditText s_lock_unlock_door;

        private EditText s_weight;

        private EditText s_temperature;

        private EditText s_light;

        private EditText s_humidity; 

        private EditText s_battery;

        private static EditText s_signal_strength;

        private static EditText s_longitude;

        private static EditText s_latitude;

        private static EditText s_date_time;

        private static EditText s_payment;

        private string a_situation;

        private ProgressBar preloader;

        GoogleMap _googleMap;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.activity_driver);

            btn_change_container = FindViewById<Button>(Resource.Id.btn_change_container);
            btn_exit_ = FindViewById<Button>(Resource.Id.btn_exit_);
            btn_open_close_container = FindViewById<Button>(Resource.Id.btn_open_close_container);
            btn_lock_unlock_door = FindViewById<Button>(Resource.Id.btn_lock_unlock_door);
            btn_change_parameters = FindViewById<Button>(Resource.Id.btn_change_parameters);
            btn_transfer_access = FindViewById<Button>(Resource.Id.btn_transfer_access);
            btn_free_for_order = FindViewById<Button>(Resource.Id.btn_free_for_order);
            container_name = FindViewById<EditText>(Resource.Id.container_name);
            s_situation = FindViewById<Spinner>(Resource.Id.s_situation);
            s_open_close_container = FindViewById<EditText>(Resource.Id.s_open_close_container);
            s_lock_unlock_door = FindViewById<EditText>(Resource.Id.s_lock_unlock_door);
            s_weight = FindViewById<EditText>(Resource.Id.s_weight);
            s_temperature = FindViewById<EditText>(Resource.Id.TemperatureEdit);
            s_light = FindViewById<EditText>(Resource.Id.s_light);
            s_humidity = FindViewById<EditText>(Resource.Id.s_humidity);
            s_battery = FindViewById<EditText>(Resource.Id.s_battery);
            s_signal_strength = FindViewById<EditText>(Resource.Id.s_signal_strength);
            s_longitude = FindViewById<EditText>(Resource.Id.s_longitude);
            s_latitude = FindViewById<EditText>(Resource.Id.s_latitude);
            s_date_time = FindViewById<EditText>(Resource.Id.s_date_time);
            s_payment = FindViewById<EditText>(Resource.Id.s_payment);
            preloader = FindViewById<ProgressBar>(Resource.Id.preloader);
            
            MapFragment mapFragment = (MapFragment)FragmentManager.FindFragmentById(Resource.Id.fragmentMap);
            mapFragment.GetMapAsync(this);

            s_signal_strength.Focusable = false;
            s_signal_strength.LongClickable = false;
            s_situation.Focusable = false;
            s_situation.LongClickable = false;
            s_open_close_container.Focusable = false;
            s_open_close_container.LongClickable = false;
            s_lock_unlock_door.Focusable = false;
            s_lock_unlock_door.LongClickable = false;           
            s_payment.Focusable = false;
            s_payment.LongClickable = false;
            s_date_time.Focusable = false;
            s_date_time.LongClickable = false;
            s_latitude.Focusable = false;
            s_latitude.LongClickable = false;
            s_longitude.Focusable = false;
            s_longitude.LongClickable = false;
            container_name.Focusable = false;
            container_name.LongClickable = false;

            s_situation.Prompt = "Выбор роли";
            s_situation.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(Spinner_ItemSelected);
            var adapter = ArrayAdapter.CreateFromResource(this, Resource.Array.a_situation_loaded_container, Android.Resource.Layout.SimpleSpinnerItem);
            
            adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            s_situation.Adapter = adapter;

            string dir_path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);

            if (File.Exists(@"" + dir_path + "box_data.txt"))
            {
                string[] strok = File.ReadAllLines(dir_path + "box_data.txt");

                if (strok.Length != 0)
                {
                    GetInfoAboutBox(dir_path);
                }
                
            }
            BuildLocationRequest();
            BuildLocationCallBack();

            fusedLocationProviderClient = LocationServices.GetFusedLocationProviderClient(this);

            //ResetUser();
            fusedLocationProviderClient.RequestLocationUpdates(locationRequest,
                locationCallback, Looper.MyLooper());


            //var telephonyManager = (TelephonyManager)GetSystemService(Context.TelephonyService);
            //var signalStrengthListener = new SignalStrength();
            //_getGsmSignalStrengthButton.Click += DisplaySignalStrength;
            string id_page = Intent.GetStringExtra("idAction");// !!!
            switch (id_page)
            {
                case "1": 
                case "2": //Получение инфы контейнера
                    {
                        GetInfoAboutBox(dir_path);// метод получения данных с контейнера 
                    }
                    break;
                case "3": 
                    {
                        
                    }
                    break;
            }


            btn_free_for_order.Click += async delegate
            {
                Toast.MakeText(this, "Ваш статус: «Свободен для закозов»", ToastLength.Long).Show();
            };

            //переход на форму выбора контейнера
            btn_change_container.Click += async delegate
            {
                
                try
                {
                    Intent ContainerSelectionActivty = new Intent(this, typeof(Auth.ContainerSelection));
                    StartActivity(ContainerSelectionActivty);

                }
                catch(Exception ex)
                {
                    Toast.MakeText(this, "" + ex.Message, ToastLength.Long).Show();
                }
            };

            btn_transfer_access.Click += async delegate
                {
                    Intent mapActivity = new Intent(this, typeof(Auth.DriverActivity));
                    StartActivity(mapActivity);
                };

            //изменение состояния контейнера
            btn_open_close_container.Click += async delegate
            {
                try
                {
                    if (s_open_close_container.Text == "закрыт")
                        s_open_close_container.Text = "раскрыт";
                    else
                        s_open_close_container.Text = "закрыт";
                }
                catch(Exception ex)
                {
                    Toast.MakeText(this, "" + ex.Message, ToastLength.Long).Show();
                }
            };

            //изменение состояния дверей
            btn_lock_unlock_door.Click += async delegate
            {
                try
                {
                    if (s_lock_unlock_door.Text == "заблокирована")
                        s_lock_unlock_door.Text = "разблокирована";
                    else
                        s_lock_unlock_door.Text = "заблокирована";
                }
                catch (Exception ex)
                {
                    Toast.MakeText(this, "" + ex.Message, ToastLength.Long).Show();
                }
            };

            //изменение ПИН-кода, очистка полей
            //btn_change_pin_code.Click += async delegate
            //{
            //    try
            //    {
            //        s_pin_access_code.Text = "";
            //    }
            //    catch (Exception ex)
            //    {
            //        Toast.MakeText(this, "" + ex.Message, ToastLength.Long).Show();
            //    }
            //};

            //редактирование данных контейнера
            btn_change_parameters.Click += async delegate
            {
                try
                {
                    if(a_situation == "Отсутствует")
                    {
                        Toast.MakeText(this, "Выберите состояние контейнера",ToastLength.Long).Show();
                    }
                    else
                    {
                        preloader.Visibility = Android.Views.ViewStates.Visible;
                        
                        StaticBox.Temperature = s_temperature.Text;
                        StaticBox.Weight = s_weight.Text;
                        StaticBox.Light = Convert.ToInt32(s_light.Text);
                        StaticBox.Wetness = s_humidity.Text;
                        //StaticBox.Code = s_pin_access_code.Text;
                        StaticBox.Name = container_name.Text;
                        StaticBox.IsOpenedBox = (s_open_close_container.Text == "раскрыт") ? true : false;
                        //Situation = s_situation.Text,
                        StaticBox.IsOpenedDoor = (s_lock_unlock_door.Text == "разблокирована") ? true : false;
                        StaticBox.BatteryPower = s_battery.Text;

                        if (a_situation == "На складе")
                        {
                            StaticBox.BoxState = ContainerState.onBase;
                        }
                        else if (a_situation == "На автомобиле")
                        {
                            StaticBox.BoxState = ContainerState.onCar;
                        }
                        else if (a_situation == "Выгруженным у грузоотправителя")
                        {
                            StaticBox.BoxState = ContainerState.onConsignee;
                        }
                        else if (a_situation == "После разгрузки у грузополучателя")
                        {
                            StaticBox.BoxState = ContainerState.onShipper;
                        }

                       

                        SmartBox container = new SmartBox
                        {
                            Id = StaticBox.SmartBoxId,
                            Temperature = StaticBox.Temperature,
                            Name = StaticBox.Name,
                            // Weight = StaticBox.Weight,
                            Weight = StaticBox.Weight,
                            Light = StaticBox.Light,
                            Wetness = StaticBox.Wetness,
                            Code = StaticBox.Code,
                            IsOpenedBox = StaticBox.IsOpenedBox,
                            BoxState = StaticBox.BoxState,
                            IsOpenedDoor = StaticBox.IsOpenedDoor,
                            BatteryPower = StaticBox.BatteryPower
                        };

                       

                        var myHttpClient = new HttpClient();

                        var uri = ("http://iot-tmc-cen.1gb.ru/api/container/editbox?id=" + container.Id + "&IsOpenedBox=" + container.IsOpenedBox + "&Name=" + container.Name + "&IsOpenedDoor=" + container.IsOpenedDoor + "&Weight=" + container.Weight + "&Light=" + container.Light + "&Code=" + container.Code + "&Temperature=" + container.Temperature + "&Wetness=" + container.Wetness + "&BatteryPower=" + container.BatteryPower + "&BoxState=" + container.BoxState);
                        var uri2 = ("http://81.177.136.11:8003/sensor?id=" + container.Id + "&IsOpenedBox=" + container.IsOpenedBox + "&Name=" + container.Name + "&IsOpenedDoor=" + container.IsOpenedDoor + "&Weight=" + container.Weight + "&Light=" + container.Light + "&Code=" + container.Code + "&Temperature=" + container.Temperature + "&Wetness=" + container.Wetness + "&BatteryPower=" + container.BatteryPower + "&BoxState=" + container.BoxState);


                        HttpResponseMessage response = await myHttpClient.PutAsync(uri.ToString(), new StringContent(JsonConvert.SerializeObject(container), Encoding.UTF8, "application/json"));
                        HttpResponseMessage responseFromAnotherServer = await myHttpClient.PutAsync(uri2.ToString(), new StringContent(JsonConvert.SerializeObject(container), Encoding.UTF8, "application/json"));

                        AuthApiData<BaseResponseObject> o_data = new AuthApiData<BaseResponseObject>();

                        string s_result;
                        using (HttpContent responseContent = response.Content)
                        {
                            s_result = await responseContent.ReadAsStringAsync();
                        }

                        string s_result_from_server;
                        using (HttpContent responseContent = responseFromAnotherServer.Content)
                        {
                            s_result_from_server = await responseContent.ReadAsStringAsync();
                        }

                        o_data = JsonConvert.DeserializeObject<AuthApiData<BaseResponseObject>>(s_result);

                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            Toast.MakeText(this, o_data.Message, ToastLength.Long).Show();
                        }
                        Intent.PutExtra("idAction", "2");
                        //перезапуск страницы

                        Recreate();
                    }                   
                }
                catch (Exception ex)
                {
                    Toast.MakeText(this, "" + ex.Message, ToastLength.Long).Show();
                }
            };

            btn_exit_.Click += async delegate
            {
                File.Delete(dir_path + "user_data.txt");              
                ClearField();
                Intent ActivityMain = new Intent(this, typeof(MainActivity));
                StartActivity(ActivityMain);
            };

        }

        /// <summary>
        /// сбор информации о контейнере
        /// </summary>
        /// <param name="dir_path"></param>
        private async void GetInfoAboutBox(string dir_path)
        {
            try
            {
                ContainerResponse container = new ContainerResponse();

                //извлечение данных контейнера из файла
                //using (FileStream fs = new FileStream(dir_path + "box_data.txt", FileMode.OpenOrCreate))
                //{
                //    container = await System.Text.Json.JsonSerializer.DeserializeAsync<ContainerResponse>(fs);
                //}

                //пример чтения данных с файла
                string file_data_remember;
                using (FileStream file = new FileStream(dir_path + "box_data.txt", FileMode.Open, FileAccess.Read))
                {
                    // преобразуем строку в байты
                    byte[] array = new byte[file.Length];
                    // считываем данные
                    file.Read(array, 0, array.Length);
                    // декодируем байты в строку
                    file_data_remember = Encoding.Default.GetString(array);
                    file.Close();
                }

                container = JsonConvert.DeserializeObject<ContainerResponse>(file_data_remember);

                var myHttpClient = new HttpClient();
                
                var uri = new Uri("http://iot.tmc-centert.ru/api/container/getbox?id=" + container.SmartBoxId);
                HttpResponseMessage response = await myHttpClient.GetAsync(uri);

                AuthApiData<BoxDataResponse> o_data = new AuthApiData<BoxDataResponse>();

                string s_result;
                using (HttpContent responseContent = response.Content)
                {
                    s_result = await responseContent.ReadAsStringAsync();
                }

                o_data = JsonConvert.DeserializeObject<AuthApiData<BoxDataResponse>>(s_result);// !!!!
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    if (o_data.Status == "0")
                    {
                        Toast.MakeText(this, o_data.Message, ToastLength.Long).Show();
                        BoxDataResponse exported_data = new BoxDataResponse();
                        exported_data = o_data.ResponseData;

                        StaticBox.AddInfoBox(exported_data);
                        //добавляем инфу о найденном контейнере
                        container_name.Text = container.Name;
                        s_open_close_container.Text = (exported_data.IsOpenedBox == false) ? "закрыт" : "раскрыт";
                        s_weight.Text = exported_data.Weight.Replace(",",".");
                        s_lock_unlock_door.Text = (exported_data.IsOpenedDoor == false) ? "заблокирована" : "разблокирована";

                        var boxState = s_open_close_container.Text;
                        var doorState = s_lock_unlock_door.Text;

                        if (exported_data.BoxState == ContainerState.onBase)
                        {
                            a_situation = "На складе";
                        }
                        else if (exported_data.BoxState == ContainerState.onCar)
                        {                           
                            a_situation = "На автомобиле";
                        }
                        else if (exported_data.BoxState == ContainerState.onConsignee)
                        {
                            a_situation = "Выгруженным у грузоотправителя";
                        }
                        else if (exported_data.BoxState == ContainerState.onShipper)
                        {                         
                            a_situation = "После разгрузки у грузополучателя";
                        }                       

                        s_temperature.Text = exported_data.Temperature.Replace(",", ".");
                        if(exported_data.Light <= 50)
                        {
                            s_light.Text = "Нет";
                        }
                        else if(exported_data.Light > 50)
                        {
                            s_light.Text = "Да";
                        }
                        //s_pin_access_code.Text = (exported_data.Code == null) ? "0000" : exported_data.Code;
                        s_humidity.Text = exported_data.Wetness.Replace(",", ".");
                        s_battery.Text = exported_data.BatteryPower.Replace(",", ".");
                        btn_lock_unlock_door.Text = "Заблокировать/Разблокировать";
                        s_signal_strength.Text = "Хороший";
                    }
                    else
                    {
                        Toast.MakeText(this, o_data.Message, ToastLength.Long).Show();
                    }
                }
            }
            catch (Exception ex)
            {
                Toast.MakeText(this, "" + ex.Message, ToastLength.Long).Show();
            }

        }

        FusedLocationProviderClient fusedLocationProviderClient;
        LocationRequest locationRequest;
        LocationCallback locationCallback;


        public void OnMapReady(GoogleMap googleMap)
        {
            _googleMap = googleMap;////11111

            MarkerOptions markerOptions = new MarkerOptions();
            LatLng location = new LatLng(StaticBox.Latitude, StaticBox.Longitude);
            markerOptions.SetPosition(location);
            markerOptions.SetTitle("Я здесь");
            googleMap.AddMarker(markerOptions);

            CameraPosition.Builder builder = CameraPosition.InvokeBuilder();
            builder.Target(location);
            builder.Zoom(18);
            builder.Bearing(0);
            builder.Tilt(65);

            CameraPosition cameraPosition = builder.Build();
            CameraUpdate cameraUpdate = CameraUpdateFactory.NewCameraPosition(cameraPosition);

            googleMap.UiSettings.ZoomControlsEnabled = true;
            googleMap.UiSettings.CompassEnabled = true;
            googleMap.MoveCamera(cameraUpdate);

            
        }
        private void BuildLocationCallBack()
        {
            locationCallback = new DriverLocationCallBack(this);
        }

        private void BuildLocationRequest()
        {
            locationRequest = new LocationRequest();
            locationRequest.SetPriority(LocationRequest.PriorityBalancedPowerAccuracy);
            locationRequest.SetInterval(1000);
            locationRequest.SetFastestInterval(3000);
            locationRequest.SetSmallestDisplacement(10f);
        }

        /// <summary>
        /// Подключение к карте api ключ. 
        /// https://console.developers.google.com/apis/credentials?project=geogeometry&hl=RU&supportedpurview=project
        /// информация https://docs.microsoft.com/ru-ru/xamarin/android/platform/maps-and-location/maps/maps-api#google-maps-api-prerequisites
        /// </summary>

        internal class DriverLocationCallBack : LocationCallback
        {
            private DriverActivity driverActivity;

            public DriverLocationCallBack(DriverActivity driverActivity)
            {
                this.driverActivity = driverActivity;
            }

            public override async void OnLocationResult(LocationResult result)
            {
                base.OnLocationResult(result);

                try
                {
                    StaticBox.Latitude = result.LastLocation.Latitude;
                    StaticBox.Longitude = result.LastLocation.Longitude;
                    StaticBox.Signal = 0;
                    StaticBox.Date = DateTime.Now;

                    s_longitude.Text = result.LastLocation.Latitude.ToString();
                    s_latitude.Text = result.LastLocation.Longitude.ToString();
                    s_date_time.Text = DateTime.Now.ToString();
                    s_signal_strength.Text = "Хороший";

                    // Получаю информацию о клиенте.
                    BoxLocation gpsLocation = new BoxLocation
                    {
                        id = StaticBox.SmartBoxId,
                        lat1 = StaticBox.Latitude,
                        lon1 = StaticBox.Longitude,
                        signal = StaticBox.Signal,
                        date = StaticBox.Date
                    };



                    var myHttpClient = new HttpClient();
                    var uri = new Uri("http://iot-tmc-cen.1gb.ru/api/container/setcontainerlocation?id=" + gpsLocation.id + "&lat1=" + gpsLocation.lat1 + "&lon1=" + gpsLocation.lon1 + "&signal=" + gpsLocation.signal + "&date=" + gpsLocation.date);
                    var uri2 = new Uri("http://81.177.136.11:8003/geo?id=" + gpsLocation.id + "&lat1=" + gpsLocation.lat1 + "&lon1=" + gpsLocation.lon1 + "&signal=" + gpsLocation.signal + "&date=" + gpsLocation.date);
                    //json структура.
                    var formContent = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                { "Id", gpsLocation.id },
                { "Lon1", gpsLocation.lon1.ToString()},
                { "Lat1", gpsLocation.lat1.ToString()},
                { "Signal", "0"},
                { "Date", DateTime.Now.ToString()}
            });

                    HttpResponseMessage response = await myHttpClient.PostAsync(uri.ToString(), formContent);// !!!!
                    HttpResponseMessage responseFromAnotherServer = await myHttpClient.PostAsync(uri2.ToString(), new StringContent(JsonConvert.SerializeObject(gpsLocation), Encoding.UTF8, "application/json"));
                    AuthApiData<BaseResponseObject> o_data = new AuthApiData<BaseResponseObject>();

                    string s_result;
                    using (HttpContent responseContent = response.Content)
                    {
                        s_result = await responseContent.ReadAsStringAsync();
                    }

                    string s_result_from_another_server;
                    using (HttpContent responseContent = responseFromAnotherServer.Content)
                    {
                        s_result_from_another_server = await responseContent.ReadAsStringAsync();
                    }

                    o_data = JsonConvert.DeserializeObject<AuthApiData<BaseResponseObject>>(s_result);
                }
                catch (Exception ex)
                {
                    
                    throw;
                }
                

            }
        }

        private void Spinner_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            var spinner = sender as Spinner;
            a_situation = spinner.GetItemAtPosition(e.Position).ToString();
        }

    //    spinner.setOnItemSelectedListener(new OnItemSelectedListener()
    //    {

          

    //public void onItemSelected(AdapterView<?> parent, View view, int pos,
    //        long id)
    //        {
    //            // TODO Auto-generated method stub
    //            ((TextView)parent.getChildAt(0)).setTextColor(Color.MAGENTA);
    //            ((TextView)parent.getChildAt(0)).setTextSize(12);
    //        }

           

    //public void onNothingSelected(AdapterView<?> arg0)
    //        {
    //            // TODO Auto-generated method stub

    //        }
    //    });

        //очистка всех полей
        void ClearField()
        {
            container_name.Text = "";
            //s_situation.Text = "";
            s_open_close_container.Text = "";
            s_lock_unlock_door.Text = "";
            s_weight.Text = "";
            s_temperature.Text = "";
            s_light.Text = "";
            s_humidity.Text = "";
            s_battery.Text = "";
            s_signal_strength.Text = "";
            s_longitude.Text = "";
            s_latitude.Text = "";
            s_date_time.Text = "";
        }

        

        //void DisplaySignalStrength(object sender, EventArgs e)
        //{
        //    var telephonyManager.Listen(signalStrengthListener, PhoneStateListenerFlags.SignalStrengths);
        //    var signalStrengthListener.SignalStrengthChanged += HandleSignalStrengthChanged;
        //}

        //void HandleSignalStrengthChanged(int strength)
        //{
        //    // We want this to be a one-shot thing when the button is pushed. Make sure to unhook everything
        //    var signalStrengthListener.SignalStrengthChanged -= HandleSignalStrengthChanged;
        //    var telephonyManager.Listen(signalStrengthListener, PhoneStateListenerFlags.None);

        //    // Update the UI with text and an image.
        //    var gmsStrengthImageView.SetImageLevel(strength);
        //    gmsStrengthTextView.Text = string.Format("GPS Signal Strength ({0}):", strength);
        //}
    }
}


