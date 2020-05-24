namespace HomeAutomationWeb.Hubs
{
    using Microsoft.AspNetCore.SignalR;
    using System.Linq;
    using System.Threading.Tasks;
    using Newtonsoft.Json;
    using HomeAutomationWeb.Services.Irrigation;

    public class EndUserHub : Hub
    {
        private readonly IrrigationService irrigationService;

        public EndUserHub(IrrigationService irrigationService)
        {
            this.irrigationService = irrigationService;
        }
        public async Task IrrigationButtonClicked(string btnName)
        {
            await irrigationService.BtnClicked(btnName);
        }
    }    
}
