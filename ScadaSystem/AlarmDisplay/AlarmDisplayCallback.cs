using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlarmDisplay.AlarmDisplayServiceReference;

namespace AlarmDisplay
{
    class AlarmDisplayCallback : IAlarmDisplayCallback
    {
        public void PrintActivatedAlarm(AlarmLog alarmLog)
        {
            int priority = alarmLog.Priority;
            Console.WriteLine("Trebalo bi da printa,jer je priority" + alarmLog.Priority);
            while (priority > 0) {
                Console.WriteLine("Activated alarm with id " + alarmLog.AlarmId + " of tag " + alarmLog.TagName + " with value " + alarmLog.CurrentValue + " at " + alarmLog.TimeStamp);
                priority--;
            }
        }
    }
}
