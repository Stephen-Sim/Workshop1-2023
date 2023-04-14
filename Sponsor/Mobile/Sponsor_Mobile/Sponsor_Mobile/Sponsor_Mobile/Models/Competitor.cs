using System;
using System.Collections.Generic;
using System.Text;

namespace Sponsor_Mobile.Models
{
    public class Competitor
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string CountryName { get; set; }
        public string SkillName { get; set; }
        public string RequiredAmount { get; set; }
        public string Color { get; set; }
    }
}
