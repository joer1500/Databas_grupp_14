using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uppgift6
{
    class Schoolchild
    {
        public int id { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string section { get; set; }


        public override string ToString()
        {
            return $"{firstname} {lastname}";
        }
    }
}
