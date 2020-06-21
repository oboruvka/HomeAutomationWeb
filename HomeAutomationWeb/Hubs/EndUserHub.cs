namespace HomeAutomationWeb.Hubs
{
    using Microsoft.AspNetCore.SignalR;
    using System.Threading.Tasks;
    using HomeAutomationWeb.Services.Irrigation;
    using System;

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
        public async Task StartIrrigation(string pump, string timeInMinutes)
        {
            var parsedPump = Enum.Parse<PumpType>(pump);
            var secondsToRun = int.Parse(timeInMinutes) * 60;
            await irrigationService.StartPump(parsedPump, secondsToRun);
        }

        public async Task WebStarted()
        {
            await irrigationService.WebStarted();
        }

    }    

}
