using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uppgift6
{
    class Staff
    {

        public int staffID { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string profession { get; set; }
        public string sectionname { get; set; }
        public int sectionid { get; set; }


        public override string ToString()
        {
            return firstname + " " + lastname + " " + profession;
        }
    }
}
