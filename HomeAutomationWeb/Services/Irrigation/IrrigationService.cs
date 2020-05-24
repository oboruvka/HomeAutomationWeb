using HomeAutomationWeb.Hubs;
using Microsoft.AspNet.SignalR.Hubs;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using System;
using System.Data;
using System.Threading.Tasks;

namespace HomeAutomationWeb.Services.Irrigation
{
    public class IrrigationService
    {
        private readonly IHubContext<EndUserHub> hubContext;
        private readonly Type irrigationStatusType;
        private IrrigationStatus status = new IrrigationStatus();

        public IrrigationService(IHubContext<EndUserHub> hubContext)
        {
            this.hubContext = hubContext;
            irrigationStatusType = typeof(IrrigationStatus);

        }
        public async Task BtnClicked(string btnName)
        {
            //TODO logic ... now just return statuses to client
            UpdateStatus(btnName);
            await Task.Delay(1000);
            await hubContext.Clients.All.SendAsync("IrrigationStatusUpdate", JsonConvert.SerializeObject(status));
        }

        private bool UpdateStatus(string btnName)
        {
            try
            {
                var btnValue = (bool)irrigationStatusType.GetProperty(btnName).GetValue(status);
                btnValue = !btnValue;
                irrigationStatusType.GetProperty(btnName).SetValue(status,btnValue);
                return true;
            }
            catch(Exception e)
            {
                return false;
            }
        }
    }
}
