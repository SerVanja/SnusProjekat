using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ScadaSystem
{
    class Program
    {

        static void Main(string[] args)
        {
          

            ServiceHost svc = new ServiceHost(typeof(ServiceDatabaseManager));
            ServiceHost svc1 = new ServiceHost(typeof(ServiceTrending));
            ServiceHost svc2 = new ServiceHost(typeof(ServiceAlarmDisplay));
            ServiceHost svc3 = new ServiceHost(typeof(ServiceRealTimeUnit));
            ServiceHost svc4 = new ServiceHost(typeof(ServiceReportManager));

            svc.Open();
            svc1.Open();
            svc2.Open();
            svc3.Open();
            svc4.Open();
            Console.WriteLine("Listeninggg..");
            Console.ReadLine();

            svc.Close();
            svc1.Close();
            svc2.Close();
            svc3.Close();
            svc4.Close();
        }
    }
}
