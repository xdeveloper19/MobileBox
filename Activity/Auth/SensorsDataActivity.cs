using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
using Android.Media;
using Android.OS;
using Android.Provider;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Support.V4.Content;
using Android.Support.V7.App;
using Android.Util;
using Android.Views;
using Android.Widget;
using GeoGeometry.Activity.Cameraa;
using GeoGeometry.Container;
using GeoGeometry.Model.Box;
//using Java.IO;
using Java.Lang;
using Newtonsoft.Json;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Plugin.Settings;
using Xamarin.Forms;
using XLabs.Platform.Services.Media;
using static Android.Content.ClipData;
using static Android.Provider.MediaStore;

namespace GeoGeometry.Activity.Auth
{
    [Activity(Label = "SensorsDataActivity")]
    public class SensorsDataActivity: AppCompatActivity, ActivityCompat.IOnRequestPermissionsResultCallback
    {
        static readonly int REQUEST_CAMERA_WriteExternalStorage = 0;

        static string filepath = "/storage/emulated/0/Pictures/";

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

            if(StaticBox.CameraOpenOrNo == 1)
            {
                CheckPermission();
            }

        }

        

        Android.Graphics.Bitmap loadAndResizeBitmap(string filePath)
        {
            BitmapFactory.Options options = new BitmapFactory.Options { InJustDecodeBounds = true };
            BitmapFactory.DecodeFile(filePath, options);
            
            int REQUIRED_SIZE = 100;
            int width_tmp = options.OutWidth, height_tmp = options.OutHeight;
            int scale = 4;
            while (true)
            {
                if (width_tmp / 2 < REQUIRED_SIZE || height_tmp / 2 < REQUIRED_SIZE)
                    break;
                width_tmp /= 2;
                height_tmp /= 2;
                scale++;
            }

            options.InSampleSize = scale;
            options.InJustDecodeBounds = false;
            //failed
            Android.Graphics.Bitmap resizedBitmap = BitmapFactory.DecodeFile(filePath, options);

            ExifInterface exif = null;
            try
            {
                //failed
                exif = new ExifInterface(filePath);
                string orientation = exif.GetAttribute(ExifInterface.TagOrientation);

                Matrix matrix = new Matrix();
                switch (orientation)
                {
                    case "1": // landscape
                        break;
                    case "3":
                        matrix.PreRotate(180);
                        resizedBitmap = Android.Graphics.Bitmap.CreateBitmap(resizedBitmap, 0, 0, resizedBitmap.Width, resizedBitmap.Height, matrix, false);
                        matrix.Dispose();
                        matrix = null;
                        break;
                    case "4":
                        matrix.PreRotate(180);
                        resizedBitmap = Android.Graphics.Bitmap.CreateBitmap(resizedBitmap, 0, 0, resizedBitmap.Width, resizedBitmap.Height, matrix, false);
                        matrix.Dispose();
                        matrix = null;
                        break;
                    case "5":
                        matrix.PreRotate(90);
                        resizedBitmap = Android.Graphics.Bitmap.CreateBitmap(resizedBitmap, 0, 0, resizedBitmap.Width, resizedBitmap.Height, matrix, false);
                        matrix.Dispose();
                        matrix = null;
                        break;
                    case "6": // portrait
                        matrix.PreRotate(90);
                        resizedBitmap = Android.Graphics.Bitmap.CreateBitmap(resizedBitmap, 0, 0, resizedBitmap.Width, resizedBitmap.Height, matrix, false);
                        matrix.Dispose();
                        matrix = null;
                        break;
                    case "7":
                        matrix.PreRotate(-90);
                        resizedBitmap = Android.Graphics.Bitmap.CreateBitmap(resizedBitmap, 0, 0, resizedBitmap.Width, resizedBitmap.Height, matrix, false);
                        matrix.Dispose();
                        matrix = null;
                        break;
                    case "8":
                        matrix.PreRotate(-90);
                        resizedBitmap = Android.Graphics.Bitmap.CreateBitmap(resizedBitmap, 0, 0, resizedBitmap.Width, resizedBitmap.Height, matrix, false);
                        matrix.Dispose();
                        matrix = null;
                        break;
                }

                return resizedBitmap;
            }

            catch (IOException ex)
            {
                Console.WriteLine("An exception was thrown when reading exif from media file...:" + ex.Message);
                return null;
            }
        }

        public void CheckPermission()
        {
            if ((ContextCompat.CheckSelfPermission(this, Manifest.Permission.Camera) == (int)Permission.Granted) && (ContextCompat.CheckSelfPermission(this, Manifest.Permission.WriteExternalStorage) == (int)Permission.Granted))
            {
                // Camera and store permission has  been granted
                takePicture();
            }
            else
            {
                // Camera and store permission has not been granted
                RequestPermission();

            }


        }

        private void RequestPermission()
        {

            ActivityCompat.RequestPermissions(this, new System.String[] { Manifest.Permission.Camera, Manifest.Permission.WriteExternalStorage }, REQUEST_CAMERA_WriteExternalStorage);

        }


