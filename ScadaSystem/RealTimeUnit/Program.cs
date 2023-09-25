using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using RealTimeUnit.RealTimeUnitServiceReference;

namespace RealTimeUnit
{
    class Program
    {

        static RealTimeUnitClient proxy = new RealTimeUnitClient();

        const string KEY_STORE_NAME = "MyKeyStore";
        public static CspParameters csp = new CspParameters();
        public static RSACryptoServiceProvider rsa;
        const string EXPORT_FOLDER = @"C:\public_key";
        const string PUBLIC_KEY_FILE = @"rsaPublicKey.txt";

        public static void Main(string[] args)
        {
            Console.WriteLine("------------------- Initialization of Real Time Unit -----------------------------");

            String address;
            int maxVal;
            int minVal;
            while (true) { 
            Console.WriteLine("Input I/O address for writing:");
            address = Console.ReadLine();
            if (proxy.IsAddressAvailable(address))  { break; }
            else {
                    Console.WriteLine("Address is not available.");
                }
            }
            while (true) {
                minVal = InputNumber("Input minimum value:");
                maxVal = InputNumber("Input maximum value: ");
                if (minVal < maxVal) { break; }
                Console.WriteLine("Maximum value must be greater than minimum value.");
            }
            Random r = new Random();
            CreateAsmKeys(out string containerName, true);
            ExportPublicKey();
            while (true) {
                int randNum = r.Next(minVal, maxVal);
                byte[] hash = SignMessage(randNum.ToString());
                EventWaitHandle waitHandle = new EventWaitHandle(true, EventResetMode.AutoReset, PUBLIC_KEY_FILE);
                waitHandle.WaitOne();
                proxy.LeavePublicKey(PUBLIC_KEY_FILE);
                waitHandle.Set();
                proxy.WriteValue(address,Double.Parse(randNum.ToString()),hash);
            }
        }

        private static int InputNumber(String message)
        {
            while (true)
            {
                Console.WriteLine(message);
                String input = Console.ReadLine();
                try
                {
                    int num = Int32.Parse(input);
                    return num;
                }
                catch (Exception e)
                {
                    Console.WriteLine("Invalid input!");
                }
            }
        }

        #region Create digital signature
        

        private static void CreateAsmKeys(out string containerName, bool useMachineKeyStore)
        {
            csp.KeyContainerName = KEY_STORE_NAME;

            if (useMachineKeyStore)
            {
                csp.Flags = CspProviderFlags.UseMachineKeyStore;
            }

            rsa = new RSACryptoServiceProvider(csp);
            rsa.PersistKeyInCsp = true;

            CspKeyContainerInfo info = new CspKeyContainerInfo(csp);
            containerName = info.KeyContainerName;

            Console.WriteLine($"The key container name: {info.KeyContainerName}");
            Console.WriteLine($"Unique key container name: {info.UniqueKeyContainerName}");
        }
        private static void ExportPublicKey()
        {
            if (!Directory.Exists(EXPORT_FOLDER))
            {
                Directory.CreateDirectory(EXPORT_FOLDER);
            }

            using (StreamWriter writer = new StreamWriter(Path.Combine(EXPORT_FOLDER, PUBLIC_KEY_FILE)))
            {
                writer.WriteLine(rsa.ToXmlString(false));
            }
        }

        private static byte[] SignMessage(string message)
        {
            using (var sha = SHA256Managed.Create())
            {
                var hashValue = sha.ComputeHash(Encoding.UTF8.GetBytes(message));

                RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(csp);
                var formatter = new RSAPKCS1SignatureFormatter(rsa);
                formatter.SetHashAlgorithm("SHA256");

                return formatter.CreateSignature(hashValue);
            }
        }


        #endregion

    }
}
