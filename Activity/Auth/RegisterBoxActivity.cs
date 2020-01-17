using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GeoGeometry.Model.Auth;
using Android.App;
using Android.Content;
using Android.OS;
using System.Text.Json;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using System.Net.Http;
using Newtonsoft.Json;
using System.Net;
using GeoGeometry.Container;
using System.IO;
using GeoGeometry.Model.User;
using GeoGeometry.Model.Box;
using GeoGeometry.Model;

namespace GeoGeometry.Activity.Auth
{
    [Activity(Label = "RegisterBoxActivity")]
    public class RegisterBoxActivity: AppCompatActivity
    {

        /// <summary>
        /// Имя клиента.
        /// </summary>
        private EditText box_name;

        /// <summary>
        /// Конпка регистрации.
        /// </summary>
        private Button btn_registerbox;

        /// <summary>
        /// Конпка назад.
        /// </summary>
        private ImageButton btn_back_a;

        /// <summary>
        /// Для прокрутки страницы.
        /// </summary>
        private ProgressBar preloader;


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.activity_registerbox);


            box_name = FindViewById<EditText>(Resource.Id.box_name);
            btn_registerbox = FindViewById<Button>(Resource.Id.btn_registerbox);
            preloader = FindViewById<ProgressBar>(Resource.Id.preloader);
            btn_back_a = FindViewById<ImageButton>(Resource.Id.btn_back_a);

            btn_back_a.Click += (s, e) =>
            {
                Finish();
            };

            string dir_path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            string file_data_remember;
            

            btn_registerbox.Click += async delegate
            {
                try
                {
                    preloader.Visibility = Android.Views.ViewStates.Visible;

                    RegisterBoxModel register = new RegisterBoxModel
                    {
                        Name = box_name.Text,
                    };
                    var myHttpClient = new HttpClient();

                    var uri = new Uri("http://iot-tmc-cen.1gb.ru/api/container/create?name=" + register.Name);

                    //GetAsync
                    HttpResponseMessage response = await myHttpClient.PostAsync(uri.ToString(), new StringContent(JsonConvert.SerializeObject(register.Name), Encoding.UTF8, "application/json"));
                    
                    string s_result;
                    using (HttpContent responseContent = response.Content)
                    {
                        s_result = await responseContent.ReadAsStringAsync();
                    }
                    AuthApiData<ContainerResponse> o_data = new AuthApiData<ContainerResponse>();
                    o_data = JsonConvert.DeserializeObject<AuthApiData<ContainerResponse>>(s_result);
                    //AuthApiData <ListResponse< ContainerResponse >> o_data 
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        if (o_data.Status == "0")
                        {
                            Toast.MakeText(this, o_data.Message, ToastLength.Long).Show();

                            //ListReponse<T>
                            ContainerResponse o_box_data = new ContainerResponse();
                            o_box_data = o_data.ResponseData;
                            
                            //StaticBox.AddInfoAuth(o_box_data);
                            

                            //using (FileStream file = new FileStream(dir_path + "box_data.txt", FileMode.OpenOrCreate))
                            //{
                            //    // преобразуем строку в байты
                            //    byte[] array = Encoding.Default.GetBytes("0" + JsonConvert.SerializeObject(o_box_data));
                            //    // запись массива байтов в файл
                            //    file.Write(array, 0, array.Length);
                            //}
                            //using (FileStream fs = new FileStream(dir_path + "box_data.txt", FileMode.OpenOrCreate))
                            //{
                            //    await System.Text.Json.JsonSerializer.SerializeAsync<ContainerResponse>(fs,o_box_data);
                            //}

                            using (FileStream file = new FileStream(dir_path + "box_data.txt", FileMode.OpenOrCreate, FileAccess.Write))
                            {
                                // преобразуем строку в байты
                                byte[] array = Encoding.Default.GetBytes(JsonConvert.SerializeObject(o_box_data));
                                // запись массива байтов в файл
                                file.Write(array, 0, array.Length);
                            }
                            //using (FileStream fs = new FileStream(dir_path + "box_data.txt", FileMode.OpenOrCreate))
                            //{
                            //    ContainerResponse container = await System.Text.Json.JsonSerializer.DeserializeAsync<ContainerResponse>(fs);
                            //    var name = container.Name;
                            //}

                            preloader.Visibility = Android.Views.ViewStates.Invisible;

                            // Переход на страницу водителя.
                            Intent Driver = new Intent(this, typeof(Auth.DriverActivity));
                            Driver.PutExtra("idAction", "2");
                            StartActivity(Driver);
                        }
                        else
                        {
                            Toast.MakeText(this, o_data.Message, ToastLength.Long).Show();
                        }
                    }
                    ClearField();
                }
                catch (Exception ex)
                {
                    Toast.MakeText(this, "" + ex.Message, ToastLength.Long).Show();
                }
            };

            /// <summary>
            /// Метод очистки полей.
            /// </summary>
            void ClearField()
            {
                box_name.Text = "";
            }
        }
    }

}