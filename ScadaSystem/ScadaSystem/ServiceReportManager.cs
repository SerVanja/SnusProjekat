using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScadaSystem
{
    public class ServiceReportManager : IReportManager
    {
        public List<AlarmLog> Report1(DateTime startTime, DateTime endTime)
        {
            List<AlarmLog> alarmLogs = TagProcessing.GetAlarmLogs();
            List<AlarmLog> filteredLogs = alarmLogs.Where(x => x.TimeStamp < endTime && x.TimeStamp > startTime).ToList();
            List<AlarmLog> returnData = new List<AlarmLog>();
            var groupsByPriority = from f in filteredLogs
                                    group f by f.Priority
                                    into groupByPriority
                                    orderby groupByPriority.Key
                                    select groupByPriority;

            foreach (var g in groupsByPriority)
            {
                var sortedByTimeStamp = from log in g
                           orderby log.TimeStamp
                           select log;
                foreach (var l in sortedByTimeStamp)
                {
                    returnData.Add(l);
                }
            }
            if (returnData.Count > 50)
            {
                var r= returnData.Skip(Math.Max(0, returnData.Count-50));
                returnData = new List<AlarmLog>();
                foreach (var v in r) {
                    returnData.Add(v);
                }
                return returnData;
            }
            else {
                return returnData;
            }
        }

        public List<AlarmLog> Report2(int priority)
        {
            List<AlarmLog> alarmLogs = TagProcessing.GetAlarmLogs();
            List<AlarmLog> data = new List<AlarmLog>();
            foreach (AlarmLog r in alarmLogs)
            {
                if (r.Priority == priority)
                {
                    data.Add(r);
                }
            }
            List<AlarmLog> returnData = (from l in data
                                         orderby l.TimeStamp
                                         select l).ToList();
            return returnData;
        }

        public List<Value> Report3(DateTime startTime, DateTime endTime)
        {
            List<Value> values = TagProcessing.GetTagsValue();
            List<Value> filteredValues = (from v in values
                                          where v.TimeStamp < endTime && v.TimeStamp > startTime
                                          orderby v.TimeStamp
                                          select v).ToList();
            if (filteredValues.Count > 50)
            {
                var r = filteredValues.Skip(Math.Max(0, filteredValues.Count - 50));
                filteredValues = new List<Value>();
                foreach (var v in r)
                {
                    filteredValues.Add(v);
                }
                return filteredValues;
            }
            else
            {
                return filteredValues;
            }
        }

        public List<Value> Report4()
        {
            List<Tag> AITags = TagProcessing.GetAITags();
            List<Value> returnValue = new List<Value>();
            foreach (Tag t in AITags) {
                AnalogInput a = (AnalogInput)t;
                Value val = TagProcessing.GetLastValue(a.TagName);
                returnValue.Add(val);
            }
            return returnValue;
            
        }

        public List<Value> Report5()
        {

            List<Tag> DITags = TagProcessing.GetDITags();
            List<Value> returnValue = new List<Value>();
            foreach (Tag t in DITags)
            {
                DigitalInput a = (DigitalInput)t;
                Value val = TagProcessing.GetLastValue(a.TagName);
                returnValue.Add(val);
            }
            return returnValue;

        }

        public List<Value> Report6(string tagName)
        {
            List<Value> values = TagProcessing.GetTagsValue();
            List<Value> filteredValues = (from v in values
                                          where v.TagId == tagName
                                          orderby v.InputValue
                                          select v).ToList();

            if (filteredValues.Count > 50)
            {
                var r = filteredValues.Skip(Math.Max(0, filteredValues.Count - 50));
                filteredValues = new List<Value>();
                foreach (var v in r)
                {
                    filteredValues.Add(v);
                }
                return filteredValues;
            }
            else
            {
                return filteredValues;
            }
        }

        public List<String> GetTagNames() {
            Console.WriteLine(" --------------------------------------- ");
            Console.WriteLine(TagProcessing.GetTags().Count);
            Console.WriteLine(" --------------------------------------- ");

            return TagProcessing.GetTagNames();
        }
    }
}
