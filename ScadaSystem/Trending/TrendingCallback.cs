using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trending.TrendingServiceReference;
namespace Trending
{
    public class TrendingCallback : ITrendingCallback
    {
        public void PrintValuesOfInputTags(Dictionary<String, double> tags)
        {
            Console.WriteLine("-------- Ispis posljednjih vrijednosti input tagova -----------");
            Console.WriteLine("Tag Name        Last value");
            foreach (String key in tags.Keys) {
                Console.WriteLine($" {key}                      {tags[key]}");
            }
        }
    }
}
