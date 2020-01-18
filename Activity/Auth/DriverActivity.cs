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

namespace GeoGeometry.Activity.Auth
{

    [Activity(Label = "DriverActivity")]

    public class DriverActivity : AppCompatActivity//, IOnMapReadyCallback
    {

        //private Button btn_exit_;

        //private Button btn_free_for_order;

        //private Button btn_change_container;

        //private Button btn_open_close_container;

        //private Button btn_lock_unlock_door;

        //private Button btn_change_parameters;

        //private Button btn_historyOfChange;

        //private Button btn_transfer_access;

        //private EditText container_name;

        //private Spinner s_situation;

        //private EditText s_open_close_container;

        //private EditText s_lock_unlock_door;

        //private static EditText s_longitude;

        //private static EditText s_latitude;

        //private static EditText s_date_time;

        //private static EditText s_payment;

        //private string a_situation;

        //private ProgressBar preloader;
        private RelativeLayout box_container;

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

        private static SeekBar s_signal_strength_1;

        private Button btn_save_parameters;

        //GoogleMap _googleMap;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.activity_box);
            StaticMenu.id_page = 1;


            box_container = FindViewById<RelativeLayout>(Resource.Id.box_container);
            btn_save_parameters = FindViewById<Button>(Resource.Id.btn_save_parameters);
            SmullWeight = FindViewById<TextView>(Resource.Id.SmullWeight);
            SmullTemperature = FindViewById<TextView>(Resource.Id.SmullTemperature);
            SmullLight = FindViewById<TextView>(Resource.Id.SmullLight);
            SmullHumidity = FindViewById<TextView>(Resource.Id.SmullHumidity);
            SmullBattery = FindViewById<TextView>(Resource.Id.SmullBattery);
            SmullNetworkSignal = FindViewById<TextView>(Resource.Id.SmullNetworkSignal);
            s_weight = FindViewById<SeekBar>(Resource.Id.s_weight);
            s_temperature = FindViewById<SeekBar>(Resource.Id.TemperatureEdit);
            s_light = FindViewById<SeekBar>(Resource.Id.s_light);
            s_humidity = FindViewById<SeekBar>(Resource.Id.s_humidity);
            s_battery = FindViewById<SeekBar>(Resource.Id.s_battery);
            s_signal_strength_1 = FindViewById<SeekBar>(Resource.Id.s_signal_strength_1);

            s_weight.ProgressChanged += (object sender, SeekBar.ProgressChangedEventArgs e) =>
            {
                if (e.FromUser)
                {
                    SmullWeight.Text = string.Format("{0}", e.Progress);
                }
            };
            s_temperature.ProgressChanged += (object sender, SeekBar.ProgressChangedEventArgs e) =>
            {
                if (e.FromUser)
                {
                    SmullTemperature.Text = string.Format("{0}", e.Progress);
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
                    SmullNetworkSignal.Text = string.Format("{0}", e.Progress);
                }
            };


            //btn_exit_ = FindViewById<Button>(Resource.Id.btn_exit_);
            //btn_open_close_container = FindViewById<Button>(Resource.Id.btn_open_close_container);
            //btn_lock_unlock_door = FindViewById<Button>(Resource.Id.btn_lock_unlock_door);
            //btn_change_parameters = FindViewById<Button>(Resource.Id.btn_change_parameters);
            //btn_transfer_access = FindViewById<Button>(Resource.Id.btn_transfer_access);

            //container_name = FindViewById<EditText>(Resource.Id.container_name);
            //btn_historyOfChange = FindViewById<Button>(Resource.Id.btn_historyOfChange);
            //s_situation = FindViewById<Spinner>(Resource.Id.s_situation);
            //s_open_close_container = FindViewById<EditText>(Resource.Id.s_open_close_container);
            //s_lock_unlock_door = FindViewById<EditText>(Resource.Id.s_lock_unlock_door);
            //s_longitude = FindViewById<EditText>(Resource.Id.s_longitude);
            //s_latitude = FindViewById<EditText>(Resource.Id.s_latitude);
            //s_date_time = FindViewById<EditText>(Resource.Id.s_date_time);

            //preloader = FindViewById<ProgressBar>(Resource.Id.preloader);

            //MapFragment mapFragment = (MapFragment)FragmentManager.FindFragmentById(Resource.Id.fragmentMap);
            //mapFragment.GetMapAsync(this);

            //s_situation.Focusable = false;
            //s_situation.LongClickable = false;
            //s_open_close_container.Focusable = false;
            //s_open_close_container.LongClickable = false;
            //s_lock_unlock_door.Focusable = false;
            //s_lock_unlock_door.LongClickable = false;           

