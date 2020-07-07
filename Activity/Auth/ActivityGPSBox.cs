using System;
using System.Collections.Generic;
using System.Net.Http;
using Android.App;
using Android.Gms.Location;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.OS;
using Android.Support.V7.App;
using Android.Widget;
using GeoGeometry.Container;
using GeoGeometry.Model;
using GeoGeometry.Model.Auth;
using GeoGeometry.Model.GPSLocation;
using Newtonsoft.Json;

namespace GeoGeometry.Activity.Auth
{
    [Activity(Label = "ActivityGPSBox")]
    class ActivityGPSBox : AppCompatActivity, IOnMapReadyCallback
    {
        private RelativeLayout GPS;

        GoogleMap _googleMap;

        private static EditText s_longitude;

        private static EditText s_latitude;

        private static EditText s_date_time;


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_GPS);

            GPS = FindViewById<RelativeLayout>(Resource.Id.GPS);
            s_longitude = FindViewById<EditText>(Resource.Id.s_longitude);
            s_latitude = FindViewById<EditText>(Resource.Id.s_latitude);
            s_date_time = FindViewById<EditText>(Resource.Id.s_date_time);
            MapFragment mapFragment = (MapFragment)FragmentManager.FindFragmentById(Resource.Id.fragmentMap);
            mapFragment.GetMapAsync(this);

            s_latitude.Focusable = false;
            s_latitude.LongClickable = false;
            s_longitude.Focusable = false;
            s_longitude.LongClickable = false;
            s_date_time.Focusable = false;
            s_date_time.LongClickable = false;
            //StaticBox.Sensors["Местоположение контейнера"] = "1";
            if (StaticBox.Sensors["Местоположение контейнера"] == "На складе" || StaticBox.Sensors["Местоположение контейнера"] == "У заказчика")
            {
                Android.App.AlertDialog.Builder alert = new Android.App.AlertDialog.Builder(this);
                alert.SetTitle("Внимание !");
                alert.SetMessage("Местоположение контейнера не изменяется, так как он находится на складе или у заказчика");
                alert.SetPositiveButton("Закрыть", (senderAlert, args) =>
                {
                    Toast.MakeText(this, "Предупреждение было закрыто!", ToastLength.Short).Show();
                });
                Dialog dialog = alert.Create();
                dialog.Show();
            }
                BuildLocationRequest();  
                BuildLocationCallBack();

                fusedLocationProviderClient = LocationServices.GetFusedLocationProviderClient(this);

