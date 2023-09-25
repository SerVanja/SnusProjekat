using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ScadaSystem
{
    public class ServiceRealTimeUnit : IRealTimeUnit
    {

        const string KEY_STORE_NAME = "MyKeyStore";
        public static CspParameters csp = new CspParameters();
        public static RSACryptoServiceProvider rsa;

        const string EXPORT_FOLDER = @"C:\public_key";
        
        public string public_key_file;


        public bool IsAddressAvailable(String address)
        {
            return TagProcessing.IsAddressAvailable(address);
        }

        public void WriteValue(string address, double value,byte[] message)
        {
            EventWaitHandle waitHandle = new EventWaitHandle(true, EventResetMode.AutoReset, public_key_file);
            waitHandle.WaitOne();
            ImportPublicKey();
            waitHandle.Set();
            if (VerifySignedMessage(value.ToString(), message))
            {
                TagProcessing.WriteValueToDriver(address, value);
            }
            
        }

        public void LeavePublicKey(string key)
        {
            public_key_file = key;
        }

        #region Verify digital signature

        private  void ImportPublicKey()
        {
            string path = Path.Combine(EXPORT_FOLDER, public_key_file);
            FileInfo fi = new FileInfo(path);

            if (fi.Exists)
            {
                using (StreamReader reader = new StreamReader(path))
                {
                    csp.KeyContainerName = KEY_STORE_NAME;
                    rsa = new RSACryptoServiceProvider(csp);

                    string publicKeyText = reader.ReadToEnd();
                    rsa.FromXmlString(publicKeyText);
                    rsa.PersistKeyInCsp = true;
                }
            }
        }

        private  byte[] ComputeMessageHash(string value)
        {
            using (var sha = SHA256Managed.Create())
            {
                return sha.ComputeHash(Encoding.UTF8.GetBytes(value));
            }
        }

        private  bool VerifySignedMessage(string message, byte[] signature)
        {

            var hash = ComputeMessageHash(message);

            var deformatter = new RSAPKCS1SignatureDeformatter(rsa);
            deformatter.SetHashAlgorithm("SHA256");
            return deformatter.VerifySignature(hash, signature);

        }

        #endregion
    }
}
