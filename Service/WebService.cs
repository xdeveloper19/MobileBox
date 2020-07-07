using System;
using System.Net.Http;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Support.V4.App;
using Android.Util;
using Android.Widget;
using Firebase.JobDispatcher;
using GeoGeometry.Activity.Auth;
using GeoGeometry.Container;
using GeoGeometry.Model.Auth;
using Newtonsoft.Json;
using static Android.Media.Audiofx.BassBoost;

namespace GeoGeometry.Service
{
    [Service(Name = "com.xamarin.fjdtestapp.DemoJob")]
    [IntentFilter(new[] { FirebaseJobServiceIntent.Action })]
    public class WebService : JobService
    {
        static readonly string TAG = "X:DemoService";

        public override void OnCreate()
        {
            base.OnCreate();
        }

       
        public override bool OnStartJob(IJobParameters jobParameters)
        {
            Task.Run(async () => 
            {
                await SearchPhotoRequest();

               
               if (StaticBox.IsAvalableRequest == "0")
               {
                    // Create PendingIntent
                    StaticBox.CameraOpenOrNo = 1;
                    StaticBox.IsAvalableRequest = "1";
                    Intent camera = new Intent(this, typeof(SensorsDataActivity));

                    PendingIntent resultPendingIntent = PendingIntent.GetActivity(this, 0, camera,
                    PendingIntentFlags.UpdateCurrent);

                    //Create Notification
                    NotificationCompat.Builder builder = new NotificationCompat.Builder(Application.Context, "Channel")
                        .SetSmallIcon(Resource.Mipmap.ic_launcher)
                    .SetContentTitle("Запрос клиента на получение фото.")
                    .SetContentText("Сделать фото")
                    .SetAutoCancel(true)
                    .SetVibrate(new long[] { 1000, 1000 })
                    .SetContentIntent(resultPendingIntent);

                    //Show Notification
                    Notification notification = builder.Build();
                    NotificationManager notificationManager = Application.Context.GetSystemService(Context.NotificationService) as NotificationManager;
                    notificationManager.Notify(1, notification);
                }
                JobFinished(jobParameters, true);
            });

            // Return true because of the asynchronous work
            return true;
        }

        private async Task SearchPhotoRequest()
        {
            try
            {
                var myHttpClient = new HttpClient();

                var uri = new Uri("http://iot.tmc-centert.ru/api/container/SearchCommandPhoto?name=" + StaticBox.DeviceId);
                HttpResponseMessage response = await myHttpClient.GetAsync(uri.ToString());

                string s_result;
                using (HttpContent responseContent = response.Content)
                {
                    s_result = await responseContent.ReadAsStringAsync();
                }

                AuthApiData<BaseResponseObject> o_data = new AuthApiData<BaseResponseObject>();
                o_data = JsonConvert.DeserializeObject<AuthApiData<BaseResponseObject>>(s_result);
                StaticBox.IsAvalableRequest = o_data.Status;
            }
            catch (Exception ex)
            {
                Log.Debug(TAG, ex.Message);
                Toast.MakeText(Application.Context, TAG + ex.Message, ToastLength.Short).Show();
            }

        }

        public override bool OnStopJob(IJobParameters jobParameters)
        {
            Log.Debug(TAG, "DemoJob::OnStartJob");
            // nothing to do.
            return true;
        }
    }
}