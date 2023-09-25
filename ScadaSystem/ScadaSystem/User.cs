using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ScadaSystem
{

     public class User
    {
        [Key]
        [Required]
        public String Username { get; set; }
        public String Password { get; set; }

        public User() { }
        public User(string username, string pass)
        {
            this.Username  = username;
            this.Password = pass;
        }

    }
}
