using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using Driver;

namespace ScadaSystem
{
    public static  class TagProcessing 

    {
        public static List<Tag> tags = new List<Tag>();
        public static List<Thread> threads = new List<Thread>();

        static RealTimeDriver realTimerDriver = new RealTimeDriver();
        static SimulationDriver simulationDriver = new SimulationDriver();

        static Dictionary<String, double> valuesOfInputTags = new Dictionary<string, double>();

        public delegate void updateTags(Dictionary<String, double> d);
        public static event updateTags TagValueChanged;

        public delegate void activateAlarm (AlarmLog alarmLog);
        public static event activateAlarm eventActivateAlarm;

        static readonly object tagsLocker = new object();
        static readonly object valuesOfInputTagsLocker = new object();
        static readonly object locker = new object();



        internal static void WriteValueToDriver(string address, double value)
        {
            realTimerDriver.WriteValue(address, value);
        }


        public static void StartReading() {
            Console.WriteLine("Uslo u reading");
            List<Tag> inputTags = new List<Tag>(); 
            foreach(Tag t in tags)
            {
                if (t.GetType().Equals(typeof(DigitalInput)) || t.GetType().Equals(typeof(AnalogInput))) {
                    lock (valuesOfInputTagsLocker) { 
                    valuesOfInputTags[t.TagName] = GetLastTagValue(t.TagName);
                    }
                    inputTags.Add(t);
                    Thread thread = new Thread(Simulation);
                    threads.Add(thread);
                    Console.WriteLine("Startuje nit za " + t.TagName);
                    thread.Start(t);
                    Console.WriteLine("----------------------------------------------------------");
                }
            }
            
        
        }

        public static List<Value> GetTagsValue()
        {
            List<Value> values = new List<Value>();
            using (var db = new ValueAlarmDatabase()) {
                foreach (Value a in db.Values) {
                    values.Add(a);
                }
            }
            return values;
        }

        public static List<Tag> GetTags() { return tags; }

        public static void AddAlarmLogToDatabase(AlarmLog alarmLog)
        {
            Console.WriteLine("Upisuje alarm u bazu");
            using (var db = new ValueAlarmDatabase()) {
                db.AlarmLogs.Add(alarmLog);
                db.SaveChanges();
                Console.WriteLine("Dodat");
            }
        }

        public static void AddAlarm(string tagname, Alarm alarm)
        {
            lock (tagsLocker) { 
            Tag t = findTag(tagname);
            Console.WriteLine("Tag sa " + t.TagName + t.IOAddress);
            AnalogInput a = (AnalogInput)t;
            a.Alarms.Add(alarm);
            Console.WriteLine(a.Alarms.Count);
            Console.WriteLine("Dodat alarm u listu");
            }
        }

        public static void UpdateAlarmLogs(AlarmLog alarmLog)
        {
            var path = "../../alarmsLog.txt";
            var sw = new StreamWriter(path, append: true);
            sw.WriteLine("Activated alarm with id " + alarmLog.AlarmId + " of tag " + alarmLog.TagName + " with value " + alarmLog.CurrentValue + " at " + alarmLog.TimeStamp);
            sw.Close();
        }

        public static List<Tag> GetDITags()
        {
            List<Tag> tagList = (from tag in tags
                                 where tag.GetType().Equals(typeof(DigitalInput))
                                 select tag).ToList();

            return tagList;
        }