            //s_date_time.Focusable = false;
            //s_date_time.LongClickable = false;
            //s_latitude.Focusable = false;
            //s_latitude.LongClickable = false;
            //s_longitude.Focusable = false;
            //s_longitude.LongClickable = false;


            //s_situation.Prompt = "Выбор роли";
            //s_situation.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(Spinner_ItemSelected);
            //var adapter = ArrayAdapter.CreateFromResource(this, Resource.Array.a_situation_loaded_container, Android.Resource.Layout.SimpleSpinnerItem);

            //adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            //s_situation.Adapter = adapter;

            string dir_path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            GetInfoAboutBox();
            //if (File.Exists(@"" + dir_path + "box_data.txt"))
            //{
            //    string[] strok = File.ReadAllLines(dir_path + "box_data.txt");

            //    if (strok.Length != 0)
            //    {
            //        GetInfoAboutBox(dir_path);
            //    }

            //}
            //BuildLocationRequest();
            //BuildLocationCallBack();

            //fusedLocationProviderClient = LocationServices.GetFusedLocationProviderClient(this);

            //ResetUser();
            //fusedLocationProviderClient.RequestLocationUpdates(locationRequest,
            //    locationCallback, Looper.MyLooper());


            //var telephonyManager = (TelephonyManager)GetSystemService(Context.TelephonyService);
            //var signalStrengthListener = new SignalStrength();
            //_getGsmSignalStrengthButton.Click += DisplaySignalStrength;
            //string id_page = Intent.GetStringExtra("idAction");// !!!
            //switch (id_page)
            //{
            //    case "1": 
            //    case "2": 
            //        {
            //            GetInfoAboutBox(dir_path);
            //        break;
            //    case "3": 
            //        {

            //        }
            //        break;
            //}


            //btn_free_for_order.Click += async delegate
            //{
            //    Toast.MakeText(this, "Ваш статус: «Свободен для закозов»", ToastLength.Long).Show();
            //};

            //переход на форму выбора контейнера


            //btn_transfer_access.Click += async delegate
            //    {
            //        try
            //        {
            //            Intent ActivityC = new Intent(this, typeof(Auth.ContainerSelection));
            //            StartActivity(ActivityC);
            //        }
            //        catch (Exception ex)
            //        {
            //            Toast.MakeText(this, "" + ex.Message, ToastLength.Long).Show();
            //        }
            //    };

            //btn_historyOfChange.Click += async delegate
            //{
            //    Intent Driver = new Intent(this, typeof(Auth.SensorsDataActivity));
            //    StartActivity(Driver);
            //};

            //изменение состояния контейнера
            //btn_open_close_container.Click += async delegate
            //{
            //    try
            //    {
            //        if (s_open_close_container.Text == "сложен")
            //            s_open_close_container.Text = "разложен";
            //        else 
            //            s_open_close_container.Text = "сложен";

            //    }
            //    catch(Exception ex)
            //    {
            //        Toast.MakeText(this, "" + ex.Message, ToastLength.Long).Show();
            //    }
            //};

