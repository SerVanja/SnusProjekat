using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScadaSystem
{
    public abstract class InputTag:Tag
    {
        public int ScanTime { get; set; }
        public Boolean OnScan { get; set; }
        public DriverType Driver { get; set; }

        public InputTag() { }

        public InputTag(int scanTime,Boolean OnScan,DriverType driver, String tagName, String description, String address, Boolean digital, Boolean input) :base(tagName,description,address,digital,input) {
            this.ScanTime = scanTime;
            this.OnScan = OnScan;
            this.Driver = driver;
        }
    }
}
