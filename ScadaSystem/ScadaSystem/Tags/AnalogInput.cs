using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScadaSystem
{

    public class AnalogInput:InputTag
    {
        
        public List<Alarm> Alarms { get; set; }
        public String Units { get; set; }
        public int LowLimit { get; set; }
        public int HighLimit { get; set; }

        public AnalogInput() { }

        public AnalogInput(String tagId, String address, String description, DriverType driver , int scanTime,int low,int high) :base(scanTime,false,driver,tagId,address,description,false,true)
        {
            this.LowLimit = low;
            this.HighLimit = high;
            this.Alarms = new List<Alarm>();
        }

        public AnalogInput(String tagId, String address, String description, DriverType driver, int scanTime, int low, int high,Boolean onscan) : base(scanTime, onscan, driver,tagId,description,address,false,true)
        {
            this.LowLimit = low;
            this.HighLimit = high;
            this.Alarms = new List<Alarm>();
        }
        public AnalogInput(String tagId, String address, String description, DriverType driver, int scanTime, int low, int high, List<Alarm> alarms) : base(scanTime, false, driver, tagId, description, address, false, true)
        {
            this.LowLimit = low;
            this.HighLimit = high;
            this.Alarms = alarms;

        }
        public AnalogInput(String tagId, String address, String description, DriverType driver, int scanTime, int low, int high, List<Alarm> alarms,Boolean onscan) : base(scanTime, onscan, driver, tagId, description, address, false, true)
        {
            this.LowLimit = low;
            this.HighLimit = high;
            this.Alarms = alarms;

        }
    }
}
