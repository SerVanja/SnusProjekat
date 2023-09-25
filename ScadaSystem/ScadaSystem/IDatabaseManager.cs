using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ScadaSystem
{
    [ServiceContract(SessionMode =SessionMode.Allowed)]
    public interface IDatabaseManager
    {
        [OperationContract]
        void ChangeOutputValue(String t,double v,String token);

        [OperationContract]
        void Init();

        [OperationContract]
        List<String> GetOutputValue(String token);

        [OperationContract]
        void ChangeScanMode(String tagName, String token);

        [OperationContract]
        void AddAlarm(List<Object> info, String token);

        [OperationContract]
        List<String> GetAITagNames(String token);

        [OperationContract]
        List<String> GetScanValue(String token);

        [OperationContract]
        void AddTags(List<Object> info, String token);


        [OperationContract]
        void RemoveTags(String tagName, String token);

        [OperationContract]
        List<String> GetTagNames(String token);

        [OperationContract]
        bool RegisterUser(String username,String password, String token);

        [OperationContract]
        String LogIn(String username,String password);

        [OperationContract]
        void LogOut(String token);
    }
}
