using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkMapperForms
{
    public class Details
    {
        public String? Lat { get; set; }
        public String? Long { get; set; }
        public DateTime LastSeen { get; set; }
        public bool Marked { get; set; }
    }
}
