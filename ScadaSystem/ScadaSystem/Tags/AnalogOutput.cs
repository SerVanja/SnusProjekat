using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScadaSystem
{
    public class AnalogOutput : OutputTag
    {


        public String Units { get; set; }
        public int LowLimit { get; set; }
        public int HighLimit { get; set; }

        public  AnalogOutput(){}

        public AnalogOutput(string tagId, string address, string description, int initial, int low, int high):base(initial,tagId,description,address,false,false)
        {
            this.LowLimit = low;
            this.HighLimit = high;
        }
    }
}
