using System;
using System.Collections.Generic;
using System.Text;

namespace RegisterMobile.Models
{
    public class Competition
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string DateTime { get; set; }
        public string Location { get; set; }
        public string Color { get; set; }
        public bool Status { get; set; }

    }
}
