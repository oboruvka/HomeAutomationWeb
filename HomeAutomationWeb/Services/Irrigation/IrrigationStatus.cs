namespace HomeAutomationWeb.Services.Irrigation
{
    using Newtonsoft.Json;
    using System.Runtime.Serialization;

    [DataContract]
    public class IrrigationStatus
    {       
        [DataMember]
        public bool IrrigationValveSE { get; set; }
        [DataMember]
        public bool IrrigationValveSW { get; set; }
        [DataMember]
        public bool IrrigationValveW { get; set; }
        [DataMember]
        public bool IrrigationValveNW { get; set; }
        [DataMember]
        public bool IrrigationValveNE { get; set; }
        [DataMember]
        public bool IrrigationValveN { get; set; }


        [DataMember]
        public bool IrrigationPumpWell => PumpWell.On;

        [DataMember]
        public int IrrigationPumpWellProgress => PumpWell.Progress;

        [DataMember]
        public bool IrrigationPumpRetention => PumpRetention.On;

        [DataMember]
        public int IrrigationPumpRetentionProgress => PumpRetention.Progress;
        
        [JsonIgnore]
        public Pump PumpWell = new Pump(PumpType.Well);
        [JsonIgnore]
        public Pump PumpRetention = new Pump(PumpType.Retention);

        public bool CanStart(PumpType pump) // valves belongs to particular pump
        {
            if (pump == PumpType.Well)
            {
                return IrrigationValveNW || IrrigationValveNE || IrrigationValveW || IrrigationValveSW || IrrigationValveSE;
            }
            if (pump == PumpType.Retention)
            {
                return IrrigationValveN;
            }
            return false;
        }       

    }
}
