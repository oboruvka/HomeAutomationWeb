using System;

namespace HomeAutomationWeb.Services.Irrigation
{
    public class Pump
    {
        public Pump(PumpType type)
        {
            Type = type;
        }

        public PumpType Type { get; private set; }
        public bool On { get; private set; } = false;
        public int Progress
        {
            get
            {
                if (startTime.HasValue && stopTime.HasValue)
                {
                    var elapsed = (DateTime.Now - startTime.Value).TotalSeconds;
                    var total = (stopTime.Value - startTime.Value).TotalSeconds;
                    return (int)((100 / total) * elapsed);
                }
                else return 100;
            }
        }

        public bool StopIfNeeded()
        {
            if (stopTime.HasValue && DateTime.Now >= stopTime.Value)
            {
                Stop();
                return true;
            }
            return false;
        }

        private DateTime? stopTime = null;
        private DateTime? startTime = null;

        public void Start(int secondsToRun)
        {
            On = true;
            var now = DateTime.Now;
            startTime = now;
            stopTime = now.AddSeconds(secondsToRun);
        }

        public void Stop()
        {
            On = false;
            stopTime = null;
            startTime = null;
        }
    }
}
