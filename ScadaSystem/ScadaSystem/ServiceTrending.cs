using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ScadaSystem
{
    [ServiceBehavior]
    public class ServiceTrending : ITrending
    {
        private ITrendingCallback proxy;

        
        
        public void Init()
        {
            proxy = OperationContext.Current.GetCallbackChannel<ITrendingCallback>();
            Console.WriteLine("Formiran proksi za trending");
            TagProcessing.TagValueChanged += proxy.PrintValuesOfInputTags;

        }


    }
}
