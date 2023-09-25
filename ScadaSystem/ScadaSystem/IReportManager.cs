using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ScadaSystem
{
    [ServiceContract]
    public interface IReportManager
    {
        [OperationContract]
        List<AlarmLog> Report1(DateTime startTime, DateTime endTime);

        [OperationContract]
        List<AlarmLog> Report2(int priority);

        [OperationContract]
        List<Value> Report3(DateTime startTime, DateTime endTime);

        [OperationContract]
        List<Value> Report4();

        [OperationContract]
        List<Value> Report5();

        [OperationContract]
        List<Value> Report6(String tagName);

        [OperationContract]
        List<String> GetTagNames();
    }
}