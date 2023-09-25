using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScadaSystem
{
    public class AlarmLog
    {
        [Key]
        [Required]
        public int Id { get; set; }
        [Required]
        public int AlarmId { get; set; }
        [Required]
        public DateTime TimeStamp { get; set; }
        [Required]
        public String TagName { get; set; }
        [Required]
        public double CurrentValue { get; set; }

        [Required]
        public int Priority { get; set; }

    }
}
