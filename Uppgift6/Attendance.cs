using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uppgift6
{
    class Attendance
    {
        public int id { get; set; }
        public int schoolchild { get; set; }
        public DateTime date { get; set; }
        public string sick { get; set; }
        public string attendance { get; set; }
        public int staff { get; set; }
        public string has_drop { get; set; }
        public int has_pickup { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public int section_id { get; set; }
        
    }
}
