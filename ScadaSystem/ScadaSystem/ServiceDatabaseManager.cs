using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.ServiceModel;
using System.Text;

namespace ScadaSystem
{
    [ServiceBehavior(InstanceContextMode =InstanceContextMode.PerSession)]
    class ServiceDatabaseManager : IDatabaseManager
    {
        private static Dictionary<String, User> authenticatedUsers = new Dictionary<String, User>();
        
        
        
        public static List<User> userList = new List<User>() ;
        private readonly object locker = new object();


        public delegate void updateScadaConfig(List<Tag> tags);
        public event updateScadaConfig EventUpdateScadaConfig;

        public ServiceDatabaseManager()
        {
            EventUpdateScadaConfig += TagProcessing.WriteAllTags;

        }

        public void Init() {
            Console.WriteLine("Ucitava iniiiit ");
            Console.WriteLine("Ucitavanje usera");
            UserProcessing.LoadUsers();
            Console.WriteLine("ucitavanje tagova");
            TagProcessing.ReadAllTags();
            userList = UserProcessing.GetUsers();
            Console.WriteLine("Krece citanjee");
            TagProcessing.eventActivateAlarm += TagProcessing.UpdateAlarmLogs;
            TagProcessing.eventActivateAlarm += TagProcessing.AddAlarmLogToDatabase;

            TagProcessing.StartReading();
            
        }

        public void AddAlarm(List<Object> info, String token) {
            if(authenticatedUsers.ContainsKey(token)) { 
            int id = Int32.Parse(info[0].ToString());
            int priority = Int32.Parse(info[2].ToString());
            int keyValue = Int32.Parse(info[3].ToString());

            String type;
            if (info[1].Equals("1"))
            {
                type = "low";

            }
            else {
                type = "high";
            }
            Alarm a = new Alarm(id, type, priority, keyValue);
            TagProcessing.AddAlarm(info[4].ToString(), a);
            EventUpdateScadaConfig?.Invoke(TagProcessing.tags);
            Console.WriteLine("Update scadaconfiga");
            }
        }
        public List<String> GetAITagNames(String token) {
            if (authenticatedUsers.ContainsKey(token))
            {

                List<String> names = (from tag in TagProcessing.tags
                                      where tag.GetType().Equals(typeof(AnalogInput))
                                      select tag.TagName).ToList();

                return names;
            }
            return null;
        }



            public void AddTags(List<Object> info, String token) {
            if (authenticatedUsers.ContainsKey(token))
            {

                String option = info.ElementAt(0).ToString();
                String tagId = (string)info.ElementAt(1);
                String address = (string)info.ElementAt(2);
                String description = (string)info.ElementAt(3);
                if (option.Equals("1"))
                {
                    DriverType driver;
                    if (((string)info.ElementAt(4)).Equals("1"))
                    { driver = DriverType.REAL_TIME_DRIVER; }
                    else
                    { driver = DriverType.SIMULATION_DRIVER; }
                    int scanTime = Int32.Parse(info.ElementAt(5).ToString());
                    int low = Int32.Parse(info.ElementAt(5).ToString());
                    int high = Int32.Parse(info.ElementAt(6).ToString());
                    AnalogInput t = new AnalogInput(tagId, address, description, driver, scanTime, low, high);
                    Console.WriteLine("formiralo tag");
                    TagProcessing.AddTag(t);
                }
                else if (option.Equals("2"))
                {
                    int initial = Int32.Parse(info.ElementAt(4).ToString());
                    int low = Int32.Parse(info.ElementAt(5).ToString());
                    int high = Int32.Parse(info.ElementAt(6).ToString());
                    AnalogOutput t = new AnalogOutput(tagId, address, description, initial, low, high);
                    TagProcessing.AddTag(t);

                }
                else if (option.Equals("3"))
                {
                    DriverType driver;
                    if (info.ElementAt(4).Equals("1")) { driver = DriverType.REAL_TIME_DRIVER; }
                    { driver = DriverType.SIMULATION_DRIVER; }
                    int scanTime = Int32.Parse(info.ElementAt(5).ToString());
                    DigitalInput t = new DigitalInput(tagId, address, description, driver, scanTime);
                    TagProcessing.AddTag(t);
                }
                else
                {
                    int initial = Int32.Parse(info.ElementAt(4).ToString());
                    DigitalOutput t = new DigitalOutput(initial, tagId, description, address);
                    TagProcessing.AddTag(t);

                }
                Console.WriteLine("Ide u invoke");
                EventUpdateScadaConfig?.Invoke(TagProcessing.tags);

            }
        }


        public void RemoveTags(String tagName,String token) {
            if (authenticatedUsers.ContainsKey(token))
            {

                foreach (Tag t in TagProcessing.tags)
                {
                    if (t.TagName == tagName)
                    {
                        TagProcessing.RemoveTag(t);
                        break;
                    }
                }
                EventUpdateScadaConfig?.Invoke(TagProcessing.tags);
            }
        }

