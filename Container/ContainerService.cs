using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using GeoGeometry.Model;
using GeoGeometry.Model.Auth;
using GeoGeometry.Model.Box;
using Newtonsoft.Json;
using Plugin.Settings;

namespace GeoGeometry.Container
{
    public class ContainerService
    {
        const string Url = "http://iot.tmc-centert.ru/api/container/";

        private HttpClient GetClient()
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            return client;
        }
        public async void SaveInfoBox()
        {
            
                var myHttpClient = GetClient();
            var id1 = CrossSettings.Current.GetValueOrDefault("id", "");
            var uri = new Uri("http://iot.tmc-centert.ru/api/container/getbox?id=" + id1);
            string result = await myHttpClient.GetStringAsync(uri.ToString());

            AuthApiData<ListResponse<BoxDataResponse>> o_data = new AuthApiData<ListResponse<BoxDataResponse>>();

            //string s_result;
            //using (HttpContent responseContent = response.Content)
            //{
            //    s_result = await responseContent.ReadAsStringAsync();
            //}

            o_data.ResponseData = new ListResponse<BoxDataResponse>();
                o_data = JsonConvert.DeserializeObject<AuthApiData<ListResponse<BoxDataResponse>>>(result);
            if (o_data.Status == "0")
            {
                ListResponse<BoxDataResponse> o_boxes_data = new ListResponse<BoxDataResponse>();
                o_boxes_data.Objects = o_data.ResponseData.Objects;// !!!

                //StaticBox.AddInfoObjects(o_boxes_data);
                //В статик бокс закомментируй 9 свойств
                StaticBox.Sensors["Температура"] = o_data.ResponseData.Objects.Where(f => f.SensorName == "Температура").Select(s => s.Value).FirstOrDefault();
                StaticBox.Sensors["Влажность"] = o_data.ResponseData.Objects.Where(f => f.SensorName == "Влажность").Select(s => s.Value).FirstOrDefault();
                StaticBox.Sensors["Освещенность"] = o_data.ResponseData.Objects.Where(f => f.SensorName == "Освещенность").Select(s => s.Value).FirstOrDefault();
                StaticBox.Sensors["Вес груза"] = o_data.ResponseData.Objects.Where(f => f.SensorName == "Вес груза").Select(s => s.Value).FirstOrDefault();
                StaticBox.Sensors["Уровень заряда аккумулятора"] = o_data.ResponseData.Objects.Where(f => f.SensorName == "Уровень заряда аккумулятора").Select(s => s.Value).FirstOrDefault();
                StaticBox.Sensors["Уровень сигнала"] = o_data.ResponseData.Objects.Where(f => f.SensorName == "Уровень сигнала").Select(s => s.Value).FirstOrDefault();
                StaticBox.Sensors["Состояние дверей"] = o_data.ResponseData.Objects.Where(f => f.SensorName == "Состояние дверей").Select(s => s.Value).FirstOrDefault();
                StaticBox.Sensors["Состояние контейнера"] = o_data.ResponseData.Objects.Where(f => f.SensorName == "Состояние контейнера").Select(s => s.Value).FirstOrDefault();
                StaticBox.Sensors["Местоположение контейнера"] = o_data.ResponseData.Objects.Where(f => f.SensorName == "Местоположение контейнера").Select(s => s.Value).FirstOrDefault();
            }
        }

