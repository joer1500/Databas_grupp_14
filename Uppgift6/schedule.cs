using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uppgift6
{
    class Schedule
    {
        public int id { get; set; }
        public int schoolchild_id { get; set; }
        public DateTime date { get; set; }
        public string day_off { get; set; }
        public string breakfast { get; set; }
        public TimeSpan should_drop { get; set; }
        public TimeSpan should_pickup { get; set; }
        public string walk_home_alone { get; set; }
        public string home_with_friend { get; set; }

        public override string ToString()
        {
            return date.ToString("dddd, dd MMMM yyyy");
        }

    }
}