        public static void Simulation(object o) {
            InputTag d;
            if (o.GetType().Equals(typeof(DigitalInput)))
            {
                d = (DigitalInput)o;
                    Console.WriteLine("Nasaoo digital input");
            }
            else
            {
                 d = (AnalogInput)o;
            }

            IDriver driver;
                double data;
                if (d.Driver.Equals(DriverType.REAL_TIME_DRIVER))
                { Console.WriteLine("realtime********************************************");
                    driver = realTimerDriver; }
                else
                { Console.WriteLine("simulation----------------------------------------------------------------------------------");
                    driver = simulationDriver; }
                while (true)
                {
                lock (locker) { 

                if (d.OnScan)
                    {
                        data = driver.ReadData(d.IOAddress);
                        Console.WriteLine("Sa adrese " + d.IOAddress + " ucitalo " + data);
                        if (d.GetType().Equals(typeof(AnalogInput))) { ActivateAlarms(d,data); }
                        AddTagValue(d.TagName, data);
                        Console.WriteLine("Ide invoke za "+ driver.ToString());
                        TagValueChanged?.Invoke(valuesOfInputTags);
                        Thread.Sleep(d.ScanTime * 1000);
                    }
                     }

                }
            }

        public static void AddTag(Tag t)
        {
            lock (tagsLocker) { 
                tags.Add(t);
            }
        }

        public static List<string> GetTagNames()
        {
            List<String> tagList = (from tag in tags
                                 select tag.TagName).ToList();

            return tagList;
        }

        public static void RemoveTag(Tag t)
        {
            lock (tagsLocker) {
                tags.Remove(t);
            }   
        }

        private static void ActivateAlarms(InputTag d, double data)
        {
            AnalogInput a = (AnalogInput)d;
            Console.WriteLine("Ulazi u aktiviranje alarmaaaaa");
            Console.WriteLine("trenutna vrijednost je " + data);
            Console.WriteLine("Velicina liste alarma je " + a.Alarms.Count);
            foreach (Alarm alarm in a.Alarms) {
                Console.WriteLine("tip alarm a je " + alarm.Type + " a granicna vrijednost " + alarm.Key_value);
                if (alarm.Type == "low" && data < alarm.Key_value)
                {
                    AlarmLog alarmLog = new AlarmLog
                    {
                        AlarmId = alarm.Id,
                        TimeStamp = DateTime.Now,
                        TagName = a.TagName,
                        CurrentValue = data,
                        Priority = alarm.Priority
                    };
                    eventActivateAlarm?.Invoke(alarmLog);
                    Console.WriteLine("aktiviran alaarm za loow");
                }
                else if(alarm.Type == "high" && data>alarm.Key_value){
                    AlarmLog alarmLog = new AlarmLog
                    {
                        AlarmId = alarm.Id,
                        TimeStamp = DateTime.Now,
                        TagName = a.TagName,
                        CurrentValue = data,
                        Priority = alarm.Priority
                    };
                    eventActivateAlarm?.Invoke(alarmLog);
                    Console.WriteLine("aktiviran alaarm za hifhhh");

                }
            }
        }

        public static void AddTagValue(String tagName,double data) {
            Value v = new Value
            {
                TagId = tagName,
                InputValue = data,
                TimeStamp = DateTime.Now
            };
            using (var db = new ValueAlarmDatabase()) {
                db.Values.Add(v);
                db.SaveChanges();
            }
            Tag t = (from tag in tags
                    where tag.TagName == tagName
                    select tag).ToList().First();
            lock (valuesOfInputTagsLocker) { 
            if (t.GetType().Equals(typeof(DigitalInput)) || t.GetType().Equals(typeof(AnalogInput))){
                valuesOfInputTags[t.TagName] = data;
            }
            }

        }

        public static List<AlarmLog> GetAlarmLogs() {
            List<AlarmLog> v;
            using (var db = new ValueAlarmDatabase()) {
                v = db.AlarmLogs.ToList();
            }
            return v;
        }

        public static List<Tag> GetAITags()
        {

            List<Tag> tagList = (from tag in tags
                                  where tag.GetType().Equals(typeof(AnalogInput))
                                  select tag).ToList();

            return tagList;
        }


        public static List<Alarm> GetAlarms() {
            List<Tag> AITags = GetAITags();
            List<Alarm> alarms = new List<Alarm>();
            AnalogInput aitag;
            foreach (Tag t in AITags) {
                aitag = (AnalogInput)t;
                foreach (Alarm a in aitag.Alarms) {
                    alarms.Add(a);
                }

            }
            return alarms;
        }

