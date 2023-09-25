using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ScadaSystem
{
    class ServiceAlarmDisplay : IAlarmDisplay
    {
        private IAlarmDisplayCallback proxy;

        public void Init()
        {
            proxy = OperationContext.Current.GetCallbackChannel<IAlarmDisplayCallback>();
            Console.WriteLine("Formiran proksi za alarm display-om");
            TagProcessing.eventActivateAlarm += proxy.PrintActivatedAlarm;
        }
    }
}
