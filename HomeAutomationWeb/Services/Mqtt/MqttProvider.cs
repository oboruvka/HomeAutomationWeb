namespace HomeAutomationWeb.Services.Mqtt
{
    using HomeAutomationWeb.Services.Irrigation;
    using MQTTnet;
    using MQTTnet.Client;
    using MQTTnet.Client.Options;
    using Newtonsoft.Json;
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public class MqttProvider
    {
        private readonly IMqttClient mqttClient;

        public MqttProvider()
        {
            var factory = new MqttFactory();
            mqttClient = factory.CreateMqttClient();            
        }

        public async Task Connect()
        {
            var options = new MqttClientOptionsBuilder()
                .WithClientId("HomaAutomation")
                .WithTcpServer("localhost")
                //.WithCredentials("bud", "%spencer%")
                //.WithTls()
                .WithCleanSession()
                .Build();
            var connection = mqttClient.ConnectAsync(options).Result;
            if (connection.ResultCode != MQTTnet.Client.Connecting.MqttClientConnectResultCode.Success)
                throw new ApplicationException("Cannot connect to mqtt server");
        }

        public async Task SendIrrigationStatus(IrrigationStatus status)
        {
            if (!mqttClient.IsConnected)
                await Connect();
            var message = new MqttApplicationMessageBuilder()
                .WithTopic("Logging/Irrigation")//TODO
                .WithPayload(JsonConvert.SerializeObject(status))
                .WithExactlyOnceQoS()
                .Build();

            await mqttClient.PublishAsync(message, CancellationToken.None);
        }

    }
}