        public static  void ReadAllTags() {
            XmlSerializer serializer = new XmlSerializer(tags.GetType(), new Type[] { typeof(Tag),typeof(InputTag),typeof(OutputTag),typeof(AnalogInput), typeof(AnalogOutput), typeof(DigitalInput), typeof(DigitalOutput) });
            TextReader reader = new StreamReader("../../scadaConfig.xml");
            Console.WriteLine("hoce da desiri");
            tags = (List<Tag>)serializer.Deserialize(reader);
            reader.Close();
        }

        public static double GetLastTagValue(String tagName) {
            using (var db = new ValueAlarmDatabase()) {
                List<double> values = (from tagValue in db.Values
                                      where tagValue.TagId == tagName
                                      orderby tagValue.TimeStamp descending
                                      select tagValue.InputValue).ToList();

                if (values.Count != 0)
                {
                    return CheckValue(tagName, values[0]);
                }
                else {
                    return -1;
                }
            }
        }

        private static double CheckValue(string tagName, double value)
        {
            Tag t = findTag(tagName);
            if (t.GetType().Equals(typeof(AnalogInput)))
            {
                AnalogInput a = (AnalogInput)t;
                if (value < a.LowLimit) { return Double.Parse(a.LowLimit.ToString()); }
                if (value > a.HighLimit) { return Double.Parse(a.HighLimit.ToString()); }
            }
            else if (t.GetType().Equals(typeof(AnalogOutput)))
            {
                AnalogOutput a = (AnalogOutput)t;
                if (value < a.LowLimit) { return Double.Parse(a.LowLimit.ToString()); }
                if (value > a.HighLimit) { return Double.Parse(a.HighLimit.ToString()); }
            }
           
            return value;
            

        }


        public static Value GetLastValue(String tagName)
        {
            using (var db = new ValueAlarmDatabase())
            {
                List<Value> values = (from tagValue in db.Values
                                       where tagValue.TagId == tagName
                                       orderby tagValue.TimeStamp descending
                                       select tagValue).ToList();
                if (values.Count != 0)
                {
                     double val = CheckValue(tagName, values[0].InputValue);
                    Value v = values[0];
                    v.InputValue = val;
                    return v;
                }
                else
                {
                    return null;
                }

            }
        }

        private static Tag findTag(string tagName)
        {
            foreach (Tag t in tags) { 
                if(t.TagName == tagName) { return t; }
            }
            return null;
        }

        public static void WriteAllTags(List<Tag> tags) {
            Console.WriteLine("upis");
            XmlSerializer serializer = new XmlSerializer(tags.GetType(),new Type[] { typeof(Alarm),typeof(Tag),typeof(OutputTag),typeof(InputTag),typeof(AnalogInput),typeof(AnalogOutput),typeof(DigitalInput),typeof(DigitalOutput)});
            Console.WriteLine("upis");
            TextWriter writer = new StreamWriter("../../scadaConfig.xml");
            Console.WriteLine("upis");
            serializer.Serialize(writer, tags);
            Console.WriteLine("upis");
            writer.Close();
            Console.WriteLine("upis");
        }

        internal static bool IsAddressAvailable(string address)
        {
            return realTimerDriver.IsAddressAvailable(address);
        }

         public static void ChangeScanMode(String tagName)
        {
            lock (tagsLocker) { 
            foreach (Tag t in tags)
            {
                if (t.TagName.Equals(tagName))
                {
                    InputTag a;
                    if (t.GetType().Equals(typeof(AnalogInput)))
                    {
                        a = (AnalogInput)t;
                    }
                    else {
                        a = (DigitalInput)t;
                    }
                    if (a.OnScan)
                    {
                        a.OnScan = false;
                    }
                    else
                    {
                        a.OnScan = true;
                    }
                   
                }

            }
            }
        }
    }


}
