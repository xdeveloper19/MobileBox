using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using XLabs.Forms.Controls;
using Android.Hardware;// &&&

namespace GeoGeometry.Activity.Cameraa
{
    public class CameraFragment : Fragment
    {
        Android.Hardware.Camera _camera;
        CameraPreview _camPreview;
        FrameLayout _frameLayout;

        bool _cameraReleased = false;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var ignor = base.OnCreateView(inflater, container, savedInstanceState);
            var view = inflater.Inflate(Resource.Layout.CameraFragmentLayout, container, false);
            
       
        var snapButton = view.FindViewById<Button>(Resource.Id.snapButton);
            snapButton.BringToFront();
            snapButton.Click += SnapButtonClick;

            _camera = SetUpCamera();
            _frameLayout = view.FindViewById<FrameLayout>(Resource.Id.camera_preview);
            SetCameraPreview();

            return view;
        }

        /// <summary>
        /// Take a picture.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SnapButtonClick(object sender, EventArgs e)
        {
            try
            {
                _camera.StartPreview();//https://habr.com/ru/post/112272/             
                //_camera.TakePicture(null, null, CameraPictureCallBack(Activity));//&&&
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public override void OnDestroy()
        {
            _camera.StopPreview();
            _camera.Release();
            _cameraReleased = true;
            base.OnDestroy();
        }

        public override void OnResume()
        {
            if (_cameraReleased)
            {
                _camera.Reconnect();
                _camera.StartPreview();
                _cameraReleased = false;
            }
            base.OnResume();
        }

        /// <summary>
        /// Set the Camera Preview, and pass it the current device's Camera
        /// </summary>
        private void SetCameraPreview()
        {
            _frameLayout.AddView(new CameraPreview(Activity, _camera));
        }

        /// <summary>
        /// Get an instace of the current hardware camera of the device
        /// </summary>
        /// <returns></returns>
        Android.Hardware.Camera SetUpCamera()
        {
            Android.Hardware.Camera c = null;
            try
            {
                c = Android.Hardware.Camera.Open();
            }
            catch (Exception e)
            {
                Log.Debug("", "Device camera not available now.");
            }

            return c;
        }
    }
}