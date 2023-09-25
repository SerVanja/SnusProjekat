using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ScadaSystem
{
    [ServiceContract(CallbackContract =(typeof(IAlarmDisplayCallback)))]
    interface IAlarmDisplay
    {
        [OperationContract(IsOneWay = true)]
        void Init();
    }
}
