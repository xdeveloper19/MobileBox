﻿using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Widget;
using Android.Content;
using Com.Karumi.Dexter;
using Android;
using Com.Karumi.Dexter.Listener.Single;
using Com.Karumi.Dexter.Listener;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using GeoGeometry.Model.Auth;
using Com.Karumi.Dexter.Listener.Multi;
using System.Collections.Generic;
using System;
using Android.Support.V4.App;
using Plugin.Settings;
using System.Net;
using System.Net.Http;
using GeoGeometry.Model.User;
using GeoGeometry.Model.Box;
using GeoGeometry.Container;
using System.Threading.Tasks;

namespace GeoGeometry.Activity
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : AppCompatActivity
    {
        
        RelativeLayout main_form;


        /// <summary>
        /// Конпка прехода на форму авторизации.
        /// </summary>
        private Button btn_auth_form;


        private int MY_PERMISSIONS_REQUEST_CAMERA = 100;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            try
            {
                base.OnCreate(savedInstanceState);
                // Set our view from the "main" layout resource
                SetContentView(Resource.Layout.activity_main);
               
                string file_data_remember;

                btn_auth_form = FindViewById<Button>(Resource.Id.btn_auth_form);

                string[] permissions = { Manifest.Permission.AccessFineLocation, Manifest.Permission.WriteExternalStorage, Manifest.Permission.Camera };
                
                Dexter.WithActivity(this).WithPermissions(permissions).WithListener(new CompositeMultiplePermissionsListener(new SamplePermissionListener(this))).Check();

                if (CrossSettings.Current.GetValueOrDefault("id", "") == "")
                {
                    try
                    {
                        var box = GetRandomBox();
                        if (box.Result.Status == "0")
                        {
                            CrossSettings.Current.AddOrUpdateValue("id", box.Result.ResponseData.BoxId);
                            CrossSettings.Current.AddOrUpdateValue("namebox", box.Result.ResponseData.Name);
                        }
                        else
                            Toast.MakeText(this, "" + box.Result.Message, ToastLength.Long).Show();
                    }
                    catch(Exception ex)
                    {
                        Toast.MakeText(this, "" + ex.Message, ToastLength.Long).Show();
                    }
                }

                // Переход к форме авторизация
                btn_auth_form.Click += (s, e) =>
                {
                    Intent authActivity = new Intent(this, typeof(Auth.AuthActivity));
                    StartActivity(authActivity);
                };

            }
            catch (Exception ex)
            {
                Toast.MakeText(this, "" + ex.Message, ToastLength.Long).Show();
            }
        }

        private async Task<AuthApiData<GetBoxIdResponse>> GetRandomBox()
        {
           
                string dir_path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);

                var myHttpClient = new HttpClient();
            WebRequest request = WebRequest.Create("http://iot-tmc-cen.1gb.ru/api/container/getrandombox");
            WebResponse response = request.GetResponse();
            // HttpWebRequest
            var uri = new Uri("http://iot-tmc-cen.1gb.ru/api/container/getrandombox");
            string s_result = "";
            using (Stream stream = response.GetResponseStream())
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    string line = "";
                    while ((line = reader.ReadLine()) != null)
                    {
                        s_result += line;
                    }
                }
            }
            response.Close();

            AuthApiData<GetBoxIdResponse> o_data = JsonConvert.DeserializeObject<AuthApiData<GetBoxIdResponse>>(s_result);
            //ClearField();

            return o_data;
        }

        private class SamplePermissionListener : Java.Lang.Object, IMultiplePermissionsListener
        {
            MainActivity activity;
            public SamplePermissionListener(MainActivity activity)
            {
                this.activity = activity;
            }

            public void OnPermissionDenied(PermissionDeniedResponse p0)
            {
                //Snackbar.Make(activity.main_form, "Permission Denied", Snackbar.LengthShort).Show();
            }

            public void OnPermissionGranted(PermissionGrantedResponse p0)
            {
                //Snackbar.Make(activity.main_form, "Permission Granted", Snackbar.LengthShort).Show();
            }

            public void OnPermissionRationaleShouldBeShown(IList<PermissionRequest> p0, IPermissionToken p1)
            {
                p1.ContinuePermissionRequest();
                throw new System.NotImplementedException();
            }

            public void OnPermissionsChecked(MultiplePermissionsReport p0)
            {
                if (p0.AreAllPermissionsGranted())
                {
                    
                }

                if (p0.IsAnyPermissionPermanentlyDenied)
                {
                    // show alert dialog navigating to Settings
                    
                }
            }
        }
    }

    internal class MyDismissListener : Java.Lang.Object, IDialogInterfaceOnDismissListener
    {
        private IPermissionToken token;

        public MyDismissListener(IPermissionToken token)
        {
            this.token = token;
        }

        public void OnDismiss(IDialogInterface dialog)
        {
            token.CancelPermissionRequest();
        }
    }
}