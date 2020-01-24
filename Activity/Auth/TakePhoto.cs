//using Android.Graphics;
//using Android.Provider;
//using Android;
//using Android.Support.V4.App;

//using System;
//using Android.Support.Design.Widget;
//using Android.Views;
//using System.IO;
//using Android.Support.V4.Content;
//using Android.Util;
//using Android.Support.V7.App;
//using Android.Widget;
//using Android.OS;
//using Android.Content.PM;
//using Android.Runtime;
//using Android.Content;
//using Android.App;
//using GeoGeometry.Activity.Cameraa;
//using Java.IO;
//using GeoGeometry.Container;
//using XLabs.Platform.Services.Media;
//using Plugin.Media.Abstractions;
//using Plugin.Media;
//using System.Threading.Tasks;

//namespace GeoGeometry.Activity.Auth
//{
//    [Activity(Label = "TakePhoto")]
//    public class TakePhoto : AppCompatActivity, ActivityCompat.IOnRequestPermissionsResultCallback
//    {
//        static readonly int REQUEST_CAMERA_WriteExternalStorage = 0;

//        protected override void OnCreate(Bundle savedInstanceState)
//        {
          
//            base.OnCreate(savedInstanceState);
//            // Set our view from the "main" layout resource
//            CheckPermission();         
//            //StaticBox.Key["weight"] = "1000";
//            //StaticBox.Key["light"] = "0";

//            //var value1 = StaticBox.Key["weight"];
//            //var value2 = StaticBox.Key["light"];


//            //Intent Driver = new Intent(this, typeof(StaticBoxActivity));
//            //StartActivity(Driver);
//        }


//        public void CheckPermission()
//        {
//            if ((ContextCompat.CheckSelfPermission(this, Manifest.Permission.Camera) == (int)Permission.Granted) && (ContextCompat.CheckSelfPermission(this, Manifest.Permission.WriteExternalStorage) == (int)Permission.Granted))
//            {
//                // Camera and store permission has  been granted
//                ShowCamera();
//            }
//            else
//            {
//                // Camera and store permission has not been granted
//                RequestPermission();

//            }


//        }

//        private void RequestPermission()
//        {

//            ActivityCompat.RequestPermissions(this, new String[] { Manifest.Permission.Camera, Manifest.Permission.WriteExternalStorage }, REQUEST_CAMERA_WriteExternalStorage);

//        }


//        //get result of persmissions
//        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
//        {
//            if (requestCode == REQUEST_CAMERA_WriteExternalStorage)
//            {


//                if (PermissionUtil.VerifyPermissions(grantResults))
//                {
//                    // All required permissions have been granted, display Camera.

//                    ShowCamera();
//                }
//                else
//                {
//                    // permissions did not grant, push up a snackbar to notificate USERS
//                    //Snackbar.Make(layout,"Permissions were not granted", Snackbar.LengthShort).Show();
//                }

//            }
//            else
//            {
//                base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
//            }
//        }

//        private void ShowCamera()
//        {
//            Intent picker = new Intent(MediaStore.ActionImageCapture);
//            DateTime now = DateTime.Now;
//            var intent = picker.(new StoreCameraMediaOptions
//            {
//                Name = "picture_" + now.Day + "_" + now.Month + "_" + now.Year + ".jpg",
//                Directory = null
//            });
//            StartActivityForResult(intent, 0);
//        }
//        void takePicture()
//        {
//            MediaPicker picker = new MediaPicker(this);
//            DateTime now = DateTime.Now;
//            var intent = picker.GetTakePhotoUI(new StoreCameraMediaOptions
//            {
//                Name = "picture_" + now.Day + "_" + now.Month + "_" + now.Year + ".jpg",
//                Directory = null
//            });
//            StartActivityForResult(intent, 1);
//        }

//        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
//        {
//            base.OnActivityResult(requestCode, resultCode, data);

//            if (requestCode == 1)
//            {
//                if (resultCode == Result.Ok)
//                //{
//                //    data.GetMediaFileExtraAsync(this).ContinueWith(t =>
//                //    {
//                //        using (Bitmap bmp = loadAndResizeBitmap(t.Result.Path))
//                //        {
//                //            if (bmp != null)
//                //                _takenPictureImageView.SetImageBitmap(bmp);
//                //        }

//                //    }, TaskScheduler.FromCurrentSynchronizationContext());
//                }
//            }
//        }
//        //protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
//        //{
//        //    base.OnActivityResult(requestCode, resultCode, data);
//        //    Bitmap bitmap = null;

//        //    //If user did not take a photeo , he will get result of bitmap, it is null
//        //    try
//        //    {
//        //        bitmap = (Bitmap)data.Extras.Get("data");
//        //    }
//        //    catch (Exception e)
//        //    {
//        //        Log.Error("TakePhotoDemo1", e.Message);
//        //        Toast.MakeText(this, "You did not take a photo", ToastLength.Short).Show();

//        //    }

//        //    if (bitmap != null)
//        //    {
//        //        MediaStore.Images.Media.InsertImage(ContentResolver, bitmap, "screen", "shot");

//        //        Intent authActivity = new Intent(this, typeof(Auth.SensorsDataActivity));
//        //        //authActivity.PutExtra("Image",bitmap);
//        //        StaticBox.ImageData = bitmap;
//        //        //authActivity.PutExtra("MyPhoto", bitmap);
//        //        StartActivity(authActivity);
//        //    }
//        //    else
//        //    {
//        //        Toast.MakeText(this, "You did not take a photo", ToastLength.Short).Show();
//        //    }

//        //}

//    }
//}