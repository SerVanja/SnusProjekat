using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScadaSystem
{
    public abstract class OutputTag:Tag
    {
        public int InitialValue { get; set; }

        public OutputTag() { }

        public OutputTag(int initial,String tagName, String description, String address, Boolean digital, Boolean input) : base(tagName, description, address, digital, input) {
            this.InitialValue = initial;
        }
    }
}