                fusedLocationProviderClient.RequestLocationUpdates(locationRequest,
                    locationCallback, Looper.MyLooper());
            

        }

        public void OnMapReady(GoogleMap googleMap)
        {
            _googleMap = googleMap;////11111

            if (StaticBox.Latitude == 0 || StaticBox.Longitude == 0)
            {
                Android.App.AlertDialog.Builder alert = new Android.App.AlertDialog.Builder(this);
                alert.SetTitle("Внимание !");
                alert.SetMessage("Местоположение контейнера не изменяется, так как на телефоне отключен GPS.\n Включите, пожалуйста, GPS.");
                alert.SetPositiveButton("Закрыть", (senderAlert, args) =>
                {
                    Toast.MakeText(this, "Предупреждение было закрыто!", ToastLength.Short).Show();
                });
                Dialog dialog = alert.Create();
                dialog.Show();
                return;
            }
            double latitude = StaticBox.Latitude;
            double longitude = StaticBox.Longitude;

            MarkerOptions markerOptions = new MarkerOptions();
            LatLng location = new LatLng(latitude, longitude);
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

        FusedLocationProviderClient fusedLocationProviderClient;
        LocationRequest locationRequest;
        LocationCallback locationCallback;
        private void BuildLocationCallBack()
        {
            locationCallback = new AuthLocationCallBack(this);
        }

        private void BuildLocationRequest()
        {
            locationRequest = new LocationRequest();
            locationRequest.SetPriority(LocationRequest.PriorityBalancedPowerAccuracy);
            if (StaticBox.Sensors["Местоположение контейнера"] != "На складе" || StaticBox.Sensors["Местоположение контейнера"] != "У заказчика")
            {
                locationRequest.SetInterval(1000);
                locationRequest.SetFastestInterval(3000);
                locationRequest.SetSmallestDisplacement(10f);
            } 
        }


        internal class AuthLocationCallBack : LocationCallback // !!!!
        {
            private ActivityGPSBox activityUserBoxy;

            public AuthLocationCallBack(ActivityGPSBox activityUserBoxy)
            {
                this.activityUserBoxy = activityUserBoxy;
            }

            public override async void OnLocationResult(LocationResult result)
            {
                base.OnLocationResult(result);

                try
                {
                    if (result == null)
                    {
                        return;
                    }
                    StaticBox.Latitude = result.LastLocation.Latitude;
                    StaticBox.Longitude = result.LastLocation.Longitude;

                    s_longitude.Text = result.LastLocation.Latitude.ToString();
                    s_latitude.Text = result.LastLocation.Longitude.ToString();
                    s_date_time.Text = DateTime.Now.ToString();

                    // Получаю информацию о клиенте.
                    BoxLocation gpsLocation = new BoxLocation
                    {
                        id = StaticBox.DeviceId,
                        lat1 = result.LastLocation.Latitude.ToString().Replace(",","."),
                        lon1 = result.LastLocation.Longitude.ToString().Replace(",", "."),
                        date = DateTime.Now,
                    };

                    int signal = 0;

                    var myHttpClient = new HttpClient();
                   // var uri = new Uri("http://iot-tmc-cen.1gb.ru/api/container/setcontainerlocation?id=" + gpsLocation.id + "&lat1=" + gpsLocation.lat1 + "&lon1=" + gpsLocation.lon1 + "&date=" + gpsLocation.date);
                    var uri2 = new Uri("http://smartboxcity.ru:8003/imitator/geo");


                    //json структура.
                    FormUrlEncodedContent formUrlEncodedContent = new FormUrlEncodedContent(new Dictionary<string, string>
                    {
                        { "Id", gpsLocation.id },
                        { "Lon1", gpsLocation.lon1.ToString().Replace(",",".")},
                        { "Lat1", gpsLocation.lat1.ToString().Replace(",",".")},
                        { "Date", DateTime.Now.ToString()}
                    });
                    var formContent = formUrlEncodedContent;

                   // HttpResponseMessage response = await myHttpClient.PostAsync(uri.ToString(), formContent);// !!!!
                    HttpResponseMessage responseFromAnotherServer = await myHttpClient.PostAsync(uri2.ToString(), formContent);
                    AuthApiData<BaseResponseObject> o_data = new AuthApiData<BaseResponseObject>();

                    //string s_result;
                    //using (HttpContent responseContent = response.Content)
                    //{
                    //    s_result = await responseContent.ReadAsStringAsync();
                    //}

                    string s_result_from_another_server;
                    using (HttpContent responseContent = responseFromAnotherServer.Content)
                    {
                        s_result_from_another_server = await responseContent.ReadAsStringAsync();
                    }

                    if (responseFromAnotherServer.IsSuccessStatusCode)
                    {
                        o_data = JsonConvert.DeserializeObject<AuthApiData<BaseResponseObject>>(s_result_from_another_server);
                        Toast.MakeText(Application.Context, o_data.Message, ToastLength.Short).Show();
                    }
                    else
                    {
                        ErrorResponseObject error = new ErrorResponseObject();
                        error = JsonConvert.DeserializeObject<ErrorResponseObject>(s_result_from_another_server);
                        Toast.MakeText(Application.Context, error.Errors[0], ToastLength.Short).Show();
                    }

                }
                catch (Exception ex)
                {
                    Toast.MakeText(Application.Context, ex.Message, ToastLength.Short).Show();
                }
            }
        }
    }
}