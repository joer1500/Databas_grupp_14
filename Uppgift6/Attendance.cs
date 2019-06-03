using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;

namespace Uppgift6
{
    class Attendance
    {
        public int id { get; set; }
        public int schoolchild { get; set; }
        public string date { get; set; }
        public string attendance { get; set; }
        public string sick { get; set; }
        public int staff { get; set; }
        //public TimeSpan has_drop { get; set; }
        //public TimeSpan has_pickup { get; set; }




    }
}
