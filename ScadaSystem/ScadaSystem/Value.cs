using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScadaSystem
{
    public class Value
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public String TagId { get; set; }
        [Required]
        public double InputValue { get; set; }

        [Required]
        public DateTime TimeStamp { get; set; }
    }
}
