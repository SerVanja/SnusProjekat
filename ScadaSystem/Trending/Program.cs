using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Trending.TrendingServiceReference;
namespace Trending
{
    class Program
    {
        private static TrendingClient client;

        static void Main(string[] args)
        {
            client = new TrendingClient(new InstanceContext(new TrendingCallback()));
            client.Init();
            Console.ReadLine();
        }
    }
}
