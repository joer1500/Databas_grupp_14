using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uppgift6
{
    class Pickup
    {
        public int PickupID { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Relation { get; set; }

        public override string ToString()
        {
            return Firstname + " " + Lastname + "-  " + Relation;
        }

    }
}
