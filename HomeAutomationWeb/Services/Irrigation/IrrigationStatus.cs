using System.Runtime.Serialization;

namespace HomeAutomationWeb.Services.Irrigation
{
    [DataContract]
    public class IrrigationStatus
    { 
        [DataMember]
        public bool pumpWell { get; set; }
        [DataMember]
        public bool valveSE { get; set; }
        [DataMember]
        public bool valveSW { get; set; }
        [DataMember]
        public bool valveW { get; set; }
        [DataMember]
        public bool valveNW { get; set; }
        [DataMember]
        public bool valveNE { get; set; }

        [DataMember]
        public bool pumpTank { get; set; }
        [DataMember]
        public bool valveN { get; set; }

    }
}
