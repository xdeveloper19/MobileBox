using System.Collections.Generic;
using System.Linq;
using System.Text;
using MimeKit;
using MailKit.Net.Smtp;
using Android.App;
using Android.Content;
using Android.Hardware;
using Android.Net;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Java.IO;
using System.IO;
using GeoGeometry.Model.User;
using System.Drawing;
using System;
using Android.Provider;

namespace GeoGeometry.Activity.Cameraa
{
    public class CameraPictureCallBack : Java.Lang.Object/* Android.Hardware.Camera.IPictureCallback*/
    {
        const string APP_NAME = "SmartBox";
        Context _context;



        public CameraPictureCallBack(Context cont)//1
        {
            _context = cont;
        }

        /// <summary>
        /// Callback when the picture is taken by the Camera
        /// </summary>
        /// <param name="data"></param>
        /// <param name="camera"></param>
        //protected void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        //{
        //    //base.OnActivityResult(requestCode, resultCode, data);
        //    Bitmap bitmap = null;
        //    //If user did not take a photeo , he will get result of bitmap, it is null
        //    try
        //    {
        //        bitmap = (Bitmap)data.Extras.Get("data");
        //    }
        //    catch (Exception e)
        //    {
        //        Log.Error("TakePhotoDemo1", e.Message);
        //        //Toast.MakeText(this, "You did not take a photo", ToastLength.Short).Show();

        //    }

        //    if (bitmap != null)
        //    {
        //        MediaStore.Images.Media.InsertImage(ContentResolver, bitmap, "screen", "shot");
        //        imageView1.SetImageBitmap(bitmap);
        //    }
        //    else
        //    {
        //        //Toast.MakeText(this, "You did not take a photo", ToastLength.Short).Show();
        //    }

        //}
        //public void OnPictureTaken(byte[] data, Android.Hardware.Camera camera)//2
        //{
        //    try
        //    {      

        //        //var DateGenerated = System.DateTime.Now;
        //        //var message = new MimeMessage();
        //        //message.From.Add(new MailboxAddress("From", "smartbox2019m@mail.ru"));
        //        //message.To.Add(new MailboxAddress("To", "smartbox2019m@mail.ru"));
        //        //if (!string.IsNullOrEmpty(StaticUser.Email))
        //        //{
        //        //    message.Cc.Add(new MailboxAddress("CC", StaticUser.Email));
        //        //}
        //        //message.Subject = "Снимок объекта за "+ DateGenerated.ToString();

        //        //var body = new TextPart("plain")
        //        //{
        //        //    Text = "Снимок объекта подготовлен в " + DateGenerated.ToString()
        //        //};

        //        //var attachment = new MimePart("image", "jpg")
        //        //{
        //        //    Content = new MimeContent(new MemoryStream(data), ContentEncoding.Default),
        //        //    ContentDisposition = new ContentDisposition(ContentDisposition.Attachment),
        //        //    ContentTransferEncoding = ContentEncoding.Base64,
        //        //    FileName = "снимок_объекта_"+ DateGenerated.ToString()+".jpg"

        //        //};

        //        //// now create the multipart/mixed container to hold the message text and the
        //        //// image attachment
        //        //var multipart = new Multipart("mixed");
        //        //multipart.Add(body);
        //        //multipart.Add(attachment);
        //        //message.Body = multipart;

        //        //using (var client = new SmtpClient())
        //        //{
        //        //    // For demo-purposes, accept all SSL certificates (in case the server supports STARTTLS)  
        //        //    client.ServerCertificateValidationCallback = (s, c, h, e) => true;
        //        //    client.Connect("smtp.mail.ru", 587, false);
        //        //    // Note: only needed if the SMTP server requires authentication  
        //        //    client.Authenticate("smartbox2019m@mail.ru", "MKFe5ElR");
        //        //    client.Send(message);
        //        //    client.Disconnect(true);
        //        //}

        //        ////string fileName = Uri.Parse("test.jpg").LastPathSegment;
        //        ////var os = _context.OpenFileOutput(fileName, FileCreationMode.Private);
        //        ////System.IO.BinaryWriter binaryWriter = new System.IO.BinaryWriter(os);
        //        ////binaryWriter.Write(data);
        //        ////binaryWriter.Close();

        //        ////We start the camera preview back since after taking a picture it freezes
        //        //camera.StartPreview();
        //    }
        //    catch (System.Exception e)
        //    {
        //        Log.Debug(APP_NAME, "File not found: " + e.Message);
        //    }
        //}
    }
}