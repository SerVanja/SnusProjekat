using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Driver
{
    public class SimulationDriver : IDriver
    {
        public double ReadData(string address)
        {
            // U ovoj implementaciji simulacionog driver-a adrese su opisne (po uzoru na iFIX)
            // S - sine
            // C - cosine
            // R - ramp
            Console.WriteLine("adresa" + address);
            if (address == "S") { Console.WriteLine("vraca sinus"); return Sine(); }
            else if (address == "C")
            {
                Console.WriteLine("vraca kosinus"); return Cosine();
            }
            else if (address == "R")
            {
                Console.WriteLine("vraca rampa"); return Ramp();
            }
            else { Console.WriteLine("vraca -1000"); return -1000; }
        }

        private static double Sine()
        {
            Console.WriteLine("Vraca sinus");
            return 100 * Math.Sin((double)DateTime.Now.Second / 60 * Math.PI);
        }

        private static double Cosine()
        {
            return 100 * Math.Cos((double)DateTime.Now.Second / 60 * Math.PI);
        }

        private static double Ramp()
        {
            return 100 * DateTime.Now.Second / 60;
        }
    }
}
