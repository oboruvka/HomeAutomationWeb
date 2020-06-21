namespace HomeAutomationWeb.Services.Irrigation
{
    using HomeAutomationWeb.Hubs;
    using HomeAutomationWeb.Services.Mqtt;
    using Microsoft.AspNetCore.SignalR;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public class IrrigationService 
    {
        private readonly IHubContext<EndUserHub> hubContext;
        private readonly ILogger<IrrigationService> logger;
        private readonly MqttProvider mqttProvider;
        private readonly Type irrigationStatusType;
        private readonly IrrigationStatus status = new IrrigationStatus();
        private const string shutDownButtonName = "IrrigationStop";
        private Timer pumpsCheckingTimer;
        private const int pumpsCheckingPeriodInMiliseconds = 5000;
        private readonly SemaphoreSlim semaphore = new SemaphoreSlim(1);

        public IrrigationService(IHubContext<EndUserHub> hubContext, ILogger<IrrigationService> logger, MqttProvider mqttProvider)
        {
            this.hubContext = hubContext;
            this.logger = logger;
            this.mqttProvider = mqttProvider;
            irrigationStatusType = typeof(IrrigationStatus);
            pumpsCheckingTimer = new Timer(CheckPumps, null, pumpsCheckingPeriodInMiliseconds, Timeout.Infinite);
        }

        private async void CheckPumps(object state)
        {
            try
            {
                semaphore.Wait();
                if (status.PumpWell.StopIfNeeded() || status.PumpRetention.StopIfNeeded())
                    await mqttProvider.SendIrrigationStatus(status);
                await SendStatusToAllClients();
            }
            finally
            {
                semaphore.Release();
            }
            pumpsCheckingTimer.Change(pumpsCheckingPeriodInMiliseconds, Timeout.Infinite);
        }

        public async Task WebStarted()
        {
            try
            {
                semaphore.Wait();
                await SendStatusToAllClients();
            }
            finally
            {
                semaphore.Release();
            }
        }

        public async Task BtnClicked(string btnName)
        {
            try
            {
                semaphore.Wait();
                if (UpdateStatus(btnName))
                    await mqttProvider.SendIrrigationStatus(status);
                await SendStatusToAllClients();
            }
            finally
            {
                semaphore.Release();
            }
        }

        private async Task SendStatusToAllClients() => await hubContext.Clients.All.SendAsync("IrrigationStatusUpdate", JsonConvert.SerializeObject(status));

        private bool UpdateStatus(string btnName)
        {
            try
            {
                if (btnName == shutDownButtonName)
                {
                    status.PumpWell.Stop();
                    status.PumpRetention.Stop();
                    return true;
                }
                var btnValue = (bool)irrigationStatusType.GetProperty(btnName).GetValue(status);
                btnValue = !btnValue;
                irrigationStatusType.GetProperty(btnName).SetValue(status, btnValue);
                return true;
            }
            catch (Exception e)
            {
                logger.LogError(e, "Problem when parsing and updating property {btnName}", btnName);
                return false;
            }
        }

        public async Task StartPump(PumpType pump, int secondsToRun)
        {
            try
            {
                semaphore.Wait();
                if (status.CanStart(pump))
                {
                    if (pump == PumpType.Well)
                        status.PumpWell.Start(secondsToRun);

                    if (pump == PumpType.Retention)
                        status.PumpRetention.Start(secondsToRun);

                    await mqttProvider.SendIrrigationStatus(status);
                }
                await SendStatusToAllClients();
            }
            finally
            {
                semaphore.Release();
            }
        }
    }
}
