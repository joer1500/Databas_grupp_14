using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uppgift6
{
    public partial class Guardian 
    {
        public int id { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string phonenumber { get; set; }
        public string  address { get; set; }

        public override string ToString()
        {
            return firstname + " " + lastname +":  "+ phonenumber;
        }

        public string fullName //Använder denna för att visa namn i combobox på startsidan
        {
            get
            {
                return $"{firstname} {lastname}";
            }
        }
    }
}
