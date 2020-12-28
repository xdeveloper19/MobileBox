using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
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
            CreateBoxModel model = new CreateBoxModel
            {
                id = StaticBox.DeviceId
            };

            var myHttpClient = GetClient();
            var id1 = StaticBox.DeviceId;
            //var id1 = CrossSettings.Current.GetValueOrDefault("id", "");
           // var uri = new Uri("http://iot.tmc-centert.ru/api/container/getbox?id=" + id1);
            var uri2 = new Uri("http://smartboxcity.ru:8003/imitator/status?id=" + StaticBox.DeviceId);
            HttpResponseMessage response = await myHttpClient.PostAsync(uri2.ToString(), new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json"));

            string s_result;
            using (HttpContent responseContent = response.Content)
            {
                s_result = await responseContent.ReadAsStringAsync();
            }
            AuthApiData<ListResponse<BoxDataResponse>> o_data = new AuthApiData<ListResponse<BoxDataResponse>>();

            //string result = await myHttpClient.GetStringAsync(uri.ToString());

            //AuthApiData<ListResponse<BoxDataResponse>> o_data = new AuthApiData<ListResponse<BoxDataResponse>>();

            //string s_result;
            //using (HttpContent responseContent = response.Content)
            //{
            //    s_result = await responseContent.ReadAsStringAsync();
            //}

            o_data.ResponseData = new ListResponse<BoxDataResponse>();
                o_data = JsonConvert.DeserializeObject<AuthApiData<ListResponse<BoxDataResponse>>>(s_result);
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

                EditBoxViewModel ForAnotherServer = new EditBoxViewModel
                {
                    id = StaticBox.DeviceId,
                    
                    Sensors = new Dictionary<string, string>{ 
                        ["Вес груза"] = StaticBox.Sensors["Вес груза"],
                        ["Температура"] = StaticBox.Sensors["Температура"],
                        ["Влажность"] = StaticBox.Sensors["Влажность"],
                        ["Освещенность"] = StaticBox.Sensors["Освещенность"],
                        ["Уровень заряда аккумулятора"] = StaticBox.Sensors["Уровень заряда аккумулятора"],
                        ["Уровень сигнала"] = StaticBox.Sensors["Уровень сигнала"],
                        ["Состояние дверей"] = StaticBox.Sensors["Состояние дверей"],
                        ["Состояние контейнера"] = StaticBox.Sensors["Состояние контейнера"],
                        ["Местоположение контейнера"] = StaticBox.Sensors["Местоположение контейнера"]
                    },
                };


                var date = DateTime.Now;
                

                var uri = new Uri("http://iot.tmc-centert.ru/api/container/editsensors?date=" + date + "&id=" + CrossSettings.Current.GetValueOrDefault("id", "") + "&sensors[Вес груза]=" + StaticBox.Sensors["Вес груза"]
                + "&sensors[Температура]=" + StaticBox.Sensors["Температура"] + "&sensors[Влажность]=" + StaticBox.Sensors["Влажность"] + "&sensors[Освещенность]=" + StaticBox.Sensors["Освещенность"]
                + "&sensors[Уровень заряда аккумулятора]=" + StaticBox.Sensors["Уровень заряда аккумулятора"] + "&sensors[Уровень сигнала]=" + StaticBox.Sensors["Уровень сигнала"] + "&sensors[Состояние дверей]=" + StaticBox.Sensors["Состояние дверей"]
                + "&sensors[Состояние контейнера]=" + StaticBox.Sensors["Состояние контейнера"] + "&sensors[Местоположение контейнера]=" + StaticBox.Sensors["Местоположение контейнера"]);

                var uri2 = new Uri("http://smartboxcity.ru:8003/imitator/sensors?" + "id=" + StaticBox.DeviceId + "&sensors[Вес груза]=" + StaticBox.Sensors["Вес груза"]
                + "&sensors[Температура]=" + StaticBox.Sensors["Температура"] + "&sensors[Влажность]=" + StaticBox.Sensors["Влажность"] + "&sensors[Освещенность]=" + StaticBox.Sensors["Освещенность"]
                + "&sensors[Уровень заряда аккумулятора]=" + StaticBox.Sensors["Уровень заряда аккумулятора"] + "&sensors[Уровень сигнала]=" + StaticBox.Sensors["Уровень сигнала"] + "&sensors[Состояние дверей]=" + StaticBox.Sensors["Состояние дверей"]
                + "&sensors[Состояние контейнера]=" + StaticBox.Sensors["Состояние контейнера"] + "&sensors[Местоположение контейнера]=" + StaticBox.Sensors["Местоположение контейнера"]);
                

                var formContent = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                { "id", CrossSettings.Current.GetValueOrDefault("id", "") },
                { "date", date.ToString()},
                { "sensors[Вес груза]", StaticBox.Sensors["Вес груза"]},
                { "sensors[Температура]", StaticBox.Sensors["Температура"]},
                { "sensors[Влажность]", StaticBox.Sensors["Влажность"]},
                { "sensors[Освещенность]", StaticBox.Sensors["Освещенность"]},
                { "sensors[Уровень заряда аккумулятора]", StaticBox.Sensors["Уровень заряда аккумулятора"]},
                { "sensors[Уровень сигнала]", StaticBox.Sensors["Уровень сигнала"]},
                { "sensors[Состояние дверей]", StaticBox.Sensors["Состояние дверей"]},
                { "sensors[Состояние контейнера]", StaticBox.Sensors["Состояние контейнера"]},
                { "sensors[Местоположение контейнера]", StaticBox.Sensors["Местоположение контейнера"]}
            });

                //EditBoxViewModel box = new EditBoxViewModel
                //{
                //    id = CrossSettings.Current.GetValueOrDefault("id", ""),
                //    date = date,
                //    Sensors = new Dictionary<string, string>
                //    {
                //        {"Температура",StaticBox.Sensors["Температура"] },
                //        {"Влажность",StaticBox.Sensors["Влажность"] },
                //        {"Освещенность",StaticBox.Sensors["Освещенность"] },
                //        {"Вес груза",StaticBox.Sensors["Вес груза"] },
                //        {"Уровень заряда аккумулятора",StaticBox.Sensors["Уровень заряда аккумулятора"]},
                //        {"Уровень сигнала",StaticBox.Sensors["Уровень сигнала"]},
                //        {"Состояние дверей",StaticBox.Sensors["Состояние дверей"]},
                //        {"Состояние контейнера",StaticBox.Sensors["Состояние контейнера"]},
                //        {"Местоположение контейнера",StaticBox.Sensors["Местоположение контейнера"]}
                //    }
                //};

                //var data = new StringContent(JsonConvert.SerializeObject(box));
                //HttpResponseMessage response = await myHttpClient.PostAsync(uri.ToString(), formContent);
                HttpResponseMessage responseFromAnotherServer = await myHttpClient.PostAsync(uri2.ToString(), new StringContent(JsonConvert.SerializeObject(ForAnotherServer), Encoding.UTF8, "application/json"));

                AuthApiData<BaseResponseObject> o_data = new AuthApiData<BaseResponseObject>();

                //string s_result;
                //using (HttpContent responseContent = response.Content)
                //{
                //    s_result = await responseContent.ReadAsStringAsync();
                //}

                string s_result_from_server;
                using (HttpContent responseContent = responseFromAnotherServer.Content)
                {
                    s_result_from_server = await responseContent.ReadAsStringAsync();
                }

                if (responseFromAnotherServer.IsSuccessStatusCode)
                {
                    StaticBox.CreatedAtSensors = date;
                    o_data = JsonConvert.DeserializeObject<AuthApiData<BaseResponseObject>>(s_result_from_server);
                }
                else
                {
                    ErrorResponseObject error = new ErrorResponseObject();
                    error = JsonConvert.DeserializeObject<ErrorResponseObject>(s_result_from_server);
                    o_data.Message = error.Errors[0];
                    o_data.Status = responseFromAnotherServer.StatusCode.ToString();
                    
                }

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