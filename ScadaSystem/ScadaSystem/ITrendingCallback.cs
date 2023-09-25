using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ScadaSystem
{

    public interface ITrendingCallback
    {
        [OperationContract(IsOneWay =true)]
        void PrintValuesOfInputTags(Dictionary<String, double> tags);
    }
}
