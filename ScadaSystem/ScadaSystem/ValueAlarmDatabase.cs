using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScadaSystem
{
    class ValueAlarmDatabase:DbContext
    {
        public DbSet<Value> Values { get; set; }
        public DbSet<AlarmLog> AlarmLogs { get; set; }


        public ValueAlarmDatabase()
        {
        }

    }

    
}
