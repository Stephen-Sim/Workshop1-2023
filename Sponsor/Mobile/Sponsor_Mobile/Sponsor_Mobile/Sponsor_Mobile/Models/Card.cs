using System;
using System.Collections.Generic;
using System.Text;

namespace Sponsor_Mobile.Models
{
    public class Card
    {
        public string CardNo { get; set; }
        public Nullable<decimal> Amount { get; set; }
        public string CVV { get; set; }
    }
}