            //изменение состояния дверей
            //btn_lock_unlock_door.Click += async delegate
            //{
            //    try
            //    {
            //        if (s_lock_unlock_door.Text == "закрыта" && s_open_close_container.Text != "сложен")
            //            s_lock_unlock_door.Text = "открыта";
            //        else if (s_open_close_container.Text != "сложен")
            //            s_lock_unlock_door.Text = "закрыта";
            //        else
            //            Toast.MakeText(this, "" + "Невозможно изменить состояния дверей.", ToastLength.Long).Show();
            //    }
            //    catch (Exception ex)
            //    {
            //        Toast.MakeText(this, "" + ex.Message, ToastLength.Long).Show();
            //    }
            //};

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
            btn_save_parameters.Click += async delegate
            {
                try
                {

                    StaticBox.Sensors["Вес груза"] = s_weight.Progress.ToString();
                    StaticBox.Sensors["Температура"] = s_temperature.Progress.ToString();
                    StaticBox.Sensors["Влажность"] = s_humidity.Progress.ToString();
                    StaticBox.Sensors["Освещенность"] = s_light.Progress.ToString();
                    StaticBox.Sensors["Уровень сигнала"] = s_signal_strength_1.Progress.ToString();
                    StaticBox.Sensors["Уровень заряда аккумулятора"] = s_battery.Progress.ToString();
                    
                    var o_data = await ContainerService.EditBox();

                    if (o_data.Status == "1")
                    {
                        Toast.MakeText(this, o_data.Message, ToastLength.Long).Show();
                        GetInfoAboutBox();
                    }
                    else
                    {
                        Intent authActivity = new Intent(this, typeof(Auth.TakePhoto));
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

            //string s_result;
            //using (HttpContent responseContent = response.Content)
            //{
            //    s_result = await responseContent.ReadAsStringAsync();
            //}

            o_data.ResponseData = new ListResponse<BoxDataResponse>();
            o_data = JsonConvert.DeserializeObject<AuthApiData<ListResponse<BoxDataResponse>>>(s_result);
            if (o_data.Status == "0")
            {
                ListResponse<BoxDataResponse> o_boxes_data = new ListResponse<BoxDataResponse>();
                o_boxes_data.Objects = o_data.ResponseData.Objects;// !!!

                //StaticBox.AddInfoObjects(o_boxes_data);
                //В статик бокс закомментируй 9 свойств
                StaticBox.Sensors["Температура"] = o_data.ResponseData.Objects.Where(f => f.SensorName == "Температура").Select(s => s.Value).FirstOrDefault();
                StaticBox.Sensors["Влажность"] = o_data.ResponseData.Objects.Where(f => f.SensorName == "Влажность").Select(s => s.Value).FirstOrDefault();
                StaticBox.Sensors["Освещенность"] = o_data.ResponseData.Objects.Where(f => f.SensorName == "Освещенность").Select(s => s.Value).FirstOrDefault();
                StaticBox.Sensors["Вес груза"] = o_data.ResponseData.Objects.Where(f => f.SensorName == "Вес груза").Select(s => s.Value).FirstOrDefault();
                StaticBox.Sensors["Уровень заряда аккумулятора"] = o_data.ResponseData.Objects.Where(f => f.SensorName == "Уровень заряда аккумулятора").Select(s => s.Value).FirstOrDefault();
                StaticBox.Sensors["Уровень сигнала"] = o_data.ResponseData.Objects.Where(f => f.SensorName == "Уровень сигнала").Select(s => s.Value).FirstOrDefault();
                StaticBox.Sensors["Состояние дверей"] = o_data.ResponseData.Objects.Where(f => f.SensorName == "Состояние дверей").Select(s => s.Value).FirstOrDefault();
                StaticBox.Sensors["Состояние контейнера"] = o_data.ResponseData.Objects.Where(f => f.SensorName == "Состояние контейнера").Select(s => s.Value).FirstOrDefault();
                StaticBox.Sensors["Местоположение контейнера"] = o_data.ResponseData.Objects.Where(f => f.SensorName == "Местоположение контейнера").Select(s => s.Value).FirstOrDefault();
            }
                //Заполняй остальные параметры как в этом примере

                s_weight.Progress = Convert.ToInt32(StaticBox.Sensors["Вес груза"]);
            s_temperature.Progress = Convert.ToInt32(StaticBox.Sensors["Температура"]);
            s_signal_strength_1.Progress = Convert.ToInt32(StaticBox.Sensors["Уровень сигнала"]);
            s_light.Progress = Convert.ToInt32(StaticBox.Sensors["Освещенность"]);
            s_humidity.Progress = Convert.ToInt32(StaticBox.Sensors["Влажность"]);
            s_battery.Progress = Convert.ToInt32(StaticBox.Sensors["Уровень заряда аккумулятора"]);


            //пример чтения данных с файла
            //    string file_data_remember;
            //    using (FileStream file = new FileStream(dir_path + "box_data.txt", FileMode.Open, FileAccess.Read))
            //    {
            //        // преобразуем строку в байты
            //        byte[] array = new byte[file.Length];
            //        // считываем данные
            //        file.Read(array, 0, array.Length);
            //        // декодируем байты в строку
            //        file_data_remember = Encoding.Default.GetString(array);
            //        file.Close();
            //    }

        }

        //FusedLocationProviderClient fusedLocationProviderClient;
        //LocationRequest locationRequest;
        //LocationCallback locationCallback;


        //public void OnMapReady(GoogleMap googleMap)
        //{
        //    _googleMap = googleMap;////11111

        //    MarkerOptions markerOptions = new MarkerOptions();
        //    LatLng location = new LatLng(StaticBox.Latitude, StaticBox.Longitude);
        //    markerOptions.SetPosition(location);
        //    markerOptions.SetTitle("Я здесь");
        //    googleMap.AddMarker(markerOptions);

        //    CameraPosition.Builder builder = CameraPosition.InvokeBuilder();
        //    builder.Target(location);
        //    builder.Zoom(18);
        //    builder.Bearing(0);
        //    builder.Tilt(65);

        //    CameraPosition cameraPosition = builder.Build();
        //    CameraUpdate cameraUpdate = CameraUpdateFactory.NewCameraPosition(cameraPosition);

        //    googleMap.UiSettings.ZoomControlsEnabled = true;
        //    googleMap.UiSettings.CompassEnabled = true;
        //    googleMap.MoveCamera(cameraUpdate);


        //}
        //private void BuildLocationCallBack()
        //{
        //    locationCallback = new DriverLocationCallBack(this);
        //}

        //private void BuildLocationRequest()
        //{
        //    locationRequest = new LocationRequest();
        //    locationRequest.SetPriority(LocationRequest.PriorityBalancedPowerAccuracy);
        //    locationRequest.SetInterval(1000);
        //    locationRequest.SetFastestInterval(3000);
        //    locationRequest.SetSmallestDisplacement(10f);
        //}

        /// <summary>
        /// Подключение к карте api ключ. 
        /// https://console.developers.google.com/apis/credentials?project=geogeometry&hl=RU&supportedpurview=project
        /// информация https://docs.microsoft.com/ru-ru/xamarin/android/platform/maps-and-location/maps/maps-api#google-maps-api-prerequisites
        /// </summary>

        //internal class DriverLocationCallBack : LocationCallback
        //{
        //    private DriverActivity driverActivity;

        //    public DriverLocationCallBack(DriverActivity driverActivity)
        //    {
        //        this.driverActivity = driverActivity;
        //    }

        //    public override async void OnLocationResult(LocationResult result)
        //    {
        //        base.OnLocationResult(result);

        //        try
        //        {
        //            StaticBox.Latitude = result.LastLocation.Latitude;
        //            StaticBox.Longitude = result.LastLocation.Longitude;
        //            StaticBox.Signal = 0;
        //            StaticBox.Date = DateTime.Now;

        //            s_longitude.Text = result.LastLocation.Latitude.ToString();
        //            s_latitude.Text = result.LastLocation.Longitude.ToString();
        //            s_date_time.Text = DateTime.Now.ToString();
        //            s_signal_strength.Progress = 0;

        //            // Получаю информацию о клиенте.
        //            BoxLocation gpsLocation = new BoxLocation
        //            {
        //                id = StaticBox.SmartBoxId,
        //                lat1 = StaticBox.Latitude,
        //                lon1 = StaticBox.Longitude,
        //                signal = StaticBox.Signal,
        //                date = StaticBox.Date
        //            };



        //            var myHttpClient = new HttpClient();
        //            var uri = new Uri("http://iot-tmc-cen.1gb.ru/api/container/setcontainerlocation?id=" + gpsLocation.id + "&lat1=" + gpsLocation.lat1 + "&lon1=" + gpsLocation.lon1 + "&signal=" + gpsLocation.signal + "&date=" + gpsLocation.date);
        //            var uri2 = new Uri("http://81.177.136.11:8003/geo?id=" + gpsLocation.id + "&lat1=" + gpsLocation.lat1 + "&lon1=" + gpsLocation.lon1 + "&signal=" + gpsLocation.signal + "&date=" + gpsLocation.date);
        //            //json структура.
        //            var formContent = new FormUrlEncodedContent(new Dictionary<string, string>
        //    {
        //        { "Id", gpsLocation.id },
        //        { "Lon1", gpsLocation.lon1.ToString()},
        //        { "Lat1", gpsLocation.lat1.ToString()},
        //        { "Signal", "0"},
        //        { "Date", DateTime.Now.ToString()}
        //    });

        //            HttpResponseMessage response = await myHttpClient.PostAsync(uri.ToString(), formContent);// !!!!
        //            HttpResponseMessage responseFromAnotherServer = await myHttpClient.PostAsync(uri2.ToString(), new StringContent(JsonConvert.SerializeObject(gpsLocation), Encoding.UTF8, "application/json"));
        //            AuthApiData<BaseResponseObject> o_data = new AuthApiData<BaseResponseObject>();

        //            string s_result;
        //            using (HttpContent responseContent = response.Content)
        //            {
        //                s_result = await responseContent.ReadAsStringAsync();
        //            }

        //            string s_result_from_another_server;
        //            using (HttpContent responseContent = responseFromAnotherServer.Content)
        //            {
        //                s_result_from_another_server = await responseContent.ReadAsStringAsync();
        //            }

        //            o_data = JsonConvert.DeserializeObject<AuthApiData<BaseResponseObject>>(s_result);
        //        }
        //        catch (Exception ex)
        //        {

        //            throw;
        //        }


        //    }
        //}

        //private void Spinner_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        //{
        //    var spinner = sender as Spinner;
        //    a_situation = spinner.GetItemAtPosition(e.Position).ToString();
        //}

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
        //void ClearField()
        //{
        //    container_name.Text = "";
        //    //s_situation.Text = "";
        //    s_open_close_container.Text = "";
        //    s_lock_unlock_door.Text = "";
        //    s_weight.Text = "";
        //    s_temperature.Text = "";
        //    s_light.Text = "";
        //    s_humidity.Text = "";
        //    s_battery.Text = "";
        //    s_signal_strength.Text = "";
        //    s_longitude.Text = "";
        //    s_latitude.Text = "";
        //    s_date_time.Text = "";
        //}



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
            


