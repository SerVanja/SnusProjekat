using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScadaSystem
{
    public class Alarm
    {
        public int Id { get; set; }
        public String Type { get; set; }
        public int Priority { get; set; }
        public int Key_value { get; set; }


        public  Alarm() { }

        public Alarm(int id, String type, int Priority, int keyValue) {
            this.Id = id;
            this.Type = type;
            this.Priority = Priority;
            this.Key_value = keyValue;
        }

    }
}
