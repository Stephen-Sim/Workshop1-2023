using System;
using System.Collections.Generic;
using System.Text;

namespace Sponsor_Mobile.Models
{
    public class Sponsorship
    {
        public string SponsorName { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; }
        public System.DateTime DateTime { get; set; }
        public long CompetitorId { get; set; }
        public long CurrencyId { get; set; }

    }
}
