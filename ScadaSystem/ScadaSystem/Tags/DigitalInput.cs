using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScadaSystem
{
    public class DigitalInput:InputTag
    {

        public DigitalInput() { }

        public DigitalInput(String tagId, String address, String description, DriverType driver, int scanTime) : base(scanTime, false, driver,tagId,description,address,true,true)
        {
        }

        public DigitalInput(String tagId, String address, String description, DriverType driver, int scanTime,Boolean onscan) : base(scanTime, onscan, driver, tagId, description, address, true, true)
        {
           
        }
    }
}
