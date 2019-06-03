using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uppgift6
{
    public partial class Needs
    {
        public int id { get; set; }
        public int child_id { get; set; }
        public string need { get; set; }

        public override string ToString()
        {
            return need;
        }

    }
}
