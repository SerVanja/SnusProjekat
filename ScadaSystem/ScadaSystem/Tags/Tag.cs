using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScadaSystem
{
    public abstract class Tag
    {
         public  String TagName { get; set; }
         public String Description { get; set; }
         public String IOAddress { get; set; }
         public Boolean Digital { get; set; }
         public Boolean Input { get; set; }

        public Tag() { }


        public Tag(String tagName, String description, String address, Boolean digital, Boolean input) {
            this.TagName = tagName;
            this.Description = description;
            this.IOAddress = address;
            this.Digital = digital;
            this.Input = input;
        }
    }
}