        public static async Task<AuthApiData<BaseResponseObject>> EditBox()
        {
            try
            {
                var myHttpClient = new HttpClient();
                
                var date = DateTime.Now;
                var uri = new Uri(Url + "editsensors?id=" + CrossSettings.Current.GetValueOrDefault("id", "") + "&date=" + date + "&sensors[Вес груза]=" + StaticBox.Sensors["Вес груза"]
                + "&sensors[Температура]=" + StaticBox.Sensors["Температура"] + "&sensors[Влажность]=" + StaticBox.Sensors["Влажность"] + "&sensors[Освещенность]=" + StaticBox.Sensors["Освещенность"]
                + "&sensors[Уровень заряда аккумулятора]=" + StaticBox.Sensors["Уровень заряда аккумулятора"] + "&sensors[Уровень сигнала]=" + StaticBox.Sensors["Уровень сигнала"] + "&sensors[Состояние дверей]=" + StaticBox.Sensors["Состояние дверей"]
                + "&sensors[Состояние контейнера]=" + StaticBox.Sensors["Состояние контейнера"] + "&sensors[Местоположение контейнера]=" + StaticBox.Sensors["Местоположение контейнера"]);

                //var uri = new Uri("http://iot.tmc-centert.ru/api/container/editsensors?id=66fbf65c-a9e1-45bb-96df-b63ca7519ce4&date=2020-01-18 19:48:22&sensors[Вес груза]=3455&sensors[Температура]=23&sensors[Влажность]=23&sensors[Освещенность]=123&sensors[Уровень заряда аккумулятора]=34&sensors[Уровень сигнала]=-29&sensors[Местоположение контейнера]=2&sensors[Состояние дверей]=1&sensors[Состояние контейнера]=1);
                //var uri2 = ("http://81.177.136.11:8003/sensor?id=" + container.Id + "&IsOpenedBox=" + container.IsOpenedBox + "&Name=" + container.Name + "&IsOpenedDoor=" + container.IsOpenedDoor + "&Weight=" + container.Weight + "&Light=" + container.Light + "&Code=" + container.Code + "&Temperature=" + container.Temperature + "&Wetness=" + container.Wetness + "&BatteryPower=" + container.BatteryPower + "&BoxState=" + container.BoxState);

                EditBoxViewModel box = new EditBoxViewModel
                {
                    id = CrossSettings.Current.GetValueOrDefault("id", ""),
                    date = date,
                    Sensors = new Dictionary<string, string>
                    {
                        {"Температура",StaticBox.Sensors["Температура"] },
                        {"Влажность",StaticBox.Sensors["Влажность"] },
                        {"Освещенность",StaticBox.Sensors["Освещенность"] },
                        {"Вес груза",StaticBox.Sensors["Вес груза"] },
                        {"Уровень заряда аккумулятора",StaticBox.Sensors["Уровень заряда аккумулятора"]},
                        {"Уровень сигнала",StaticBox.Sensors["Уровень сигнала"]},
                        {"Состояние дверей",StaticBox.Sensors["Состояние дверей"]},
                        {"Состояние контейнера",StaticBox.Sensors["Состояние контейнера"]},
                        {"Местоположение контейнера",StaticBox.Sensors["Местоположение контейнера"]}
                    }
                };
                var data = new StringContent(JsonConvert.SerializeObject(box));
                HttpResponseMessage response = await myHttpClient.PostAsync(uri.ToString(), new StringContent(JsonConvert.SerializeObject(box), Encoding.UTF8, "application/json"));
                //HttpResponseMessage responseFromAnotherServer = await myHttpClient.PutAsync(uri2.ToString(), new StringContent(JsonConvert.SerializeObject(container), Encoding.UTF8, "application/json"));

                AuthApiData<BaseResponseObject> o_data = new AuthApiData<BaseResponseObject>();

                string s_result;
                using (HttpContent responseContent = response.Content)
                {
                    s_result = await responseContent.ReadAsStringAsync();
                }

                //string s_result_from_server;
                //using (HttpContent responseContent = responseFromAnotherServer.Content)
                //{
                //    s_result_from_server = await responseContent.ReadAsStringAsync();
                //}

                o_data = JsonConvert.DeserializeObject<AuthApiData<BaseResponseObject>>(s_result);

                return o_data;
            }
            catch (Exception ex)
            {
                AuthApiData<BaseResponseObject> result = new AuthApiData<BaseResponseObject>();
                result.Status = "1";
                result.Message = ex.Message;
                return result;
            }
        }
    }
}