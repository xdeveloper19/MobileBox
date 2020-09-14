using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mqtt;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace GeoGeometry.Service
{
    public class MQTTService
    {
        IMqttClient _client;
        Context _context;
        private const string MQTT_TAG = "MQTT broker";

        public MQTTService(MQTTClient client, Context context)
        {
            this._context = context;
            StartAsync(client);
        }

        private async void StartAsync(MQTTClient client)
        {
            bool result = await Start(client);
            CreateAlertResult();
        }


        /// <summary>
        /// Запуск задачи соединения с облаком
        /// </summary>
        /// <param name="connection_string"></param>
        /// <param name="client_id"></param>
        private async Task<bool> Start(MQTTClient client)
        {
            return await Task.Run(() =>
            {
                try
                {
                    var configuration = new MqttConfiguration
                    {
                        //BufferSize = 128 * 1024,
                        Port = client.Port
                        //KeepAliveSecs = 10,
                        //WaitTimeoutSecs = 12,
                        //MaximumQualityOfService = MqttQualityOfService.AtMostOnce
                        //AllowWildcardsInTopicFilters = true
                    };

                    //System.Net.Sockets.Socket.
                    CloudConnectionResult.Message = "В ожидании соединения";
                    _client = MqttClient.CreateAsync(client.Server, configuration).Result;
                    _client.Disconnected += _client_Disconnected;
                    var sessionState = _client.ConnectAsync(new MqttClientCredentials(clientId: client.ClientId, userName: client.UserName, password: client.Password), cleanSession: false).Result;
                    CloudConnectionResult.Message = "Соединение установлено";
                    return true;
                }
                catch (System.Exception ex)
                {
                    CloudConnectionResult.Message = ex.Message;
                    Log.Debug(MQTT_TAG, ex.Message);
                    Toast.MakeText(_context, ex.Message, ToastLength.Long);
                    return false;
                }
            });

        }

        /// <summary>
        /// Событие сброса соединения
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _client_Disconnected(object sender, MqttEndpointDisconnected e)
        {
            CloudConnectionResult.Message = "Соединение сброшено!";
            CreateAlertResult();
            Log.Debug(MQTT_TAG, "Connection was lost");
        }


        /// <summary>
        /// Остановка задачи
        /// </summary>
        public async void Stop()
        {
            if (_client.IsConnected)
            {
                await _client.DisconnectAsync();
                Log.Debug(MQTT_TAG, "Disconnected");
            }
        }


        /// <summary>
        /// Опубликовать топик
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="data"></param>
        public async void PublishBoxState(string topic, string data)
        {
            try
            {
                if (_client.IsConnected)
                {
                    var message = new MqttApplicationMessage(topic, Encoding.UTF8.GetBytes(data));
                    await _client.PublishAsync(message, MqttQualityOfService.ExactlyOnce);
                    CloudConnectionResult.Message = "Сообщение успешно отправлено!";
                }
                else
                    CloudConnectionResult.Message = "Клиент не подключен к облаку";

                CreateAlertResult();
            }
            catch (System.Exception ex)
            {
                CloudConnectionResult.Message = ex.Message;
                Log.Debug(MQTT_TAG, ex.Message);
                Toast.MakeText(_context, ex.Message, ToastLength.Long);
                CreateAlertResult();
            }
        }

        private void CreateAlertResult()
        {
            AlertDialog.Builder alert = new AlertDialog.Builder(_context);
            alert.SetTitle("Статус соединения с облаком");
            alert.SetMessage(CloudConnectionResult.Message);
            alert.SetNeutralButton("Закрыть", (senderAlert, args) =>
            {

            });
            Dialog dialog = alert.Create();
            dialog.Show();
        }
    }
}