        //get result of persmissions
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        {
            if (requestCode == REQUEST_CAMERA_WriteExternalStorage)
            {


                if (PermissionUtil.VerifyPermissions(grantResults))
                {
                    // All required permissions have been granted, display Camera.

                    takePicture();

                }
                else
                {
                    // permissions did not grant, push up a snackbar to notificate USERS
                    //Snackbar.Make(layout,"Permissions were not granted", Snackbar.LengthShort).Show();
                }

            }
            else
            {
                base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            }
        }

        //private void ShowCamera()
        //{
        //    Intent picker = new Intent(MediaStore.ActionImageCapture);
        //    DateTime now = DateTime.Now;

        //    var intent = picker.(new StoreCameraMediaOptions
        //    {
        //        Name = "picture_" + now.Day + "_" + now.Month + "_" + now.Year + ".jpg",
        //        Directory = null
        //    });
        //    StartActivityForResult(intent, 0);

        //}
        void takePicture()
        {
           
            Intent intent = new Intent(MediaStore.ActionImageCapture);
            StartActivityForResult(intent, 1);
        }

        //protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        //{
        //    base.OnActivityResult(requestCode, resultCode, data);

        //    if (requestCode == 1)
        //    {
        //        if (resultCode == Result.Ok)
        //        {
        //            data.GetMediaFileExtraAsync(this).ContinueWith(t =>
        //            {
        //                using (Android.Graphics.Bitmap bmp = loadAndResizeBitmap(t.Result.Path))
        //                {
        //                    StaticBox.CameraOpenOrNo = 0;
        //                    if (bmp != null)
        //                        photobox.SetImageBitmap(bmp);
        //                }

        //            }, TaskScheduler.FromCurrentSynchronizationContext());
        //        }
        //    }
        //}
        protected override async void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            Android.Graphics.Bitmap bitmap = null;
            StaticBox.CameraOpenOrNo = 0;
            //If user did not take a photeo , he will get result of bitmap, it is null
            try
            {
                bitmap = (Android.Graphics.Bitmap)data.Extras.Get("data");
                                
            }
            catch (System.Exception e)
            {
                Log.Error("TakePhotoDemo1", e.Message);
                Toast.MakeText(this, "Не удалось загрузить фото", ToastLength.Short).Show();
            }

            if (bitmap != null)
            {
                byte[] bitmapData;
                var stream = new MemoryStream();
                bitmap.Compress(Android.Graphics.Bitmap.CompressFormat.Png, 0, stream);
                bitmapData = stream.ToArray();
                var fileContent = new ByteArrayContent(bitmapData);
                fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("application/octet-stream");
                fileContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
                {
                    Name = "file",
                    FileName = "my_uploaded_image.jpg"
                };
                string boundary = "---8d0f01e6b3b5dafaaadaad";
                MultipartFormDataContent multipartContent = new MultipartFormDataContent(boundary);
                multipartContent.Add(fileContent);
                HttpClient httpClient = new HttpClient();
                var uri = new Uri("http://smartboxcity.ru:8003/media?file=" + bitmap);
                HttpResponseMessage response = await httpClient.PostAsync(uri.ToString(), multipartContent);
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                }
                //var myHttpClient = new HttpClient();
                //
                //HttpResponseMessage responseFromAnotherServer = await myHttpClient.PostAsync(uri.ToString(), bitmap);
                //string s_result_from_another_server;
                //using (HttpContent responseContent = responseFromAnotherServer.Content)
                //{
                //    s_result_from_another_server = await responseContent.ReadAsStringAsync();
                //}
                var basePath = Android.App.Application.Context.GetExternalFilesDir(null).AbsolutePath;
                var path = MediaStore.Images.Media.InsertImage(ContentResolver, bitmap, "screen", "shot");
                if(path != null)
                {
                    var uriPath = Android.Net.Uri.Parse(path);
                    var realPath = GetRealPathFromURI(uriPath);
                    bitmap = loadAndResizeBitmap(realPath);
                    photobox.SetImageBitmap(bitmap);
                }
                else
                {
                    Toast.MakeText(this, "Не удалось загрузить фото. Скорее всего, нет разрешеия на доступ к галерее", ToastLength.Short).Show();
                }
               
            }
            else
            {
                Toast.MakeText(this, "Не удалось загрузить фото", ToastLength.Short).Show();
            }

        }
        private string GetRealPathFromURI(Android.Net.Uri uri)
        {
            string doc_id = "";
            using (var c1 = ContentResolver.Query(uri, null, null, null, null))
            {
                c1.MoveToFirst();
                string document_id = c1.GetString(0);
                doc_id = document_id.Substring(document_id.LastIndexOf(":") + 1);
            }

            string path = null;

            // The projection contains the columns we want to return in our query.
            string selection = Android.Provider.MediaStore.Images.Media.InterfaceConsts.Id + " =? ";
            using (var cursor = ContentResolver.Query(Android.Provider.MediaStore.Images.Media.ExternalContentUri, null, selection, new string[] { doc_id }, null))
            {
                if (cursor == null) return path;
                var columnIndex = cursor.GetColumnIndexOrThrow(Android.Provider.MediaStore.Images.Media.InterfaceConsts.Data);
                cursor.MoveToFirst();
                path = cursor.GetString(columnIndex);
            }
            return path;
        }
    }
}