using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;

using Android.App;
using Android.Content;
using Android.Gms.Location;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using GeoGeometry.Activity.Menu;
using GeoGeometry.Container;
using GeoGeometry.Model.Auth;
using GeoGeometry.Model.GPSLocation;
using Newtonsoft.Json;
using Plugin.Settings;

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


            BuildLocationRequest();
            BuildLocationCallBack();

            fusedLocationProviderClient = LocationServices.GetFusedLocationProviderClient(this);

            
            fusedLocationProviderClient.RequestLocationUpdates(locationRequest,
                locationCallback, Looper.MyLooper());
        }

        public void OnMapReady(GoogleMap googleMap)
        {
            _googleMap = googleMap;////11111

            double latitude = Convert.ToDouble(s_latitude.Text);
            double longitude = Convert.ToDouble(s_longitude.Text);

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
            locationRequest.SetInterval(1000);
            locationRequest.SetFastestInterval(3000);
            locationRequest.SetSmallestDisplacement(10f);
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
                    s_longitude.Text = result.LastLocation.Latitude.ToString();
                    s_latitude.Text = result.LastLocation.Longitude.ToString();
                    s_date_time.Text = DateTime.Now.ToString();

                    // Получаю информацию о клиенте.
                    BoxLocation gpsLocation = new BoxLocation
                    {
                        id = CrossSettings.Current.GetValueOrDefault("id", ""),
                        lat1 = result.LastLocation.Latitude,
                        lon1 = result.LastLocation.Longitude,
                        date = DateTime.Now,
                    };

                    int signal = 0;

                    var myHttpClient = new HttpClient();
                    var uri = new Uri("http://iot-tmc-cen.1gb.ru/api/container/setcontainerlocation?id=" + gpsLocation.id + "&lat1=" + gpsLocation.lat1 + "&lon1=" + gpsLocation.lon1 + "&signal=" + signal + "&date=" + gpsLocation.date);
                    var uri2 = new Uri("http://81.177.136.11:8003/geo?id=" + gpsLocation.id + "&lat1=" + gpsLocation.lat1 + "&lon1=" + gpsLocation.lon1 + "&signal=" + signal + "&date=" + gpsLocation.date);
                    //json структура.
                    var formContent = new FormUrlEncodedContent(new Dictionary<string, string>
                    {
                        { "Id", gpsLocation.id },
                        { "Lon1", gpsLocation.lon1.ToString()},
                        { "Lat1", gpsLocation.lat1.ToString()},
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
    }
}