        public List<String> GetTagNames(String token) {
            if (authenticatedUsers.ContainsKey(token))
            {

                List<String> names = (from tag in TagProcessing.tags
                                      select tag.TagName).ToList();

                return names;
            }
            return null;
        }

        public void ChangeOutputValue(String tagName,double value,String token)
        {
            if (authenticatedUsers.ContainsKey(token))
            {
                TagProcessing.AddTagValue(tagName, value);
            }
        }

        public List<String> GetOutputValue(String token)
        {
            if (authenticatedUsers.ContainsKey(token))
            {

                List<Tag> outputTags = (from tag in TagProcessing.tags
                                        where tag.Input == false
                                        select tag).ToList();
                List<String> info = new List<String>();
                double val;
                foreach (Tag t in outputTags)
                {
                    OutputTag m;
                    if (t.GetType().Equals(typeof(AnalogOutput)))
                    {
                        m = (AnalogOutput)t;
                    }
                    else
                    {
                        m = (DigitalOutput)t;
                    }
                    val = TagProcessing.GetLastTagValue(m.TagName);
                    if (val != -1)
                    {
                        info.Add(m.TagName + ":" + val);
                    }
                    else
                    {
                        info.Add(m.TagName + ":" + m.InitialValue);
                    }

                }

                return info;
            }
            return null;
        }

        public String LogIn(string username, string password)
        {
            if (username == "admin" && password == "admin") {

                String token = GenerateToken(username);

                authenticatedUsers.Add(token, new User("admin","admin"));

                return token;
            }
            foreach (var u in userList) {
                if (u.Username == username && ValidateEncryptedData(password,u.Password)) {
                    String token = GenerateToken(username);
                    authenticatedUsers.Add(token, u);
                    return token;
                }
            }
            return null;
        }

        private bool ValidateEncryptedData(string password,string passwordFromDatabase)
        {
            String [] arrValues = passwordFromDatabase.Split(':');
            string encryptedDbValue = arrValues[0];
            string salt = arrValues[1];
            byte[] saltedValue = Encoding.UTF8.GetBytes(salt + password);
            using (var sha = new SHA256Managed())
            {
                byte[] hash = sha.ComputeHash(saltedValue);
                string enteredValueToValidate = Convert.ToBase64String(hash);
                return encryptedDbValue.Equals(enteredValueToValidate);
            }
        }

        private string GenerateToken(string username)
        {
            RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider();
            byte[] randVal = new byte[32];
            crypto.GetBytes(randVal);
            String randStr = Convert.ToBase64String(randVal);
            return username + randStr;
        }


        public void LogOut(String token)
        {
            authenticatedUsers.Remove(token);
        }

        public bool RegisterUser(String username, String password,String token)
        {
            if (authenticatedUsers.ContainsKey(token))
            {

                foreach (User u in userList)
                {
                    if (u.Username == username) { return false; }
                }
                String encryptedPassword = EncryptData(password);
                User newUser = new User(username, encryptedPassword);
                lock (locker) { 
                userList.Add(newUser);
                }
                UserProcessing.AddUser(newUser);
                return true;
            }
            return false;
        }

        private string EncryptData(string password)
        {
            string GenerateSalt() {
                RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider();
                byte[] salt = new byte[32];
                crypto.GetBytes(salt);
                return Convert.ToBase64String(salt);
            }
            string EncryptValue(String strValue) {
                string saltValue = GenerateSalt();
                byte[] saltedPassword = Encoding.UTF8.GetBytes(saltValue + strValue);
                using (SHA256Managed sha = new SHA256Managed()) {
                    byte[] hash = sha.ComputeHash(saltedPassword);
                    return $"{Convert.ToBase64String(hash)}:{saltValue}";
                }
            }
            return EncryptValue(password);
        }

        public List<String> GetScanValue(String token)
        {
            if (authenticatedUsers.ContainsKey(token))
            {

                List<Tag> outputTags = (from tag in TagProcessing.tags
                                        where tag.Input == true
                                        select tag).ToList();
                List<String> info = new List<String>();
                foreach (Tag t in outputTags)
                {
                    InputTag m = (InputTag)t;
                    Console.WriteLine(m.OnScan);
                    Console.WriteLine(m.TagName);
                    info.Add(m.TagName + ":" + m.OnScan);

                }

                return info;
            }return null;
        }

        public void ChangeScanMode(String tagName,String token)
        {
            if (authenticatedUsers.ContainsKey(token))
            {

                TagProcessing.ChangeScanMode(tagName);
                EventUpdateScadaConfig?.Invoke(TagProcessing.tags);
            }
        }
    }
}

public enum DriverType
{
    SIMULATION_DRIVER,
    REAL_TIME_DRIVER
}
