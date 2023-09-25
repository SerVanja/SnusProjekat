using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using AlarmDisplay.AlarmDisplayServiceReference;

namespace AlarmDisplay
{
    public class Program
    {
        private static AlarmDisplayClient client;

        public static void Main(string[] args)
        {
            client = new AlarmDisplayClient(new InstanceContext(new AlarmDisplayCallback()));
            client.Init();
            Console.WriteLine("--------------------- Alarm Display -------------------------------------------------");
            Console.ReadLine();
        }
    }
}
