using System;
using System.Collections.Generic;
using System.Net.Http;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using GeoGeometry.Model.Auth;
using System.Net;
using Android.Support.V7.App;
using Newtonsoft.Json;
using System.IO;
using System.Text;
using GeoGeometry.Model.User;

namespace GeoGeometry.Activity.Auth
{
    [Activity(Label = "RegisterActivity")]
    public class RegisterActivity : AppCompatActivity
    {
        /// <summary>
        /// Имя клиента.
        /// </summary>
        private EditText s_first_name;

        /// <summary>
        /// Фамилия клиента.
        /// </summary>
        private EditText s_last_name;

        /// <summary>
        /// Почта клиента
        /// </summary>
        private EditText s_email;

        /// <summary>
        /// Пароль клиента.
        /// </summary>
        private EditText s_pass;

        /// <summary>
        /// Подтвержденный пароль клиента.
        /// </summary>
        private EditText s_pass_check;
        
        /// <summary>
        /// Конпка регистрации.
        /// </summary>
        private Button btn_register;

        /// <summary>
        /// Конпка назад.
        /// </summary>
        private ImageButton btn_back_a;

		private ProgressBar preloader;

        private string s_section_role;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.activity_register);

            btn_register = FindViewById<Button>(Resource.Id.btn_register);
            s_first_name = FindViewById<EditText>(Resource.Id.s_first_name);
            s_last_name = FindViewById<EditText>(Resource.Id.s_last_name);
            s_pass = FindViewById<EditText>(Resource.Id.s_pass);
            s_pass_check = FindViewById<EditText>(Resource.Id.s_pass_check);
            s_email = FindViewById<EditText>(Resource.Id.s_email);

            preloader = FindViewById<ProgressBar>(Resource.Id.loader);

			btn_back_a = FindViewById<ImageButton>(Resource.Id.btn_back_a);

            btn_back_a.Click += (s, e) =>
            {
                Finish();
            };

            string dir_path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);

            btn_register.Click += async delegate
            {
                try
                {
                    preloader.Visibility = Android.Views.ViewStates.Visible;

                    RegisterModel register = new RegisterModel
                    {
                        FirstName = s_first_name.Text,
                        LastName = s_last_name.Text,
                        Email = s_email.Text,
                        Password = s_pass.Text,
                        PasswordConfirm = s_pass_check.Text,
                        RoleName = "user",
                    };
                    
                    var myHttpClient = new HttpClient();
                    var uri = new Uri(" http://iot-tmc-cen.1gb.ru/api/auth/register?email=" + register.Email + "&firstname=" + register.FirstName + "&lastname=" + register.LastName + "&password=" + register.Password + "&passwordconfirm=" + register.PasswordConfirm + "&rolename=" + register.RoleName);                  
                    var _authHeader = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes(string.Format("{0}:{1}", register.FirstName, register.LastName, register.Password, register.PasswordConfirm, register.Email, register.RoleName))));


                HttpResponseMessage response = await myHttpClient.PostAsync(uri.ToString(), new StringContent(JsonConvert.SerializeObject(register), Encoding.UTF8, "application/json"));

                string s_result;
                using (HttpContent responseContent = response.Content)
                {
                    s_result = await responseContent.ReadAsStringAsync();
                }

                AuthApiData<AuthResponseData> o_data = JsonConvert.DeserializeObject<AuthApiData<AuthResponseData>>(s_result);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    if (o_data.Status == "0")
                    {
                        Toast.MakeText(this, o_data.Message, ToastLength.Long).Show();

                        AuthResponseData o_user_data = new AuthResponseData();
                        o_user_data = o_data.ResponseData;

                        StaticUser.AddInfoAuth(o_user_data);

                        using (FileStream file = new FileStream(dir_path + "user_data.txt", FileMode.OpenOrCreate, FileAccess.Write))
                        {
                            // преобразуем строку в байты
                            byte[] array = Encoding.Default.GetBytes(JsonConvert.SerializeObject(o_user_data));
                            // запись массива байтов в файл
                            file.Write(array, 0, array.Length);
                        }

                        
                            Intent Driver = new Intent(this, typeof(Auth.SensorParameters));
                            StartActivity(Driver);
                            this.Finish();

                            preloader.Visibility = Android.Views.ViewStates.Invisible;
                    }
                    else
                    {
                        Toast.MakeText(this, o_data.Message, ToastLength.Long).Show();
                    }
                }
            }
                catch (Exception ex)
                {
                    Toast.MakeText(this, "" + ex.Message, ToastLength.Long).Show();
                }
            };

        }  
    }
}