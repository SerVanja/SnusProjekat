using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ScadaSystem
{
    [ServiceContract]
    public interface IRealTimeUnit
    {
        [OperationContract]
        void WriteValue(String address, double value, byte[] hash);

        [OperationContract]
        Boolean IsAddressAvailable(String address);

        [OperationContract]
        void LeavePublicKey(String key);
    }
}
