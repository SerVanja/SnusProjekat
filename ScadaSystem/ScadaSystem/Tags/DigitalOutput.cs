using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScadaSystem
{
    public class DigitalOutput:OutputTag
    {

        public DigitalOutput() { }

        public DigitalOutput(int initial,String tagName,String description,String address):base(initial,tagName,description,address,true,false)
        {
           
        }
    }
}
