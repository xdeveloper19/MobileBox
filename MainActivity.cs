using Android.App;
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

        /// <summary>
        /// Конпка прехода на форму регистрации.
        /// </summary>
        private Button btn_reg_form;

        private int MY_PERMISSIONS_REQUEST_CAMERA = 100;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            try
            {
                base.OnCreate(savedInstanceState);
                // Set our view from the "main" layout resource
                SetContentView(Resource.Layout.activity_main);
                string dir_path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
                string file_data_remember;

                btn_auth_form = FindViewById<Button>(Resource.Id.btn_auth_form);
                btn_reg_form = FindViewById<Button>(Resource.Id.btn_reg_form);

                

                string[] permissions = { Manifest.Permission.AccessFineLocation, Manifest.Permission.WriteExternalStorage, Manifest.Permission.Camera };
                //ActivityCompat.RequestPermissions(this, new String[] { Manifest.Permission.Camera }, MY_PERMISSIONS_REQUEST_CAMERA);
                Dexter.WithActivity(this).WithPermissions(permissions).WithListener(new CompositeMultiplePermissionsListener(new SamplePermissionListener(this))).Check();



                // Переход к форме регистрации.
                btn_reg_form.Click += (s, e) =>
                {
                    Intent registerActivity = new Intent(this, typeof(Auth.RegisterActivity));
                    StartActivity(registerActivity);
                };

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

            //public void OnPermissionRationaleShouldBeShown(PermissionRequest p0, IPermissionToken token)
            //{
            //    activity.ShowRequestPermissionRationale(token);
            //}

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

                // check for permanent denial of any permission
                if (p0.IsAnyPermissionPermanentlyDenied)
                {
                    // show alert dialog navigating to Settings
                    
                }
            }
        }

        //private void ShowRequestPermissionRationale(IPermissionToken token)
        //{
        //    new Android.Support.V7.App.AlertDialog.Builder(this)
        //        .SetTitle("Разрешения GPS")
        //        .SetMessage("Необходимо разрешить использовать ваши gps данные")
        //        .SetNegativeButton("Отмена", delegate
        //        {
        //            token.ContinuePermissionRequest();
        //        })
        //        .SetPositiveButton("Ok", delegate
        //        {
        //            token.ContinuePermissionRequest();
        //        })
        //        .SetOnDismissListener(new MyDismissListener(token)).Show();
        //}
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