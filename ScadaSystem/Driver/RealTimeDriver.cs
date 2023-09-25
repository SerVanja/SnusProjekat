using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Driver
{
    public class RealTimeDriver : IDriver
    {
        public static Dictionary<String, double> values = new Dictionary<string, double>();

        static readonly object locker = new object();


        public double ReadData(String address)
        {
            if (values.ContainsKey(address))
            {
                Console.WriteLine("real time driver vraca " + values[address]+" na adresu"+address);
                return values[address];
            }
            else {
                Console.WriteLine("real time driver vraca  " + 0+" na adresu "+address);
                return 0.0;
            }
        }

        public void WriteValue(string address, double value)
        {
            lock (locker) { 
            if (values.ContainsKey(address)) {
                values[address] = value;
            }
            else { 
                values.Add(address, value);
            }
            }
        }

        public bool IsAddressAvailable(string address)
        {
            return !values.ContainsKey(address);
        }
    }
}
